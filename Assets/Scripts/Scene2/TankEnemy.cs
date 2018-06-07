using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float health;
    private float currentHealth;
    public int numberOfEnemies;
    int count = 0;
    bool isEnd = false;
    public Image healthBar;
    public AudioSource audioSrc;
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        currentHealth = health;
        audioSrc = GetComponent<AudioSource>();
    }
    void Update()
    {
        healthBar.fillAmount = currentHealth / health;
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
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (target.position.x < transform.position.x)
        {
            //face left
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void damaged(float amount)
    {
        audioSrc.Play();
        health -= amount;
        healthBar.fillAmount = currentHealth / 100;
        if (health <= 0)
        {
            MapStateController._instance.CheckEnemiesAlive();
            Destroy(gameObject);
        }
    }
}
