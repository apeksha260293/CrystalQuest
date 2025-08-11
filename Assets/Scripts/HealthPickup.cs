using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameObject pickupEffect;
    public GameObject audioSource;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Instantiate(audioSource, transform.position, Quaternion.identity);
            Destroy(gameObject);
            collision.GetComponent<Player>().Heal();            
        }
    }
}
