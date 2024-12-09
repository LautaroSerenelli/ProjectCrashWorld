using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float FuerzaRebote;

    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            Transform parentTransform = other.transform.parent;
            CharacterController characterController = parentTransform.GetComponent<CharacterController>();

            Vector3 initialVelocity = new Vector3(characterController.velocity.x, FuerzaRebote, characterController.velocity.z);
            StartCoroutine(SmoothBounce(characterController, initialVelocity, 0.5f));
        }
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