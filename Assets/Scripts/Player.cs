using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    private Animator animator;

    public float health = 100.0f;

    private Rigidbody2D rb;

    private Vector2 moveAmount;

    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int relationBetweenHeartAndHealth = 20;

    private Animator cameraAnimator;
    public Animator hurtPanel;

    private SceneTransitions sceneTransitions;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cameraAnimator = Camera.main.GetComponent<Animator>();
        sceneTransitions = FindObjectOfType<SceneTransitions>();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }    

    public void TakeDamage(int damage)
    {
        health -= damage;
        float healthToHeart = health / relationBetweenHeartAndHealth;
        UpdateHeartUI(healthToHeart);
        cameraAnimator.SetTrigger("shake");
        hurtPanel.SetTrigger("hurt");

        if (health <= 0)
        {
            Destroy(gameObject);
            sceneTransitions.LoadScene("Lose");
        }
    }

    private void UpdateHeartUI(float currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void ChangeWeapon(GameObject weapon)
    {
        GameObject[] Weapons = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < Weapons.Length; i++)
        {
            Destroy(Weapons[i]);
        }
        Instantiate(weapon, transform.position, transform.rotation, transform);
    }

    public void Heal(){
        health += relationBetweenHeartAndHealth;

        if(health > 100)
            health = 100;

        float healthToHeart = health / relationBetweenHeartAndHealth;
        UpdateHeartUI(healthToHeart);
    }
}
