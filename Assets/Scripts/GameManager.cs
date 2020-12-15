using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text ScoreLifeText;

    Player player;


    private void Start()
    {
        player = FindObjectOfType<Player>();
    }


    private void Update()
    {
        ScoreLifeText.text = player.playerLife.ToString();
        
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
