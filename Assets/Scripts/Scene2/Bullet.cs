using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    public bool isTurretSpawned;
    private void Update()
    {
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerCombatController>().damaged(damage);
            if (isTurretSpawned) {
                DataCollectionController._instance.UpdateTurretDamage(damage);
            } else {
                DataCollectionController._instance.UpdateRangedDamage(damage);
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Boundary" || collision.gameObject.tag == "Wall") {
            Destroy(gameObject);
        }
    }
}
