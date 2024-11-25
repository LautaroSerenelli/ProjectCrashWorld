using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnEffectPrefab;
    public int poolSize = 10;
    private List<GameObject> enemyPool;

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        enemyPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    public GameObject GetEnemyFromPool()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);

                return enemy;
            }
        }
        return null; // Si no hay enemigos disponibles en el pool
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    public void PlaySpawnEffect(Vector3 position)
    {
        if (spawnEffectPrefab != null)
        {
            GameObject effect = Instantiate(spawnEffectPrefab, position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }
}