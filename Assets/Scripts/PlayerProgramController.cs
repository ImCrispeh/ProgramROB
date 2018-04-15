using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgramController : MonoBehaviour {
    public float moveDist = 1.5f;
    public float moveDuration = 1f;
    public float moveTimer = 0f;
    private bool isMoving = false;
    public int currNumOfActions = 0;
    public int maxNumOfActions = 5;
    public List<String> actions;
    public Text actionsText;
    public Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
        actions = new List<String>();
        rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        
        // Key inputs here for testing before implementing on buttons
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            AddAction("MoveRight");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            AddAction("MoveLeft");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            AddAction("MoveUp");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            AddAction("MoveDown");
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            LoadActionList();
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            RemoveAction();
        }
	}

    public void RemoveAction() {
        if (TurnController._instance.GetIsPlayerTurn()) {
            if (currNumOfActions > 0) {
                currNumOfActions--;
                actions.RemoveAt(currNumOfActions);
                UpdateActionText();
            }
        }
    }

    public void LoadActionList() {
        if (TurnController._instance.GetIsPlayerTurn()) {
            StartCoroutine(ExecuteActionList());
        }
    }

    // For buttons to use
    public void AddAction(String action) {
        if (TurnController._instance.GetIsPlayerTurn()) {
            if (currNumOfActions < maxNumOfActions) {
                actions.Add(action);
                currNumOfActions++;
                UpdateActionText();
            }
        }
    }

    public void UpdateActionText() {
        actionsText.text = "";
        foreach (String action in actions) {
            actionsText.text += action + "\n";
        }
    }

    // Coroutines used so that they can be queued
    // Need to use WaitForSeconds so that actions do not get locked out by the isMoving condition
    IEnumerator MoveRight() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x + moveDist, transform.position.y)));
        }
        yield return new WaitForSeconds(1.2f);
    }

    IEnumerator MoveLeft() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x - moveDist, transform.position.y)));
        }
        yield return new WaitForSeconds(1.2f);
    }

    IEnumerator MoveUp() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x, transform.position.y + moveDist)));
        }
        yield return new WaitForSeconds(1.2f);
    }

    IEnumerator MoveDown() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x, transform.position.y - moveDist)));
        }
        yield return new WaitForSeconds(1.2f);
    }

    IEnumerator ExecuteActionList() {
        foreach (String actionFunc in actions) {
            Debug.Log(actionFunc);
            yield return StartCoroutine(actionFunc);
        }
        actions.Clear();
        currNumOfActions = 0;
        UpdateActionText();
        TurnController._instance.EnemyTurn();
    }

    IEnumerator Move(Vector2 source, Vector2 target) {
        isMoving = true;
        while (moveTimer < moveDuration) {
            moveTimer += Time.deltaTime;
            rigid.MovePosition(Vector2.Lerp(source, target, moveTimer / moveDuration));
            yield return null;
        }
        //transform.position = target;
        isMoving = false;
        moveTimer = 0f;
    }
}
