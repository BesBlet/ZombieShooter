using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    Rigidbody2D rb;

   public int playerDamage = 40;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    
    {
        rb.velocity = -transform.up * speed;
    }

    private void OnBecameInvisible()
    { 
        LeanPool.Despawn(gameObject);
    }
    
}
