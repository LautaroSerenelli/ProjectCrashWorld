using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool enemyPool;
    public Transform spawnPoint;
    public bool singleUse = true;
    private bool hasSpawned = false;
    public int enemyAmount = 1;

    public Vector3 spawnOffsetRange = new Vector3(5f, 0f, 5f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (singleUse && hasSpawned)
            {
                return;
            }

            for (int i = 0; i < enemyAmount; i++)
            {
                GameObject enemy = enemyPool.GetEnemyFromPool();

                if (enemy != null)
                {
                    Vector3 randomPosition = spawnPoint.position + new Vector3(
                        Random.Range(-spawnOffsetRange.x, spawnOffsetRange.x),
                        Random.Range(-spawnOffsetRange.y, spawnOffsetRange.y),
                        Random.Range(-spawnOffsetRange.z, spawnOffsetRange.z)
                    );

                    enemy.transform.position = randomPosition;
                    enemy.transform.rotation = spawnPoint.rotation;

                    enemyPool.PlaySpawnEffect(randomPosition);  
                }
                else
                {
                    Debug.Log("No hay enemigos disponibles en el pool.");
                }
            }

            if (singleUse)
            {
                hasSpawned = true;
            }
        }
    }
}