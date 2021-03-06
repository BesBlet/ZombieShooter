﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float speed = 5f;


    Rigidbody2D rb;
    Animator animator;
    Zombie zombie;

    public Vector3 targetPosition;

    Player player;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        zombie = GetComponent<Zombie>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        Move();
        Rotate();
    }

    private Vector3 Position()
    {
        Vector3 zombiePosition = transform.position;
        Vector3 direction = zombiePosition - targetPosition;
        return direction;

    }

    private void Move()
    {
        Vector3 direction = Position();

        if (direction.magnitude > 1)
        {
            direction = direction.normalized;
        }

        rb.velocity = -direction * speed;

        animator.SetFloat("Speed", direction.magnitude);

    }

    void Rotate()
    {
        Vector3 direction = Position();

        direction.z = 0;
        transform.up = direction;

    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }
}
