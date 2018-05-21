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
    public bool isTankSpawed;

    void Start() {
        target = GameObject.FindWithTag("Player").transform;
       
    }
    void Update() {
        if (target != null) {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
			if (distanceToTarget < chaseRange) {
				checkPosition();
				if (distanceToTarget > attackRange) {
					transform.position = Vector2.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
				}
			}
            if (distanceToTarget < attackRange) {
                if (Time.time > lastAttackTime + attackDelay) {
                    target.gameObject.GetComponent<PlayerCombatController>().damaged(damage);
                    lastAttackTime = Time.time;
                    if (isTankSpawed) {
                        DataCollectionController._instance.UpdateTankSpawnDamage(damage);
                    } else {
                        DataCollectionController._instance.UpdateMeleeDamage(damage);
                    }
                }
            }
            //checkPosition();
        }
    }

    private void checkPosition() {
		if (target.position.x > transform.position.x && !gameObject.GetComponent<SpriteRenderer> ().flipX) { //face right
			gameObject.GetComponent<SpriteRenderer> ().flipX = true;
        }
		else if (target.position.x < transform.position.x && gameObject.GetComponent<SpriteRenderer> ().flipX) { //face left
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
        }
    }

    public void damaged(int amount) {
        health -= amount;
        if (health <= 0) {
            MapStateController._instance.CheckEnemiesAlive();
            Destroy(gameObject);
        }
    }
}
