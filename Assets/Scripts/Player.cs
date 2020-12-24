using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Bullet bulletPrefab;
    public GameObject shootPosition;

    public float fireRate = 1f;
    float nextFire;
    
    public int playerHealth = 100;
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
        
        CheckFire();
        
    }

    public void UpdateHealth(int amount)
    {
        playerHealth += amount;
            
        if (playerHealth <= 0)
        {
           
            Death();
        }
            
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
       death = true;
       print("Player Death");
       animator.SetBool("Death", true);
   }
}
