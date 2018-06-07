using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour {
	
    public float speed;
    public int damage;
    Transform target;
    public float chaseRange;
    public float attackRange;
    private float lastAttackTime;
    public float attackDelay;
    public float health;
    private float currentHealth;
    public bool isTankSpawed;
    public Image healthBar;
    public Sprite[] frames;
    public SpriteRenderer rend;
    public AudioSource audioSrc;
    public int fps = 6;
    void Start() {
        target = GameObject.FindWithTag("Player").transform;
        currentHealth = health;
        rend = GetComponent<SpriteRenderer>();
        audioSrc = GetComponent<AudioSource>();
    }
    void Update() {
        if (frames.Length > 0) {
            int i = (int)Mathf.Clamp(((Time.time * fps) % frames.Length), 0, frames.Length - 1);
            rend.sprite = frames[i];
        }
        
        healthBar.fillAmount = currentHealth / health;
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
		if (target.position.x > transform.position.x && gameObject.GetComponent<SpriteRenderer> ().flipX) { //face right
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
        }
		else if (target.position.x < transform.position.x && !gameObject.GetComponent<SpriteRenderer> ().flipX) { //face left
			gameObject.GetComponent<SpriteRenderer> ().flipX = true;
        }
    }

    public void damaged(float amount) {
        audioSrc.Play();
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth/100;
        if ( currentHealth <= 0) {
            MapStateController._instance.CheckEnemiesAlive();
            Destroy(gameObject);
        }
    }
}
