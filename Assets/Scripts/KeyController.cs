using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {
    public GameObject door;
    public int keyID;
    public bool isCollected;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Destroy(door);
            Destroy(this.gameObject);
        }
    }
}
