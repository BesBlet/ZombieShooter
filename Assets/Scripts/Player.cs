using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action PlayerIsDeath = delegate { };
    public Action HealthChanged = delegate { };
    public Action AmmoIsChanged = delegate { };
    public Action TotalAmmoIsChanged = delegate { };

    [Header("Player SFX")]
    public AudioSource playerDeathSound;
    
    [Header("Pistol SFX")]
    public AudioSource pistolShot;
    public AudioSource pistolReloadStart;
    public AudioSource pistolReloadEnd;
    public AudioSource pistolEmptyMagazine;
    public AudioSource pistolCartridgeDrop;

    public GameObject magazinePrefub;
    public Bullet bulletPrefab;
    public GameObject shootPosition;
    
    [Header("Pistol Config")]
    public int playerHealth = 100;
    public int magazineCapacity = 7;
   // public int totalAmmoNumber = 49;
    public float fireRate = 1f;
    public bool death;
    [HideInInspector]
    public int capacity;
    
    float nextFire;
    

    Animator animator; 
    CircleCollider2D collider;
    Rigidbody2D rb;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        capacity = magazineCapacity;
        death = false;
        print("Capacity " + capacity);
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
        CheckNumberAmmo();
        
        if (/*totalAmmoNumber < 0 &&*/ magazineCapacity < 0)
        {
            return;
        }
        
        
        if (Input.GetButton("Fire1") && nextFire <= 0 && magazineCapacity > 0)
        {
            Shoot();
            magazineCapacity--;
            AmmoIsChanged();
            print("ammo " + magazineCapacity);
            
            
        }

        if (magazineCapacity <= 0 /*&& totalAmmoNumber > 0 */)
        {
            /*totalAmmoNumber -= capacity;
            print("total " + totalAmmoNumber);
            TotalAmmoIsChanged();*/
            
            print("Start coroutine");
            StartCoroutine(CheckPistolReload());
            
            Instantiate(magazinePrefub, transform.position, Quaternion.identity);
            Destroy(magazinePrefub,5f);
            
            magazineCapacity += capacity;
            AmmoIsChanged();

        }
       

        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        
    }
    

    void CheckNumberAmmo()
    {
        if (Input.GetButton("Fire1") /*&& totalAmmoNumber <= 0*/ && magazineCapacity <= 0)
        {
            pistolEmptyMagazine.Play();
        }
    }

    IEnumerator CheckPistolReload()
    {
        pistolReloadStart.Play();
        
        yield return new WaitForSeconds(1);
        
        pistolReloadEnd.Play();
        
        print("Stop coroutine");
        yield break;

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
       collider.enabled = false;
       rb.isKinematic = false;
       PlayerIsDeath();
   }
}
