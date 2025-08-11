using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Player playerScript;
    public float speed;
    public int damage;

    public float lifetime;

    public GameObject projectileEffect;

    private void Start(){
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();;
        Invoke("DestroyProjectile", lifetime);
    }
    private void Update(){ 
        transform.Translate(Vector2.up * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            playerScript.TakeDamage(damage);
            Destroy(gameObject);
        } 
    }

    void DestroyProjectile()
    {
        Instantiate(projectileEffect, transform.position, Quaternion.identity); // instantiates the explosion particle
        Destroy(gameObject);
    }
}
