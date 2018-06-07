using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Turrets : MonoBehaviour
{
    public Image healthBar;
    public float health;
    private float currentHealth;
    Transform target;
    public float attackRange;
    private float lastAttackTime = 0;
    public float attackDelay;
    public float bulletForce;
    public Transform shootPoint;
    public GameObject projectTile;
    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        currentHealth = health;
    }
    void Update()
    {
        healthBar.fillAmount = currentHealth / health;
        if (target != null) {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < attackRange) {
                checkPosition();
                Vector3 targetDir = target.position - transform.position;
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 180f;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 90 * Time.deltaTime);
                if (Time.time > lastAttackTime + attackDelay) {
                    GameObject Bullet = Instantiate(projectTile, shootPoint.transform.position, Quaternion.identity);
                    Bullet.GetComponent<Bullet>().isTurretSpawned = true;
                    Bullet.GetComponent<Rigidbody2D>().AddRelativeForce(-transform.right * bulletForce);
                    lastAttackTime = Time.time;

                    //   }
                }
            }
        }
    }

    private void checkPosition() {
        if (target.position.x > transform.position.x && !gameObject.GetComponent<SpriteRenderer>().flipY) { //face right
            gameObject.GetComponent<SpriteRenderer>().flipY = true;

        } else if (target.position.x < transform.position.x && gameObject.GetComponent<SpriteRenderer>().flipY) { //face left
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }
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
