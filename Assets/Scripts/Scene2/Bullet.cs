using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    private void Update()
    {
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Destroy(gameObject);
            collision.gameObject.GetComponent<PlayerCombatController>().damaged(damage);
        }

        if (collision.gameObject.tag == "Boundary" || collision.gameObject.tag == "Wall") {
            Destroy(gameObject);
        }
    }
}
