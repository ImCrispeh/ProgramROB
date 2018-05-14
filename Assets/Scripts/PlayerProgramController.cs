﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgramController : MonoBehaviour {
    public bool hitWall;
    public float visibilityMultiplier = 1f;
    public int maxHealth = 15;
    public int currHealth;
    public int damage = 3;
    public Vector2 lastPos;
    public float moveDist = 1.5f;
    public float moveDuration = 1f;
    public float moveWait;
    public float moveTimer = 0f;
    private bool isMoving = false;
    public int currNumOfActions = 0;
    public int maxNumOfActions = 5;
    public int actionPoints;
    public int maxActionPoints = 30;
    public List<String> actions;
    public Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
        actions = new List<String>();
        StatsController._instance.UpdateActionPoints(actionPoints, currNumOfActions);
        StatsController._instance.UpdateHealth(currHealth);
        StatsController._instance.UpdateActionsList(actions, false);
        rigid = GetComponent<Rigidbody2D>();
        lastPos = transform.position;
        moveWait = moveDuration + 0.2f;
	}

	// Update is called once per frame
	void Update () {
        // Key inputs here for hotkeys
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            AddAction("MoveRight");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            AddAction("MoveLeft");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            AddAction("MoveUp");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            AddAction("MoveDown");
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            LoadActionList();
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            RemoveAction();
        }
    }

    //Collision & Triggers
    /*void OnTriggerStay2D (Collider2D other) {
		Debug.Log ("Working");
			if (Input.GetButtonDown ("Fire1") && other.transform.tag == "Enemy") {
				other.gameObject.GetComponent<TestEnemyController> ().Damage (damage);
			}
	}*/

    public void RemoveAction() {
        if (TurnController._instance.GetIsPlayerTurn()) {
            if (currNumOfActions > 0) {
                currNumOfActions--;
                actions.RemoveAt(currNumOfActions);
                StatsController._instance.UpdateActionsList(actions, false);
                StatsController._instance.UpdateActionPoints(actionPoints, currNumOfActions);
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
            if (actionPoints > 0) {
                if (currNumOfActions < maxNumOfActions) {
                    actions.Add(action);
                    currNumOfActions++;
                    StatsController._instance.UpdateActionsList(actions, false);
                    StatsController._instance.UpdateActionPoints(actionPoints, currNumOfActions);
                } else {
                    StatsController._instance.DisplayMax();
                }
            }
        }
    }

    // Coroutines used so that they can be queued
    // Need to use WaitForSeconds so that actions do not get locked out by the isMoving condition
    IEnumerator MoveRight() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x + moveDist, transform.position.y)));
        }
        yield return new WaitForSeconds(moveWait);
    }

    IEnumerator MoveLeft() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x - moveDist, transform.position.y)));
        }
        yield return new WaitForSeconds(moveWait);
    }

    IEnumerator MoveUp() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x, transform.position.y + moveDist)));
        }
        yield return new WaitForSeconds(moveWait);
    }

    IEnumerator MoveDown() {
        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x, transform.position.y - moveDist)));
        }
        yield return new WaitForSeconds(moveWait);
    }

    IEnumerator ExecuteActionList() {
        actions.Reverse();
        for (int i = actions.Count-1; i >= 0; i--) {
            currNumOfActions--;
            actionPoints--;
            StatsController._instance.UpdateActionPoints(actionPoints, currNumOfActions);
            yield return StartCoroutine(actions[i]);
            actions.RemoveAt(i);
            StatsController._instance.UpdateActionsList(actions, true);
        }
        //actions.Clear();
        //StatsController._instance.UpdateActionsList(actions);
        TurnController._instance.EnemyTurn();
    }

    IEnumerator Move(Vector2 source, Vector2 target) {
        lastPos = transform.position;
        isMoving = true;
        DarkRoomController._instance.SetPlayerMoving(isMoving);
        while (moveTimer < moveDuration) {
            moveTimer += Time.deltaTime;
            rigid.MovePosition(Vector2.Lerp(source, target, moveTimer / moveDuration));
            yield return null;
        }
        if (hitWall) {
            transform.position = lastPos;
            hitWall = false;
        }
        isMoving = false;
        DarkRoomController._instance.SetPlayerMoving(isMoving);
        moveTimer = 0f;
        lastPos = transform.position;
    }

    public void ChangeSpeed(bool isFast) {
        if (isFast) {
            moveDuration = 0.5f;
            moveWait = moveDuration + 0.05f;
        } else {
            moveDuration = 1f;
            moveWait = moveDuration + 0.2f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall") {
            hitWall = true;
        }

        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyMap>().isAlive = false;
			MapStateController._instance.LoadCombatScene(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Exit") {
            MapStateController._instance.SaveEndOfLevelData();
            MapStateController._instance.EndGame(true, "");
        }
    }
}
    
