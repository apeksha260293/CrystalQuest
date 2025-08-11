using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    public float timeBetweenAttacks;
    public int damage;

    public int healthPickupChance;
    public GameObject healthPickup;
    public int pickupChance;
    public GameObject[] pickups;

    public GameObject enemyDeathEffect;

    
    [HideInInspector]
    public Transform player; // We'll want the enemies to follow the player

    public virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damage) {
        health -= damage;

        if(health <= 0) {
            int randomNumber = Random.Range(0, 101);
            if(randomNumber < pickupChance){
                GameObject pickup = pickups[Random.Range(0, pickups.Length)];
                Instantiate(pickup, transform.position, transform.rotation);
            }

            if(randomNumber < healthPickupChance){
                Instantiate(healthPickup, transform.position, transform.rotation);
            }
            
            Instantiate(enemyDeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
