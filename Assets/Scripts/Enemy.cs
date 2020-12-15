using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Bullet bulletPrefab;
    public GameObject shootPosition;

    public float fireRate = 1f;
    public int enemyLife = 75;
    float nextFire;
    
    Animator animator;

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


}
