using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int health;
    public int amount;
    Transform target;
    public float Speed;
    bool isDetect;
    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (isDetect == true)
        {
            Vector3 targetDir = transform.position - target.position;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180);

            transform.Translate(Vector3.up * Time.deltaTime * Speed);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isDetect = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDetect = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Player")
        {
            target.gameObject.GetComponent<Player>().damaged(amount);
        }
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
