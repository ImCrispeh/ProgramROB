using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public Collider2D meleeCollider;
    public float speed;
    bool attacking;
    private float attackTimer = 0f;
    private float attackCd = 0.5f;
    void Start()
    {
        meleeCollider.enabled = false;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        
        if (Input.GetMouseButtonDown(0) && !attacking){
            attacking = true;
            attackTimer = attackCd;
            meleeCollider.enabled = true;
        }
        if(attacking)
        {
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                meleeCollider.enabled = false;
            }
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
