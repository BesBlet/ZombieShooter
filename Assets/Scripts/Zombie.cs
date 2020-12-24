using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float moveRadius = 10;
    public float attackRarius = 5;
    public float visionRadius = 20;

    public float distanceToPlayer;

    public float attackRate = 3f;
    
    public int zombieHealth = 80;
    public int zombieDamage = 20;

    public bool returnToStart;

    float nextAttack;

    
    
    Player player;
    Bullet bullet;
    
    ZombieState activeState;
    
    Animator animator;
    
    ZombieMovement movement;
   

   enum ZombieState
    {
        STAND,
        MOVE,
        ATTACK,
        RETURN,
        DEATH
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<ZombieMovement>();
    }
    void Start()
    {
        player = FindObjectOfType<Player>();
        activeState = ZombieState.STAND;
        returnToStart = false;
    }



    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieState.STAND:
                DoStand(distanceToPlayer);
                //Stand
                break;
            
            case ZombieState.MOVE:
                DoMove(distanceToPlayer);
                //Move
                break;
            
            case ZombieState.ATTACK:
                DoAttack(distanceToPlayer);
                //Attack
                break;
            
            case ZombieState.RETURN:
                DoReturn(distanceToPlayer);
                //Return
                break;
            
            case ZombieState.DEATH:
                DoDeath();
                //Death
                break;
        }
    }

    void DoDeath()
    {
        if (zombieHealth <= 0)
        {
            activeState = ZombieState.DEATH;
            return;
        }
        movement.enabled = false;
        animator.SetBool("Death", true);
    }
    
    
    private void DoAttack(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRarius)
        {
            activeState = ZombieState.MOVE;
            return;
        }

        if (nextAttack > 0)
        {
            nextAttack -= Time.deltaTime;
        }

        if (nextAttack <= 0)
        {
            animator.SetTrigger("Shoot");
            nextAttack = attackRate;
            movement.enabled = false;
            DamageToPlayer();
            
        }
        
    }

    public void DamageToPlayer()
    {
        if (distanceToPlayer > attackRarius)
        {
            return;
        }
        player.UpdateHealth(-zombieDamage);    
    }

    
    private void DoMove(float distanceToPlayer)
    {
        if (distanceToPlayer < attackRarius)
        {
            activeState = ZombieState.ATTACK;
            return;
        }

        movement.enabled = true;
    }

    
    private void DoStand(float distanceToPlayer)
    {
        if (distanceToPlayer < moveRadius)
        {
            activeState = ZombieState.MOVE;
            return;
        }

        movement.enabled = false;
    }
    
    
    private void DoReturn(float distanceToPlayer)
    {
        if (distanceToPlayer > visionRadius)
        {
            activeState = ZombieState.RETURN;
            return;
        }

        returnToStart = true;
        movement.enabled = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRarius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, visionRadius);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player Bullet")) // 10 =  player bullet
        {
            bullet = collision.gameObject.GetComponent<Bullet>();

            if (zombieHealth > 0)
            {
                zombieHealth -= bullet.playerDamage;
            }
            Destroy(bullet.gameObject);
        }
    }
}
