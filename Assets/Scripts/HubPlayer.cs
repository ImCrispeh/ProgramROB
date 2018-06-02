using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HubPlayer : MonoBehaviour {
    public float speed;
    public Rigidbody2D rigid;
    public Text interactText;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate() {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rigid.velocity = new Vector2(moveX * speed, moveY * speed);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "UpgradeEntry") {
            interactText.text = "Press E to select upgrades";
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene("UpgradeScreen");
            }
        }

        if (collision.gameObject.tag == "S1Entry") {
            interactText.text = "Press E to enter level";
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene(1);
            }
        }

        if (collision.gameObject.tag == "S2Entry") {
            interactText.text = "Press E to enter level";
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene(2);
            }
        }

        if (collision.gameObject.tag == "S3Entry") {
            interactText.text = "Press E to enter level";
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene(3);
            }
        }

        if (collision.gameObject.tag == "S4Entry") {
            interactText.text = "Press E to enter level";
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene(4);
            }
        }

        if (collision.gameObject.tag == "S5Entry") {
            MapStateController map = MapStateController._instance;
            if (!map.key1Collected || !map.key2Collected || !map.key3Collected || !map.key4Collected) {
                interactText.text = "Must collect all keys: " + MapStateController._instance.numKeys + "/4";
            } else {
                interactText.text = "Press E to end";
                if (Input.GetKeyDown(KeyCode.E)) {
                    GameReset._instance.EndGame(true, "");
                }
            }
        }

        if (collision.gameObject.tag == "UpgradeAreaEntry") {
            interactText.text = "Press E to enter upgrade area";
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene("UpgradeArea");
            }
        }

        if (collision.gameObject.tag == "Exit") {
            interactText.text = "Press E to exit";
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene("Hub");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        interactText.text = "";
    }
}
