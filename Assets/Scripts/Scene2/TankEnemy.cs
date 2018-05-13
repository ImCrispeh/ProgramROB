using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : MonoBehaviour
{
    public float speed;
    public int damage;
    Transform target;
    public Transform SpawnPoint;
    public GameObject Enemies;
    public float chaseRange;
    public float attackRange;
    private float lastAttackTime;
    public float attackDelay;
    public int health, numberOfEnemies;
    int count = 0;
    bool isEnd = false;
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            Debug.Log(distanceToTarget);
            if (distanceToTarget < chaseRange)
            {
                if (distanceToTarget > attackRange)
                {
                    if (isEnd == false)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                        checkPosition();
                    }
                }
                if (distanceToTarget <= attackRange)
                {
                    if(count < numberOfEnemies)
                    {
                        if (Time.time > lastAttackTime + attackDelay)
                        {
                            Instantiate(Enemies, SpawnPoint.position, SpawnPoint.rotation);
                            lastAttackTime = Time.time;

                            count++;
                        }

                    }
                    else
                    {
                        isEnd = true;
                    }
                }
            }
            
        }
    }

    private void checkPosition()
    {
        if (target.position.x > transform.position.x)
        {
            //face right
            transform.localScale = new Vector3(-2.5f, 2.5f, 1);
        }
        else if (target.position.x < transform.position.x)
        {
            //face left
            transform.localScale = new Vector3(2.5f, 2.5f, 1);
        }
    }

    public void damaged(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            //MapStateController._instance.CheckEnemiesAlive();
            Destroy(gameObject);
        }
    }
}
