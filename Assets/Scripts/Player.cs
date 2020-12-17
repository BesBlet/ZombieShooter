using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Bullet bulletPrefab;
    public GameObject shootPosition;

    public float fireRate = 1f;
    float nextFire;
    
    public int playerLife = 100;
    public bool death;

    Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        death = false;
    }

    
    void Update()
    {
        if (death)
        {
            return;
        }
        
        if (playerLife <= 0)
        {
            death = true;
            print("Player Death");
            Death();
        }
        
        CheckFire();
        
    }

   

    private void CheckFire()
    {
        if (Input.GetButton("Fire1") && nextFire <= 0)
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


   public void Death()
   {
       animator.SetBool("Death", true);
   }
}
