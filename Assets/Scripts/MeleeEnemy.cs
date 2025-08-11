using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float stopDistance;
    private float timeAttack;

    public float attackSpeed;

    private void Update() {
        if(player != null) {
            if(Vector2.Distance(transform.position, player.position) > stopDistance){
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            } else {
                if(Time.time > timeAttack) {
                    StartCoroutine(Attack());
                    timeAttack = Time.time + timeBetweenAttacks;
                }
            }
        }
    }

    IEnumerator Attack() {
        player.GetComponent<Player>().TakeDamage(damage);

        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = player.position;

        float percent = 0;
        while(percent <= 1) {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4; // 2ª degree equation
            transform.position = Vector2.Lerp(originalPosition, targetPosition, formula); // Lerp(origin, target, t) => t is clamped to the range of [0,1]
            yield return null;
        }
    }
}
