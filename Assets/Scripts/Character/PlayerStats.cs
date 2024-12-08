using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int initialHealth = 1;
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Lives Settings")]
    public int initialLives = 3;
    public int currentLives;

    [Header("Respawn Settings")]
    public float respawnDelay = 0.5f;
    private bool isInvulnerable = false;
    public bool isDead = false;

    [Header("Fruits Settings")]
    public int currentFruits = 0;
    public int fruitsForLive = 10;

    [Header("Referencias")]
    [SerializeField] private Transform respawnPoint;
    public GameObject damageParticlesPrefab;

    private PlayerController playerController;

    private void Start()
    {
        currentHealth = initialHealth;
        currentLives = initialLives;

        playerController = GetComponent<PlayerController>();
    }

    public void AddItem(int amount)
    {
        currentFruits += amount;

        if (currentFruits >= fruitsForLive)
        {
            GainLive(1);
            currentFruits -= fruitsForLive;
        }
    }

    public void GainLive(int amount)
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, int.MaxValue);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        isInvulnerable = true;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        TriggerDamageParticles();

        StartCoroutine(InvulnerableCoroutine());
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    private void Die()
    {
        isDead = true;
        currentLives--;

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            Respawn();
        }
    }

    private void TriggerDamageParticles()
    {
        if (damageParticlesPrefab != null)
        {
            GameObject particles = Instantiate(damageParticlesPrefab, transform.position, Quaternion.identity);
            
            Destroy(particles, 2f);
        }
    }

    private IEnumerator InvulnerableCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        if (currentHealth <= 0)
        {
            Die();
        }

        isInvulnerable = false;
    }

    private void Respawn()
    {
        playerController.ResetMovement();
        playerController.GetComponent<CharacterController>().enabled = false;
        transform.position = respawnPoint.position;
        currentHealth = initialHealth;
        isDead = false;
        StartCoroutine(ReactivateCharacterController());
    }

    private IEnumerator ReactivateCharacterController()
    {
        yield return new WaitForSeconds(0.1f);
        playerController.GetComponent<CharacterController>().enabled = true;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}