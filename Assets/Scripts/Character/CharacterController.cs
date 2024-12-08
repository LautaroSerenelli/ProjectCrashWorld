using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Idle
    public float timeUntilShortIdle = 5f;
    private float idleTimer = 0f;
    private bool isShortIdle = false;
    private bool isWaitingForIdle = false;

    // Speed
    public float minMoveSpeed = 7f;
    public float maxMoveSpeed = 15f;
    public float acceleration = 5f;
    private float currentMoveSpeed = 0f;

    // Jump
    public float jumpForce = 10f;
    public float gravity = 20f;
    public float jumpHoldGravity = 6f;
    public float maxFallSpeed = -20f;
    public float maxJumpHoldTime = 0.2f;

    public LayerMask groundLayer;

    // Spin
    public float attackRadius = 1.75f;
    public int attackDamage = 1;

    // Slide
    private bool isSliding = false;
    public float slideSpeed = 20f;
    private float slideDuration = 0.5f;
    private bool canSlide = true;
    private bool wasIdleBeforeSlide = true;

    // Body Slam
    private bool isBodySlam = false;
    private bool canBodySlam = true;
    private bool hasLanded = false;
    private bool isImpacted = false;

    private CharacterController characterController;
    private Transform mainCameraTransform;
    private Vector3 moveDirection;

    private bool isIdle;
    private bool isGrounded;
    private bool isJumping;
    private bool isSpinning = false;
    private bool isMoving;

    private float verticalSpeed;
    private float jumpStartTime;
    private float preJumpHorizontalSpeed;

    private Animator animator;
    private Vector3 centerPosition;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCameraTransform = Camera.main.transform;
        characterController.center = new Vector3(characterController.center.x, characterController.height / 2f, characterController.center.z);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDirection = forward * verticalInput + right * horizontalInput;

        isMoving = targetDirection.magnitude >= 0.1f;
        bool isIdle = !isMoving && isGrounded && !isSliding && !isBodySlam;

        if (!isSpinning && !isBodySlam)
        {
            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            animator.SetBool("isWalking", isMoving && isGrounded && currentMoveSpeed < maxMoveSpeed && !isSliding);
            animator.SetBool("isRunning", isMoving && isGrounded && currentMoveSpeed >= maxMoveSpeed && !isSliding);
            if (!hasLanded && !isImpacted && !isBodySlam && !isShortIdle)
            {
                animator.SetBool("isIdle", isGrounded && !isMoving && !isSliding);
            }
        }
        else
        {
            // Desactivar otras animaciones
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);   
        }

        // Idle
        if (isIdle)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= timeUntilShortIdle && !isShortIdle)
            {
                ChangeToShortIdle();
            }
        }

        // Aceleración
        if (isMoving && currentMoveSpeed < maxMoveSpeed && !isSpinning && !isBodySlam)
        {
            currentMoveSpeed += acceleration * Time.deltaTime;
        }
        else if (!isMoving)
        {
            currentMoveSpeed = 0f;
        }

        currentMoveSpeed = Mathf.Max(currentMoveSpeed, minMoveSpeed);

        if (!isBodySlam)
        {
            moveDirection = (forward * verticalInput + right * horizontalInput).normalized * currentMoveSpeed;
        }

        isGrounded = characterController.isGrounded;

        // Jump
        if (isGrounded && !isImpacted)
        {
            isJumping = false;

            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                verticalSpeed = jumpForce;
                jumpStartTime = Time.time;
                preJumpHorizontalSpeed = Mathf.Sqrt(Mathf.Pow(characterController.velocity.x, 2) + Mathf.Pow(characterController.velocity.z, 2));

                if (isMoving)
                {
                    verticalSpeed += preJumpHorizontalSpeed;
                }

                animator.SetTrigger("jump");
            }
        }

        if (isJumping && Input.GetButton("Jump"))
        {
            float jumpTimeElapsed = Time.time - jumpStartTime;
            float adjustedJumpForce = jumpForce - jumpTimeElapsed * 2f;

            verticalSpeed = adjustedJumpForce;

            if (verticalSpeed > 0)
            {
                verticalSpeed -= jumpHoldGravity * Time.deltaTime;
            }

            if (jumpTimeElapsed >= maxJumpHoldTime)
            {
                isJumping = false;
            }
        }

        if (!isGrounded)
        {
            verticalSpeed -= gravity * Time.deltaTime;
            verticalSpeed = Mathf.Max(verticalSpeed, maxFallSpeed);
        }

        moveDirection.y = verticalSpeed;

        characterController.Move(moveDirection * Time.deltaTime);

        // Spin
        if (Input.GetMouseButtonDown(0) && !isSpinning && !isBodySlam)
        {
            PerformSpinAttack();
        }

        if (!isMoving && isGrounded)
        {
            isIdle = true;
            if (isSpinning)
            {
                animator.SetBool("isIdleTrigger", true);
            }
            else
            {
                animator.SetBool("isIdleTrigger", false);
            }
        }

        // Slide
        if (canSlide && Input.GetMouseButtonDown(1) && isGrounded && currentMoveSpeed > 0 && !isBodySlam)
        {
            wasIdleBeforeSlide = isIdle;
            PerformSlide();
        }

        // Body Slam
        if (isBodySlam)
        {
            moveDirection = Vector3.zero;
        }

        if (canBodySlam && Input.GetMouseButtonDown(1) && !isGrounded)
        {
            PerformBodySlam();
        }
    }

    // Spin
    void PerformSpinAttack()
    {
        isSpinning = true;
        animator.SetTrigger("spinAttack");

        float yOffset = characterController.height / 2f;
        centerPosition = transform.position + transform.up * yOffset;

        StartCoroutine(SpinAttackRoutine());

        StartCoroutine(ResetSpinState());
    }

    IEnumerator SpinAttackRoutine()
    {
        while (isSpinning)
        {
            centerPosition = transform.position + transform.up * (characterController.height / 2f);
            PerformAreaAttack(centerPosition, attackRadius, attackDamage);
            yield return null;
        }

    }

    IEnumerator ResetSpinState()
    {
        if (isIdle)
        {
            yield return new WaitForSeconds(0.7f);
            isSpinning = false;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            isSpinning = false;
        }
    }

    // Slide
    void PerformSlide()
    {
        isSliding = true;
        canSlide = false;
        animator.SetTrigger("slide");
        StartCoroutine(SlideAttackRoutine());
        StartCoroutine(ResetSlideState());
    }

    IEnumerator SlideAttackRoutine()
    {
        while (isSliding)
        {
            centerPosition = transform.position + transform.up * (characterController.height / 2f);
            PerformAreaAttack(centerPosition, attackRadius, attackDamage);
            yield return null;
        }
    }

    IEnumerator ResetSlideState()
    {
        yield return new WaitForSeconds(slideDuration);
        isSliding = false;
        canSlide = true;
    }

    // Body Slam
    void PerformBodySlam()
    {
        isBodySlam = true;
        canBodySlam = false;
        animator.SetTrigger("bodySlam");
        verticalSpeed = jumpForce;
        currentMoveSpeed = 0f;
        StartCoroutine(BodySlamAttackRoutine());
        StartCoroutine(ResetBodySlamState());
    }

    IEnumerator BodySlamAttackRoutine()
    {
        while (isBodySlam)
        {
            centerPosition = transform.position + transform.up * (characterController.height / 2f);
            PerformAreaAttack(centerPosition, attackRadius * 1.5f, attackDamage * 2);
            yield return null;
        }
    }

    IEnumerator ResetBodySlamState()
    {
        yield return new WaitUntil(() => characterController.isGrounded);
        hasLanded = true;
    }

    IEnumerator ResetImpactState()
    {
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("bodySlamEnd");
        isBodySlam = false;
        canBodySlam = true;
        isImpacted = false;
    }

    // Idle
    void ChangeToShortIdle()
    {
        isShortIdle = true;
        isWaitingForIdle = true;
        animator.SetBool("isIdle", false);
        animator.SetTrigger("idleShort01");

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Warrior Idle")
            {
                StartCoroutine(WaitForShortIdleAnimation(clip.length));
                break;
            }
        }
    }

    IEnumerator WaitForShortIdleAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (isWaitingForIdle)
        {
            if (!isMoving && isGrounded && !isSliding && !isBodySlam && !isSpinning)
            {
                animator.SetBool("isIdle", true);
            }

            isShortIdle = false;
            idleTimer = 0f;
            isWaitingForIdle = false;
        }
    }

    void PerformAreaAttack(Vector3 attackCenter, float attackRadius, int damage)
    {
        Collider[] colliders = Physics.OverlapSphere(attackCenter, attackRadius);

        foreach (Collider collider in colliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (collider.CompareTag("Crates"))
            {
                Crates crate = collider.GetComponent<Crates>();
                crate.Break();
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color (0f, 0f, 1f, 0.5f);
        Gizmos.DrawSphere(centerPosition, attackRadius);
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            Vector3 slideDirection = transform.forward;

            float modifiedSlideSpeed = wasIdleBeforeSlide ? slideSpeed * 2f : slideSpeed;

            Vector3 slideVelocity = slideDirection * modifiedSlideSpeed;
            characterController.Move(slideVelocity * Time.fixedDeltaTime);
        }
   
        if (isBodySlam)
        {
            moveDirection = Vector3.zero;
            animator.SetBool("isIdle", false);
        }
        
        if (hasLanded)
        {
            animator.SetTrigger("bodySlamImpact");
            isImpacted = true;
            PerformAreaAttack(centerPosition, attackRadius, attackDamage * 2);
            StartCoroutine(ResetImpactState());
            hasLanded = false;
        }
    }

    public void ResetMovement()
    {
        verticalSpeed = 0f;
        moveDirection = Vector3.zero;
    }
}