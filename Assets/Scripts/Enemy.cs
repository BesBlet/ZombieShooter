using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Bullet bulletPrefab;
    public GameObject shootPosition;

    public float fireRate = 3f;
    public int enemyLife = 75;
    float nextFire;

    Animator animator;
    Bullet bullet;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (enemyLife <= 0)
        {
            Death();
        }
        else
        {
            CheckFire();
        }

    }


    private void CheckFire()
    {
        if (nextFire <= 0)
        {
            Shoot();
        }
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        animator.SetTrigger("Shoot");
        // TODO sound
        Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);

        nextFire = fireRate;
    }


    void Death()
    {
        animator.SetBool("Death", true);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {



        if (collision.gameObject.layer == LayerMask.NameToLayer("Player Bullet")) // 10 =  player bullet
        {
           bullet = collision.gameObject.GetComponent<Bullet>();

            if (enemyLife > 0)
            {
                enemyLife -= bullet.playerDamage;
            }
            Destroy(bullet.gameObject);
        }


    }
}
