using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject weaponToEquip;
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            collision.GetComponent<Player>().ChangeWeapon(weaponToEquip);
        }
    }
}
