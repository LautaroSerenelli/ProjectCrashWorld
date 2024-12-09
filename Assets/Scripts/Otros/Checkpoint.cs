using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private ParticleSystem checkpointParticles;
    [SerializeField] private ParticleSystem respawnUpdateParticles;
    [SerializeField] private GameObject checkpointEffectPrefab;

    private bool isCheckpointActivated = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCheckpointActivated)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.respawnPoint = transform;

                if(checkpointParticles != null)
                {
                    Vector3 spawnPosition = GetColliderCenter();
                    Instantiate(checkpointParticles, spawnPosition, Quaternion.identity);
                }
            }
            
            isCheckpointActivated = true;
        }
    }

    public void Start()
    {
        if (checkpointEffectPrefab != null)
        {
            Vector3 spawnPosition = GetColliderCenter();
            Instantiate(checkpointEffectPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void PlayRespawnParticles(Vector3 respawnPosition)
    {
        if (respawnUpdateParticles != null)
        {
            Vector3 spawnPosition = GetColliderCenter();
            ParticleSystem respawnParticlesInstance = Instantiate(respawnUpdateParticles, spawnPosition, Quaternion.identity);

            var main = respawnParticlesInstance.main;
            main.duration = 1f;
            respawnParticlesInstance.Play();
            Destroy(respawnParticlesInstance.gameObject, 1f);
        }
    }

    public Vector3 GetColliderCenter()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.center;
        }
        return transform.position;
    }
}