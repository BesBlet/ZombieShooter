﻿using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Action HealthChanged = delegate { }; //delegate { } - пустое действие, чтобы не было ошибки в случае, если никто не подпишется

    [Header("AI config")]
    public float moveRadius = 10;
    public float standbyRadius = 15;
    public float attackRadius = 6;
    public int zAngle = 90;

    [Header("Gameplay config")]
    public float attackRate = 1f;
    public int health = 100;
    public int damage = 20;

    [Header("Zombie SFX")] 
    
    public AudioSource pistolDamage;
    public AudioSource finalPistolDamage;

    Player player;
    LevelManager levelManager;

    ZombieState activeState;

    Rigidbody2D rb;
    CircleCollider2D zCollider;
    Animator animator;
    AIDestinationSetter aIDestinationSetter;
    AIPath aIPath;



    float nextAttack; //через сколько времени можно произвести следующую атаку
    float distanceToPlayer;

    bool isDead = false;

    Vector3 startPosition;

    enum ZombieState
    {
        STAND,
        RETURN,
        MOVE_TO_PLAYER,
        ATTACK
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        zCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        aIPath = GetComponent<AIPath>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;

        levelManager = FindObjectOfType<LevelManager>();

        startPosition = transform.position;
        ChangeState(ZombieState.STAND);

        levelManager.ZombieAmount();
    }

    void Update()
    {
        if (isDead)
        {
            aIPath.enabled = false;
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieState.STAND:
                DoStand();
                break;
            case ZombieState.RETURN:
                DoReturn();
                break;
            case ZombieState.MOVE_TO_PLAYER:
                DoMove();
                break;
            case ZombieState.ATTACK:
                DoAttack();
                break;
        }
    }

    public void UpdateHealth(int amount)
    {
        health += amount;
        if(health <= 0)
        {
            isDead = true;
            animator.SetBool("Death", true);
            zCollider.enabled = false;
            rb.isKinematic = false;
            levelManager.ZombieLVLReboot();
            
        }
        HealthChanged(); //вызов события
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        
        if (health <= bullet.playerDamage)
        {
            finalPistolDamage.Play();
        }
        else
        { 
            pistolDamage.Play();
        }
        
        UpdateHealth(-bullet.playerDamage);
        
    }
    
   

    private void ChangeState(ZombieState newState)
    {
        switch (newState)
        {
            case ZombieState.STAND:
                aIPath.enabled = false;
                break;
            case ZombieState.RETURN:
                aIPath.enabled = true;
                break;
            case ZombieState.MOVE_TO_PLAYER:
                aIDestinationSetter.target = player.transform;
                aIPath.enabled = true;
                //Play move sound
                break;
            case ZombieState.ATTACK:
                aIPath.enabled = false;
                break;
        }
        activeState = newState;
    }

    
    
    private void DoStand()
    {
        CheckMoveToPlayer();
    }

    private void DoReturn()
    {
        if (CheckMoveToPlayer())
        {
            return;
        }

        float distanceToStart = Vector3.Distance(transform.position, startPosition);
        if (distanceToStart <= 0.05f) 
        {
            ChangeState(ZombieState.STAND);
            return;
        }
    }

    private bool CheckMoveToPlayer()
    {
        //проверям радиус
        if (distanceToPlayer > moveRadius)
        {
            return false;
        }


        //проверям препятствия
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);

        float angle = Vector3.Angle(-transform.up, directionToPlayer);
        if(angle > zAngle/2)
        {
            return false;
        }

        LayerMask layerMask = LayerMask.GetMask("Obstacles");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, layerMask);
        if(hit.collider != null)
        {
            //есть коллайдер
            return false;
        }


        //бежать за игроком
        ChangeState(ZombieState.MOVE_TO_PLAYER);
        return true;
    }

    private void DoMove()
    {
        if (distanceToPlayer < attackRadius)
        {
            ChangeState(ZombieState.ATTACK);
            animator.SetFloat("Speed", 0);
            return;
        }
        if (distanceToPlayer > standbyRadius)
        {
            ChangeState(ZombieState.RETURN);
            animator.SetFloat("Speed", 0);
            return;
        }


        animator.SetFloat("Speed", 1);
        //move

    }
    private void DoAttack()
    {
        if (distanceToPlayer > attackRadius)
        {
            ChangeState(ZombieState.MOVE_TO_PLAYER);
            StopAllCoroutines();
            return;
        }

        nextAttack -= Time.deltaTime;
        if (nextAttack <= 0)
        {
            animator.SetTrigger("Shoot");

            nextAttack = attackRate;
        }
    }

    public void DamageToPlayer()
    {
        if (distanceToPlayer > attackRadius)
        {
            return;
        }
        player.UpdateHealth(-damage);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, standbyRadius);

        Gizmos.color = Color.cyan;
        Vector3 lookDirection = -transform.up;
        Vector3 leftViewVector = Quaternion.AngleAxis(zAngle / 2, Vector3.forward) * lookDirection;
        Vector3 rightViewVector = Quaternion.AngleAxis(-zAngle / 2, Vector3.forward) * lookDirection;

        Gizmos.DrawRay(transform.position, lookDirection * moveRadius);
       
    }
}