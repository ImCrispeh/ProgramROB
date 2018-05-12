using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomController : MonoBehaviour {
    public static DarkRoomController _instance;
    public GameObject darkness;
    public GameObject maskObj;
    public Transform maskParent;
    public GameObject playerMask;
	public GameObject PowerGenMask;
    public GameObject[] enemyMasks;
    public GameObject[] enemies;
    public Transform player;
    public bool isPlayerMoving = false;
    public bool isEnemiesMoving = false;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerMask.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyMasks = new GameObject[enemies.Length];
        for (int i = 0; i < enemyMasks.Length; i++) {
            enemyMasks[i] = Instantiate(maskObj, enemies[i].transform.position, Quaternion.identity, maskParent);
        }
        ShowEnemies(false);
		PowerGenMask.SetActive (false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isPlayerMoving) {
            playerMask.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
        }

		if (isEnemiesMoving) {
            for (int i = 0; i < enemyMasks.Length; i++) {
                if (enemyMasks[i] != null) {
                    if (enemies[i] == null) {
                        Destroy(enemyMasks[i]);
                    } else {
                        enemyMasks[i].transform.position = Camera.main.WorldToScreenPoint(enemies[i].transform.position);
                    }
                }
            }
        }
	}

    public void ShowEnemies(bool toggle) {
        for (int i = 0; i < enemyMasks.Length; i++) {
            if (enemyMasks[i] != null) {
                if (enemies[i] == null) {
                    Destroy(enemyMasks[i]);
                } else {
                    enemyMasks[i].SetActive(toggle);
                }
            }
        }
    }

    public void ToggleEffect(bool toggle) {
        darkness.SetActive(toggle);
    }

    public void SetPlayerMoving(bool moving) {
        isPlayerMoving = moving;
        TurnController._instance.EnableSpeedChange(!moving);
    }

    public void SetEnemiesMoving(bool moving) {
        isEnemiesMoving = moving;
        TurnController._instance.EnableSpeedChange(moving);
    }

	public void SetRoomsLight(bool choice){
		PowerGenMask.SetActive (choice);
	}
}
