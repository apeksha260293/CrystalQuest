using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float health;
    public int damage;

    private Player playerClass;
    private GameObject player;

    private int halfHealth;

    private Animator anim;
    public Enemy[] enemies;
    private Vector3 summonOffset = new Vector3(2, 2, 0);

    public GameObject bossSplash;
    public GameObject bossDeathEffect;

    public GameObject bossDyingAudioEffect;
    public GameObject bossEnteringAudioEffect;

    private Slider healthBar;

    private SceneTransitions sceneTransitions;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerClass = FindObjectOfType<Player>();
        halfHealth = (int) health/2;
        anim = GetComponent<Animator>();
        healthBar = FindObjectOfType<Slider>();
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        sceneTransitions = FindObjectOfType<SceneTransitions>();
    }    

    private void FindAndDestroyAllEnemiesInScene() {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);           
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health;
        float healthToHeart = health / playerClass.relationBetweenHeartAndHealth;

        if (health <= 0)
        {
            Instantiate(bossDyingAudioEffect, transform.position, Quaternion.identity);
            Instantiate(bossSplash, transform.position, Quaternion.identity);
            Instantiate(bossDeathEffect, transform.position, Quaternion.identity);
            FindAndDestroyAllEnemiesInScene();
            Destroy(this.gameObject);
            healthBar.gameObject.SetActive(false);
            sceneTransitions.LoadScene("Win");
        } else {
            if (health <= halfHealth)
            {
                anim.SetTrigger("stage2");
            }

            Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position + summonOffset, transform.rotation);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            player.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void ShakeCamera(){
        Instantiate(bossEnteringAudioEffect, transform.position, Quaternion.identity);
        Invoke("ShakeCameraDelay", 0.5f);
    }

    private void ShakeCameraDelay(){
        Camera.main.GetComponent<Animator>().SetTrigger("bossShake");
    }
}
