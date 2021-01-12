using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text playerHealthText;
    public GameObject rebootPanel;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerHealthText.text = player.playerHealth.ToString();
        
        player.playerIsDeath += rebootPanelView;
        player.HealthChanged += UpdateHealth;
        
        Time.timeScale = 1f;
    }


    public void rebootPanelView()
    {
        Time.timeScale = 0f;
        rebootPanel.SetActive(true);
    }


    public void UpdateHealth()
    {
        playerHealthText.text = player.playerHealth.ToString();
    }

    public void RebootButton()
    {
        SceneManager.LoadScene(0);
    }
    
    
}