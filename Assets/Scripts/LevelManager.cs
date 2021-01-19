using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
   int sceneIndex;

   GameManager gameManager;
   public GameObject playButtton;
   private void Start()
   {
      gameManager = FindObjectOfType<GameManager>();
      gameManager.IsRebootScene += RebootLVL;
   }

   public void PlayButtonOnClick()
   {
      StartCoroutine(PlayButtonAnim());
   }

   IEnumerator PlayButtonAnim()
   {
      yield return new WaitForSeconds(1);
      NextLVL();
   }
   public void NextLVL()
   {
      sceneIndex = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(sceneIndex + 1);
   }
   public void RebootLVL()
   {
     SceneManager.LoadScene(sceneIndex);
   }
}
