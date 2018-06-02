using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FireEnemy : MonoBehaviour
{
    public float health;
    private float currentHealth;
    protected Transform target;
    public float Speed;
    bool isDetect;
    public float attackRange;
    public float chaseRange;
    protected float lastAttackTime = 0;
    public float attackDelay;
    public GameObject projectTile;
    public float bulletForce;
    public Transform shootPoint;
    public Image healthBar;
    protected void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        currentHealth = health;
    }
    void Update()
    {
        healthBar.fillAmount = currentHealth / health;
        if (target != null) {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < chaseRange)
            {
                if (distanceToTarget > attackRange)
                {
                    Rotate();
                    transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
                }

                if (distanceToTarget < attackRange)
                {
                    Rotate();
                    if (Time.time > lastAttackTime + attackDelay)
                    {
                        GameObject Bullet = Instantiate(projectTile, shootPoint.position, transform.rotation);
                        Bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, -bulletForce));
                        lastAttackTime = Time.time;
                    }
                }
            }
        }
    }
    private void Rotate()
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 90 * Time.deltaTime);
    }


    public void damaged(float amount)
    {
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / 100;
        if (currentHealth <= 0)
        {
            MapStateController._instance.CheckEnemiesAlive();
            Destroy(gameObject);
        }
    }
}
