using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bomb : MonoBehaviour
{
    private Animator animator;

    public GameObject explosionEffectPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
   {
      Explode();
   }


   void Explode()
   {
       Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
       Destroy(gameObject);
   }
}
