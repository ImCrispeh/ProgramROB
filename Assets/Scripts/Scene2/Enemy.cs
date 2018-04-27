using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed;
    public int damage;
    Transform target;
    public float chaseRange;
    public float attackRange;
    private float lastAttackTime;
    public float attackDelay;
    public int health;
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
       
    }
    void Update()
    {
        if (target != null) {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            //if(distanceToTarget < chaseRange)
            //{
            if (distanceToTarget > attackRange) {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            //}
            if (distanceToTarget < attackRange) {
                if (Time.time > lastAttackTime + attackDelay) {
                    target.gameObject.GetComponent<PlayerCombatController>().damaged(damage);
                    lastAttackTime = Time.time;
                }
            }
            checkPosition();
        }
    }

    private void checkPosition()
    {
        if (target.position.x > transform.position.x)
        {
            //face right
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
        }
        else if (target.position.x < transform.position.x)
        {
            //face left
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
    }

    public void damaged(int amount)
    {
        health -= amount;
        if (health <= 0) {
            MapStateController._instance.CheckEnemiesAlive();
            Destroy(gameObject);
        }
    }
}
