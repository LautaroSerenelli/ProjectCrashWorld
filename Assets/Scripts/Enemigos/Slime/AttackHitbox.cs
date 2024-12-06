using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private Slime slime;

    public void Initialize(Slime slime)
    {
        this.slime = slime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.TakeDamage(slime.damage);
            }
        }
    }
}