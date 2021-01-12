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
    public float attackRadius = 3;

    [Header("Gameplay config")]
    public float attackRate = 1f;
    public int health = 100;
    public int damage = 20;

    Player player;

    ZombieState activeState;

    CircleCollider2D zCollider;
    Animator animator;
    ZombieMovement movement;


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
        zCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        movement = GetComponent<ZombieMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        startPosition = transform.position;
        ChangeState(ZombieState.STAND);

        HealthChanged += Method1;
    }

    void Method1()
    {
        print("Health changed");
    }

    public void UpdateHealth(int amount)
    {
        health += amount;
        if(health <= 0)
        {
            isDead = true;
            animator.SetBool("Death", true);
            zCollider.enabled = false;
            //trigger animation death
        }
        HealthChanged(); //вызов события
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        UpdateHealth(-bullet.playerDamage);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
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

    private void ChangeState(ZombieState newState)
    {
        switch (newState)
        {
            case ZombieState.STAND:
                movement.enabled = false;
                break;
            case ZombieState.RETURN:
                movement.targetPosition = startPosition;
                movement.enabled = true;
                break;
            case ZombieState.MOVE_TO_PLAYER:
                movement.enabled = true;
                //Play move sound
                break;
            case ZombieState.ATTACK:
                movement.enabled = false;
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
        //if (distanceToPlayer < moveRadius)
        //{
        //    ChangeState(ZombieState.MOVE_TO_PLAYER);
        //    return;
        //}

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
            return;
        }
        if (distanceToPlayer > standbyRadius)
        {
            ChangeState(ZombieState.RETURN);
            return;
        }

        //move
        movement.targetPosition = player.transform.position;
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

    //IEnumerator AttackCoroutine()
    //{
    //    while (true)
    //    {
    //        animator.SetTrigger("Shoot");
    //        player.UpdateHealth(-damage);
    //        yield return new WaitForSeconds(attackRate);
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, standbyRadius);
    }
}