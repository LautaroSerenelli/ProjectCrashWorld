using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public PlayerStats playerStats;

    public TextMeshProUGUI fruitText;
    
    public void Update()
    {
        lifeText.text = playerStats.currentLives.ToString();
        fruitText.text = playerStats.currentFruits.ToString();
    }
}