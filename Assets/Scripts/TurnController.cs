using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour {
    public static TurnController _instance;
    private bool isPlayerTurn;
    public Text turnText;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        PlayerTurn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool GetIsPlayerTurn() {
        return isPlayerTurn;
    }

    public void EnemyTurn() {
        isPlayerTurn = false;
        turnText.text = "Enemy Turn";
        StartCoroutine(EnemyTurnTimeTest());
    }

    public void PlayerTurn() {
        isPlayerTurn = true;
        turnText.text = "Player Turn";
    }

    IEnumerator EnemyTurnTimeTest() {
        DarkRoomController._instance.ToggleEffect(false);
        yield return new WaitForSeconds(3.5f);
        DarkRoomController._instance.ToggleEffect(true);
        PlayerTurn();
    }
}
