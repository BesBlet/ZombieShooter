using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 5f;
    
    
    Rigidbody2D rb;
    Animator animator;

    Player player;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        //Move();
        //Rotate();
    }
    
    private void Move()
    {

        Vector2 direction = new Vector2(0, 0);

        if (direction.magnitude > 1)
        {
            direction = direction.normalized;
        }

        rb.velocity = direction * speed;

        animator.SetFloat("Speed", direction.magnitude);

    }
    
    void Rotate()
    {
        Vector3 playerPosition = player.transform.position;
        playerPosition.z = 0;

        transform.LookAt(-playerPosition);

       
    }

}
