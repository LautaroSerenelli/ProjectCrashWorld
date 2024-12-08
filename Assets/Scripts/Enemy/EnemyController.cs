using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    void Start()
    {
        enemyPool = FindObjectOfType<EnemyPool>(); // Encuentra el pool de enemigos en la escena
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        
        // Verificar si el enemigo murió
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private EnemyPool enemyPool;

    void Die()
    {
        enemyPool.ReturnEnemyToPool(gameObject);
    }
}