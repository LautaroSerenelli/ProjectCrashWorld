using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCloseState : IEnemyState
{
    private Slime slime;
    private float attackTimer;
    private bool isPreparingAttack = false;
    private bool isAttacking = false;
    private bool isLongRangeAttack = false;

    private GameObject attackHitbox;
    private BoxCollider attackCollider;

    public AttackCloseState(Slime slime)
    {
        this.slime = slime;
    }

    public void EnterState()
    {
        attackTimer = slime.attackCooldown;
        isPreparingAttack = false;
        isAttacking = false;
        isLongRangeAttack = false;

        slime.animator.SetBool("IdleBattle", true);
        slime.animator.SetBool("RunFWD", false);
        slime.animator.SetBool("WalkFWD", false);

        CreateAttackHitbox();
    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(slime.transform.position, slime.player.position);

        if (distanceToPlayer > slime.attackRangeLong)
        {
            slime.SetState(Slime.EnemyState.Pursuit);
            return;
        }

        if (distanceToPlayer > slime.attackRangeClose && distanceToPlayer <= slime.attackRangeLong)
        {
            isLongRangeAttack = true;
        }
        else if (distanceToPlayer <= slime.attackRangeClose)
        {
            isLongRangeAttack = false;
        }

        if (!isPreparingAttack && !isAttacking)
        {
            FollowPlayerWithLook();

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                PrepareAttack();
            }
        }
        else if (isAttacking)
        {
            string attackAnimation = isLongRangeAttack ? "Attack02" : "Attack01";
            var animatorStateInfo = slime.animator.GetCurrentAnimatorStateInfo(0);

            if (animatorStateInfo.IsName(attackAnimation) && animatorStateInfo.normalizedTime >= 1.0f)
            {
                ResetAttack();
            }
            else if (animatorStateInfo.IsName(attackAnimation) && animatorStateInfo.normalizedTime >= 0.5f)
            {
                ActivateAttackHitbox();
            }
        }
    }

    private void FollowPlayerWithLook()
    {
        if (!isAttacking)
        {
            Vector3 directionToPlayer = (slime.player.position - slime.transform.position).normalized;
            slime.transform.rotation = Quaternion.Slerp(slime.transform.rotation,
                Quaternion.LookRotation(directionToPlayer),
                Time.deltaTime);
        }
    }

    private void PrepareAttack()
    {
        isPreparingAttack = true;

        slime.StartCoroutine(WaitBeforeAttack(0.5f));
    }

    private IEnumerator WaitBeforeAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        isAttacking = true;
        slime.animator.SetBool("IdleBattle", false);
        Debug.Log(isLongRangeAttack);
        slime.animator.SetTrigger(isLongRangeAttack ? "Attack02" : "Attack01");
    }

    private void ResetAttack()
    {
        isAttacking = false;
        attackTimer = slime.attackCooldown;
        slime.animator.SetBool("IdleBattle", true);
        EnterState();
    }

    private void CreateAttackHitbox()
    {
        if (attackHitbox == null)
        {
            attackHitbox = new GameObject("AttackHitbox");
            attackHitbox.transform.SetParent(slime.transform);
            attackHitbox.transform.localPosition = new Vector3(0, 0, 1);

            attackCollider = attackHitbox.AddComponent<BoxCollider>();
            attackCollider.isTrigger = true;

            attackCollider.size = new Vector3(1, 1, 6);

            attackCollider.center = new Vector3(0, 0, attackCollider.size.z / 2);

            AttackHitbox hitboxScript = attackHitbox.AddComponent<AttackHitbox>();
            hitboxScript.Initialize(slime);

            attackHitbox.SetActive(false);
        }
    }

    private void ActivateAttackHitbox()
    {
        Vector3 directionToPlayer = (slime.player.position - slime.transform.position).normalized;
        attackHitbox.transform.rotation = Quaternion.LookRotation(directionToPlayer);

        attackHitbox.SetActive(true);

        slime.StartCoroutine(DeactivateHitboxAfterAttack());
    }

    private IEnumerator DeactivateHitboxAfterAttack()
    {
        yield return new WaitForSeconds(0.1f);
        attackHitbox.SetActive(false);
    }
}