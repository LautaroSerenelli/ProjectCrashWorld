using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int savedHealth;
    public int savedLives;
    public int savedFruits;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerData(int health, int lives, int fruits)
    {
        savedHealth = health;
        savedLives = lives;
        savedFruits = fruits;
    }

    public void LoadPlayerData(PlayerStats playerStats)
    {
        playerStats.currentHealth = savedHealth;
        playerStats.currentLives = savedLives;
        playerStats.currentFruits = savedFruits;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Nivel1")
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats != null)
            {
                LoadPlayerData(playerStats);
            }
        }
    }

    public void ResetPlayerData()
    {
        savedHealth = 1;
        savedLives = 3;
        savedFruits = 0;
    }
}