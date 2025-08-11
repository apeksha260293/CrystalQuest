using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class summoner : Enemy
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    
    private Vector2 targetPosition;
    private Animator anim;

    private float summonTime;
    public float timeBetweenSummons;
    public Enemy enemyToInstantiate;

    public float stopDistance;
    private float timeAttack;
    public float attackSpeed;

    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();

        float xPos = Random.Range(minX, maxX);
        float yPos = Random.Range(minY, maxY);

        targetPosition = new Vector2(xPos, yPos);
    }


    void Update()
    {
        if(player != null) {
            if(Vector2.Distance(transform.position, targetPosition) > 0.5f){
                // if summoner is not moving, try to enable the "Apply Root Motion" inside Animator in Inspector
                this.transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                anim.SetBool("isRunning", true);
            } else {
                anim.SetBool("isRunning", false);
                if(Time.time >= summonTime){
                    summonTime = Time.time + timeBetweenSummons;
                    anim.SetTrigger("summon");
                }
            }

            if (Vector2.Distance(transform.position, player.position) < stopDistance)
            {               
                if (Time.time > timeAttack)
                {
                    StartCoroutine(Attack());
                    timeAttack = Time.time + timeBetweenAttacks;
                }
            }
        }
    }

    IEnumerator Attack()
    {
        player.GetComponent<Player>().TakeDamage(damage);

        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = player.position;

        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4; // 2ª degree equation
            transform.position = Vector2.Lerp(originalPosition, targetPosition, formula); // Lerp(origin, target, t) => t is clamped to the range of [0,1]
            yield return null;
        }
    }

    // This function is called every time the trigger "summon" is activated. 
    // You can set it inside the Animation panel, inside the animation desired (in this case
    // it is "summoning"), changing the timestamp you want to be triggered and then add the event
    // using "Add event..." and selecting the fuction below.
    public void Summon(){
        Instantiate(enemyToInstantiate, transform.position, transform.rotation);
    }
}
