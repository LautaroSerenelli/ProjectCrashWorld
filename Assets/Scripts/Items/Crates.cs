using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crates : MonoBehaviour
{
    public ItemPool fruitPool;
    public int minFruits = 1;
    public int maxFruits = 5;
    
    public void Awake()
    {
        GameObject itemPoolObject = GameObject.Find("FruitPool");
        if (itemPoolObject != null)
        {
            fruitPool = itemPoolObject.GetComponent<ItemPool>();
        }
    }

    public void Break()
    {
        int fruitsToSpawn = Random.Range(minFruits, maxFruits);
        for (int i = 0; i < fruitsToSpawn; i++)
        {
            GameObject fruit = fruitPool.GetItemFromPool();
            // Invoke("DestroyBox", 0.01f);
            if (fruit != null)
            {
                Vector3 dropPosition = transform.position;
                dropPosition.y = dropPosition.y + 0.5f;
                fruit.transform.position = dropPosition;
                fruit.SetActive(true);

                Fruits fruits = fruit.GetComponent<Fruits>();
                if (fruits != null)
                {
                    fruits.Launch();
                }
            }
        }
        DestroyBox();
    }

    public void DestroyBox()
    {
        gameObject.SetActive(false);
    }
}