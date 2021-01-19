using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action playerIsDeath = delegate { };
    public Action HealthChanged = delegate { };

    [Header("Player SFX")]
    public AudioSource playerDeathSound;
    
    [Header("Pistol SFX")]
    public AudioSource pistolShot;
    public AudioSource pistolReloadStart;
    public AudioSource pistolReloadEnd;
    public AudioSource pistolEmptyMagazine;
    public AudioSource pistolCartridgeDrop;
    
    
    public Bullet bulletPrefab;
    public GameObject shootPosition;

    public float fireRate = 1f;
    float nextFire;

    public int magazineCapacity = 7;
    private int capacity;
    public int totalBulletNumber = 49;
    
    public int playerHealth = 100;
    public bool death;

    Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        capacity = magazineCapacity;
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
        
        if (playerHealth < 0)
        {
            return;
        }

        if (playerHealth <= 0)
        {
           
            Death();
        }
        
        HealthChanged();
    }
   

    private void CheckFire()
    {
        if (Input.GetButton("Fire1") && nextFire <= 0 && magazineCapacity > 0)
        {
            Shoot();
            magazineCapacity--;
            
            
        }

        if (magazineCapacity <= 0)
        {
            StartCoroutine(CheckPistolReload());
        }
        else
        {
            StopCoroutine(CheckPistolReload());
        }

        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        
        
        CheckNumberAmmo();
        
    }
    

    void CheckNumberAmmo()
    {
        if (totalBulletNumber <= 0)
        {
            pistolEmptyMagazine.Play();
        }
    }

    IEnumerator CheckPistolReload()
    {
        pistolReloadStart.Play();
        totalBulletNumber -= capacity;
        yield return new WaitForSeconds(1);
        magazineCapacity += capacity;
        pistolReloadEnd.Play();
    }

    private void Shoot()
    {
        animator.SetTrigger("Shoot");
        
        pistolShot.Play();
       
        Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);
        
        pistolCartridgeDrop.Play();
        
        nextFire = fireRate;
    }


   public void Death()
   {
       death = true;
       print("Player Death");
       animator.SetBool("Death", true);
       playerDeathSound.Play();
       playerIsDeath();
   }
}
