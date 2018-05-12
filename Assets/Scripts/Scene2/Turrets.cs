using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{

    public int health;
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
    }
    void Update()
    {
        if (target != null) {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < attackRange)
            {
            Vector3 targetDir = target.position - transform.position;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 180f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 90 * Time.deltaTime);
            if (Time.time > lastAttackTime + attackDelay) {
                GameObject Bullet = Instantiate(projectTile, shootPoint.transform.position, Quaternion.identity);
                Bullet.GetComponent<Rigidbody2D>().AddRelativeForce(-transform.right * bulletForce);
                lastAttackTime = Time.time;

              }
            }
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
