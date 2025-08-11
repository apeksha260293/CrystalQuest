using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{    
    public float stopDistance;
    private float attackTime;

    private Animator animator;

    public Transform shotPoint;

    public GameObject rangedEnemyProjectile;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if(player != null) {
            if(Vector2.Distance(transform.position, player.position) > stopDistance){
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            if(Time.time >= attackTime) {
                attackTime = Time.time + timeBetweenAttacks;
                animator.SetTrigger("attack");
            }
        }
    }

    // will be instantiated by the function inside the Animation tab
    public void rangedAttack(){
        Vector2 attackDirection = player.position - shotPoint.position;
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        shotPoint.rotation = rotation;

        Instantiate(rangedEnemyProjectile, shotPoint.position, shotPoint.rotation);

    }
}
