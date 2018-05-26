using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour {
    public static TurnController _instance;
    private bool isPlayerTurn;
    public Text turnText;
    public GameObject player;
    public GameObject[] enemies;
    public Toggle speedChangeTgl;
	public int repelTurns;
	public bool isPlayerRepelling;
    //public Canvas canvas;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        PlayerTurn();
	}
	
	// Update is called once per frame
	void Update () {
        if (!player.GetComponent<PlayerProgramController>().isPerformingActions) {
            if (Input.GetKeyDown(KeyCode.F)) {
                speedChangeTgl.isOn = !speedChangeTgl.isOn;
                ChangeSpeed(speedChangeTgl.isOn);
            }
        }
	}

    public void ChangeSpeed(bool isFast) {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (enemies.Length == 0) {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }
        player.GetComponent<PlayerProgramController>().ChangeSpeed(isFast);

        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                enemy.GetComponent<EnemyMap>().ChangeSpeed(isFast);
            }
        }
    }

    public void EnableSpeedChange(bool isEnabled) {
        speedChangeTgl.interactable = isEnabled;
    }

    public bool GetIsPlayerTurn() {
        return isPlayerTurn;
    }

    public void EnemyTurn() {
		isPlayerRepelling = (repelTurns > 0);
		isPlayerTurn = false;
		turnText.text = "Enemy Turn";
        StartCoroutine(EnemyMovement());
        if (isPlayerRepelling) {
            repelTurns--;
        }
    }

    public void PlayerTurn() {
        if (player.GetComponent<PlayerProgramController>().actionPoints == 0) {
            MapStateController._instance.EndGame(false, "No action points remaining");
        } else {
            if (!speedChangeTgl.interactable) {
                speedChangeTgl.interactable = true;
            }
            isPlayerTurn = true;
            turnText.text = "Player Turn";
        }
    }

    IEnumerator EnemyMovement() {
        DarkRoomController._instance.ShowEnemies(true);
        //DarkRoomController._instance.SetEnemiesMoving(true);
        //canvas.gameObject.SetActive(false);
        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                if (enemy.GetComponent<Renderer>().isVisible) {
                    int i = 0;
                    while (i < 3 && !enemy.GetComponent<EnemyMap>().isMoving) {
                        yield return StartCoroutine(enemy.GetComponent<EnemyMap>().MoveEnemy());
                        i++;
                    }
                    enemy.GetComponent<EnemyMap>().ResetAfterTurn();
                }
            }
        }
        yield return new WaitForSeconds(1f);
        DarkRoomController._instance.ShowEnemies(false);
        //DarkRoomController._instance.SetEnemiesMoving(false);
        //canvas.gameObject.SetActive(true);
        EnableSpeedChange(true);
        PlayerTurn();
    }
}
