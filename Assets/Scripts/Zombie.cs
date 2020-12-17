using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float moveRadius = 10;
    public float attackRarius = 5;

    Player player;

    ZombieState activeState;

    Animator animator;

    ZombieMovement movement;

   enum ZombieState
    {
        STAND,
        MOVE,
        ATTACK
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
    }



    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieState.STAND:
                if (distance < moveRadius)
                {
                    activeState = ZombieState.MOVE;
                    return;
                }
                movement.enabled = false;
                //Stand
                break;
            case ZombieState.MOVE:
                if(distance < attackRarius)
                {
                    activeState = ZombieState.ATTACK;
                    return;
                }
                movement.enabled = true;
                //Move
                break;
            case ZombieState.ATTACK:
                if (distance > attackRarius)
                {
                    activeState = ZombieState.MOVE;
                    return;
                }
                animator.SetTrigger("Shoot");
                movement.enabled = false;
                //Attack
                break;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRarius);

    }
}
