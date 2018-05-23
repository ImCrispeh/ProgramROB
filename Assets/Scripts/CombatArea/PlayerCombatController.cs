using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatController : MonoBehaviour {
    public int health;
    public int range;
    public Rigidbody2D rigid;
    public float speed;
    private Transform firePoint;
    public int damage = 3;
    public float fireRate;
    public float timeToFire = 0;
    public Text healthText;
    public LayerMask allowHit;
    public Material lineMat;
    public GameObject particle;
    // Use this for initialization
    void Start () {
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        healthText.text = "Health: " + health;
        firePoint = transform.GetChild(0).GetChild(0);
        if (firePoint == null) {
            Debug.Log("Fire Point not Found");
        }
        rigid = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (fireRate == 0) {
            if (Input.GetButtonDown("Fire2")) {
                Shooting();
            }
        } else {
            if (Input.GetButton("Fire2") && Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;
                Shooting();
            }
        }
    }

    private void FixedUpdate() {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rigid.velocity = new Vector2(moveX * speed, moveY * speed);
    }

    private void Shooting() {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPos, mousePos - firePointPos, range, allowHit);
        //Debug.DrawLine (firePointPos, mousePos, Color.red);
        if (hit.collider != null) {
            DrawFiringLine(hit.transform.position, firePointPos, true);
            GameObject hitObj = hit.collider.gameObject;
            //Debug.DrawLine(firePointPos, hit.point, Color.red);
            if (hitObj.GetComponent<Enemy>() != null) {
                hitObj.GetComponent<Enemy>().damaged(damage);
                Instantiate(particle, hitObj.transform.position, Quaternion.identity);
            } else if (hitObj.GetComponent<FireEnemy>() != null) {
                hitObj.GetComponent<FireEnemy>().damaged(damage);
                Instantiate(particle, hitObj.transform.position, Quaternion.identity);
            } else if (hitObj.GetComponent<Turrets>() != null) {
                hitObj.GetComponent<Turrets>().damaged(damage);
                Instantiate(particle, hitObj.transform.position, Quaternion.identity);
            } else if (hitObj.GetComponent<TankEnemy>() != null) {
                hitObj.GetComponent<TankEnemy>().damaged(damage);
                Instantiate(particle, hitObj.transform.position, Quaternion.identity);
            }
        } else {
            DrawFiringLine(mousePos, firePointPos, false);
        }
    }

    private void DrawFiringLine(Vector2 end, Vector2 firePoint, bool hitEnemy) {
        gameObject.AddComponent<LineRenderer>();
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        lr.material = lineMat;
        lr.widthMultiplier = 0.05f;
        Vector2 dir = (end - firePoint).normalized;
        lr.SetPosition(0, firePoint);
        if (hitEnemy) {
            lr.SetPosition(1, end);
        } else {
            lr.SetPosition(1, firePoint + dir * range);
        }
        Destroy(lr, 0.05f);
    }

    public void damaged(int amount) {
        Instantiate(particle, gameObject.transform.position, Quaternion.identity);
        health -= amount;
        healthText.text = "Health: " + health;
        if (health <= 0) {
            MapStateController._instance.EndGame(false, "You were destroyed");
            
        }
    }
}
