using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuerteCaida : MonoBehaviour
{
    public int damageAmount = 10;

    private PlayerStats playerStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = other.gameObject.GetComponent<PlayerStats>();
        
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }
        }
    }
}