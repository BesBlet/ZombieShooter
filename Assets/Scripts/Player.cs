using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  Lean.Pool;

public class Player : MonoBehaviour
{
    public static Player Instance;

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
    public int totalAmmoNumber = 49;
    public float fireRate = 1f;
    public bool death;
    [HideInInspector]
    public int capacity;
    
    float nextFire;
    bool isReloading;
    

    Animator animator; 
    CircleCollider2D collider;
    Rigidbody2D rb;
    

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        capacity = magazineCapacity;
        death = false;
        print("Capacity " + capacity);
        isReloading = false;
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
        
        IfPistolReload();
        
        if (isReloading)
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

        
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        
    }
    

    void CheckNumberAmmo()
    {
        if (Input.GetButton("Fire1") && totalAmmoNumber <= 0 && magazineCapacity <= 0)
        {
            pistolEmptyMagazine.Play();
        }
    }

    void IfPistolReload()
    {
        if (magazineCapacity > 0)
        {
            isReloading = false;
        }
        else
        {
            isReloading = true;
        }
        
        
        if (magazineCapacity <= 0 && totalAmmoNumber > 0 && isReloading)
        {
            totalAmmoNumber -= capacity;
            print("total " + totalAmmoNumber);
            TotalAmmoIsChanged();
            
            print("Start coroutine");
            StartCoroutine(CheckPistolReload());
            
            LeanPool.Spawn(magazinePrefub, transform.position, Quaternion.identity);
            LeanPool.Despawn(magazinePrefub.gameObject,5f);
            
            magazineCapacity += capacity;
            AmmoIsChanged();

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
       
        LeanPool.Spawn(bulletPrefab, shootPosition.transform.position, transform.rotation);
        
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


    private void OnDestroy()
    {
        if(this == Instance)
        {
            Instance = null;
        }
    }
}
