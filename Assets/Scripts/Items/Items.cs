using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public string itemName = "Item";
    public int value = 1;
    public GameObject collectParticlesPrefab;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            CollectItem(playerStats);
        }
    }

    private void CollectItem(PlayerStats playerStats)
    {
        playerStats.AddItem(value);

        GameObject particles = Instantiate(collectParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(particles, 2f);

        gameObject.SetActive(false);
    }
}