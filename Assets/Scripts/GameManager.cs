using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Action IsRebootScene = delegate { };
    
    [Header("Player health UI")]
    public Text playerHealthText;
    
    [Header("Ammo UI")]
    public Text ammoText;
    public Text totalAmmoText;
    public GameObject rebootPanel;

    Player player;

    private void Start()
    {
        player = Player.Instance;

        playerHealthText.text = player.playerHealth.ToString();
        
        ammoText.text = player.magazineCapacity.ToString();
        totalAmmoText.text = player.totalAmmoNumber.ToString();
        
        player.AmmoIsChanged += AmmoUpdate;
        player.TotalAmmoIsChanged += TotalAmmoUpdate;
        
        player.PlayerIsDeath += rebootPanelView;
        player.HealthChanged += UpdateHealth;
        
        Time.timeScale = 1f;
    }

    private void TotalAmmoUpdate()
    {
        totalAmmoText.text = player.totalAmmoNumber.ToString();
    }

    private void AmmoUpdate()
    {
        ammoText.text = player.magazineCapacity.ToString();
        if (player.magazineCapacity < 3)
        {
            ammoText.color = Color.red;
        }
        else if (player.magazineCapacity == player.capacity)
        {
            ammoText.color = Color.white;
        }
    }


    public void rebootPanelView()
    {
        StartCoroutine(WaitPanel(2));
       
    }

    IEnumerator WaitPanel(int delay)
    {
        
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(delay);
        rebootPanel.SetActive(true);

    }
    public void UpdateHealth()
    {
        playerHealthText.text = player.playerHealth.ToString();

        if (player.playerHealth > 80)
        {
            playerHealthText.color = Color.green;
        }
        
        else if (player.playerHealth < 80 && player.playerHealth > 40)
        {
            playerHealthText.color = Color.yellow;
        }
        
        else if (player.playerHealth < 40 )
        {
            playerHealthText.color = Color.red;
        }
    }

    public void RebootButton()
    {
        IsRebootScene();
    }
    
    public void ExitButton()
    {
        Application.Quit();
    }


    public void SettingsButton()
    {
        
    }
    
}