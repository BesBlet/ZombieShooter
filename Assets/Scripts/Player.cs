using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Bullet bulletPrefab;
    public GameObject shootPosition;

    public float fireRate = 1f;

    float nextFire;
   
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);

            nextFire = fireRate;
        }
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        
    }
}
