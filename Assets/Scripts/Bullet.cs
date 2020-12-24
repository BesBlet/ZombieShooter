using System;
using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {
        rb.velocity = -transform.up * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        
        //if (collision.gameObject.layer == 8) // 8 = Enemy 
        //{

        //    if (collision.GetComponent<Enemy>())
        //    {
        //        Enemy enemy = FindObjectOfType<Enemy>();

        //        if (enemy.enemyLife > 0)
        //        {
        //            enemy.enemyLife -= playerDamage;
        //        }

        //        Destroy(gameObject);
        //    }
            
        //}

        
    }
}
