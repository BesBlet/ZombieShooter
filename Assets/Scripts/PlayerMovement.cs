using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    
    
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
        if (player.death)
        {
            return;
        }
        Move();
        Rotate();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(inputX, inputY);

        if (direction.magnitude > 1)
        {
            direction = direction.normalized;
        }

        rb.velocity = direction * speed;

        animator.SetFloat("Speed", direction.magnitude);

    }


    void Rotate()
    {
        Vector3 playerPosition = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePosition - playerPosition;

        direction.z = 0;
        transform.up = -direction;
    }
}
