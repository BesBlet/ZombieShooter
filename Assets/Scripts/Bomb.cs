using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bomb : MonoBehaviour
{
    public LayerMask damageLayers;
    public float radius = 5f;
    public int damage = 50;
    public GameObject explosionEffectPrefab;

    private void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO check tag Bullet
        Bullet bullet = collision.GetComponent<Bullet>();
        
        print("damage: " + bullet.playerDamage);

        Explode();
    }

    private void Explode()
    {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageLayers);

        foreach(Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
            }
            collider.gameObject.SendMessage("UpdateHealth", -damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
