using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemPool itemPool;
    public Transform[] spawnPoints;

    private void Start()
    {
        SpawnItems(spawnPoints.Length);
    }

    public void SpawnItems(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject item = itemPool.GetItemFromPool();

            if (item != null)
            {
                Transform spawnPoint = spawnPoints[i];
                item.transform.position = spawnPoint.position;
                item.transform.rotation = spawnPoint.rotation;
            }
            else
            {
                Debug.Log("No hay ítems disponibles en el pool.");
            }
        }
    }
}