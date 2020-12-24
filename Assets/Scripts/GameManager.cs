using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text ScoreHealthText;
    public Text ZombieHealthText;

    Player player;
    Zombie zombie;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        zombie = FindObjectOfType<Zombie>();
    }


    private void Update()
    {
        ScoreHealthText.text = player.playerHealth.ToString();
        ZombieHealthText.text = zombie.zombieHealth.ToString();
        
        
        if( player.death == true )
        {
            StartCoroutine(LevelReboot());
        }
        
        
    }
    
    
    IEnumerator LevelReboot()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
