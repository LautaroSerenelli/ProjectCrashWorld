using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPiesDetector : MonoBehaviour
{
    private PlayerController player;
    private float previousYPosition;
    
    public void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public void Update()
    {
        previousYPosition = player.transform.position.y;
    }

    public void OnTriggerEnter (Collider other)
    {
        float currentYPosition = player.transform.position.y;

        if (player.isJumping && currentYPosition < previousYPosition && other.CompareTag("Crates"))
        {
            Crates crate = other.GetComponent<Crates>();
            if (crate != null)
            {
                crate.Break();
                ApplyJumpBoost();
            }
        }

        previousYPosition = currentYPosition;
    }

    private void ApplyJumpBoost()
    {
        Vector3 bounceVelocity = new Vector3(0, player.jumpForce * 5f, 0);

        StartCoroutine(SmoothBounce(player.characterController, bounceVelocity, 0.5f));
    }

    private IEnumerator SmoothBounce(CharacterController characterController, Vector3 initialVelocity, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float decayFactor = Mathf.Exp(-elapsedTime * 3f);
            Vector3 currentVelocity = initialVelocity * decayFactor;

            characterController.Move(currentVelocity * Time.deltaTime);
            yield return null;
        }
    }
}