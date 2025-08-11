using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;
    public float lifetime;
    public int damage;

    public GameObject soundEffect;

    public GameObject explosion;

    void Awake()
    {
        Instantiate(soundEffect, transform.position, transform.rotation);
    }
    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * speed);
    }

    void DestroyProjectile(){
        Instantiate(explosion, transform.position, Quaternion.identity); // instantiates the explosion particle
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Enemy") {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            DestroyProjectile();
        }

        if (collision.tag == "Boss")
        {
            collision.GetComponent<Boss>().TakeDamage(damage);
            DestroyProjectile();
        }
    }
}
