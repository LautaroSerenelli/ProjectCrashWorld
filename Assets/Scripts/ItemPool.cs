using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public GameObject itemPrefab;
    public int poolSize = 25;
    private List<GameObject> itemPool;

    public void Awake()
    {
        InitializePool();
    }

    void InitializePool()
    {
        itemPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            item.SetActive(false);
            itemPool.Add(item);
        }
    }

    public GameObject GetItemFromPool()
    {
        foreach (GameObject item in itemPool)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                return item;
            }
        }
        return null;
    }

    public void ReturnItemToPool(GameObject item)
    {
        item.SetActive(false);
    }
}