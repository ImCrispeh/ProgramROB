using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : MonoBehaviour
{

    public int health;
    public int amount;
    Transform target;
    public float Speed;
    bool isDetect;
    public float attackRange;
    public float chaseRange;
    private float lastAttackTime = 0;
    public float attackDelay;
    public GameObject projectTile;
    public float bulletForce;
    //public Transform shootPoint;
    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
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
                GameObject Bullet = Instantiate(projectTile, transform.position, transform.rotation);
                Bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, -bulletForce));
                lastAttackTime = Time.time;
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


    public void damaged(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log(health);
    }
}
