using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform TriggerDestino;

    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            if (TriggerDestino != null)
            {
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.characterController.enabled = false;
                }
                other.transform.position = TriggerDestino.position;
                if (playerController != null)
                {
                    playerController.characterController.enabled = true;
                }
            }
        }
    }
}