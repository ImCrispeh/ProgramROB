using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerProgramController : MonoBehaviour {
    public bool hitWall;
    public float visibilityMultiplier = 1f;
    public int maxHealth = 15;
    public int currHealth;
    public int damage = 3;
    public Vector2 lastPos;
    public float moveDist = 1.5f;
    public float moveDuration;
    public float moveWait;
    public float moveTimer = 0f;
    private bool isMoving = false;
    public bool isPerformingActions;
    public int currNumOfActions = 0;
    public int maxNumOfActions = 5;
    public int estCost;
    public int actionPoints;
    public int maxActionPoints;
    public List<String> actions;
    public Rigidbody2D rigid;

    public bool isRepelAvailable;
    public bool isRevealAvailable;
    public bool isConvertAvailable;

    public bool isConversionActive;
    public bool isRevealUsed;

    public AudioSource audioSrc;

    // Use this for initialization
    void Start() {
        actions = new List<String>();
        if (isConversionActive) {
            estCost = 1;
        } else {
            estCost = 0;
        }
        OverlayController._instance.UpdateActionPoints(actionPoints, estCost);
        OverlayController._instance.UpdateHealth(currHealth);
        OverlayController._instance.UpdateActionsList(actions, false, false);
        OverlayController._instance.EnableExtraActions();
        rigid = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        lastPos = transform.position;
        ChangeSpeed(TurnController._instance.speedChangeTgl.isOn);
    }

	// Update is called once per frame
	void Update () {
        // Key inputs here for hotkeys
        if (!isPerformingActions) {
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

            if (Input.GetKeyDown(KeyCode.E)) {
                AddAction("RepelEnemies");
            }

            if (Input.GetKeyDown(KeyCode.M)) {
                AddAction("RevealMap");
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                AddAction("ToggleConversion");
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                LoadActionList();
            }

            if (Input.GetKeyDown(KeyCode.Backspace)) {
                RemoveAction();
            }
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
        if (!isPerformingActions) {
            if (TurnController._instance.GetIsPlayerTurn()) {
                if (currNumOfActions > 0) {

                    if (actions[actions.Count - 1] == "RevealMap") {
                        isRevealUsed = !isRevealUsed;
                        OverlayController._instance.revealBtn.interactable = true;
                    }

                    int cost = CheckCost(actions[actions.Count-1]);
                    if (actions[actions.Count - 1] == "ToggleConversion") {
                        isConversionActive = !isConversionActive;
                        if (isConversionActive) {
                            estCost++;
                        } else {
                            estCost--;
                        }
                        OverlayController._instance.convertBtn.interactable = true;
                    }
                    currNumOfActions--;
                    estCost -= cost;
                    actions.RemoveAt(actions.Count-1);
                    OverlayController._instance.UpdateActionsList(actions, false, false);
                    OverlayController._instance.UpdateActionPoints(actionPoints, estCost);
                }
            }
        }
    }

    public void LoadActionList() {
        TurnController._instance.EnableSpeedChange(false);
        if (!isPerformingActions) {
            if (TurnController._instance.GetIsPlayerTurn()) {
                StartCoroutine(ExecuteActionList());
            }
        }
    }

    // For buttons to use
    public void AddAction(String action) {
        if (TurnController._instance.GetIsPlayerTurn()) {
            if (!isPerformingActions) {
                if (actionPoints > estCost || (action == "ToggleConversion" && isConversionActive)) {
                    if (currNumOfActions < maxNumOfActions) {
                        bool isMovement = CheckIfMovement(action);
                        bool actionAdded = false;
                        if (action == "ToggleConversion" && isConvertAvailable) {
                            isConversionActive = !isConversionActive;
                            if (isConversionActive) {
                                estCost++;
                            } else {
                                estCost--;
                            }
                            actions.Add(action);
                            actionAdded = true;
                            OverlayController._instance.convertBtn.interactable = false;
                        }
                        int cost = CheckCost(action);

                        if (action == "RevealMap" && isRevealAvailable) {
                            isRevealUsed = !isRevealUsed;
                            actions.Add(action);
                            actionAdded = true;
                            OverlayController._instance.revealBtn.interactable = false;
                        } else if (action == "RepelEnemies" && isRepelAvailable) {
                            actions.Add(action);
                            actionAdded = true;
                        } else if (isMovement) {
                            actions.Add(action);
                            actionAdded = true;
                        }

                        if (actionAdded) {
                            currNumOfActions++;
                            estCost += cost;
                            OverlayController._instance.UpdateActionsList(actions, false, isMovement);
                            OverlayController._instance.UpdateActionPoints(actionPoints, estCost);
                        }
                    } else {
                        OverlayController._instance.DisplayMax();
                    }
                }
            }
        }
    }

    public bool CheckIfMovement(String action) {
        return action.Contains("Move");
    }

    public int CheckCost(String action) {
        if (CheckIfMovement(action)) {
            return 1;
        } else if (action == "RepelEnemies") {
            return 3;
        } else if (action == "RevealMap") {
            return 5;
        }

        return 0;
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

    IEnumerator RepelEnemies() {
        if (!isMoving) {
            TurnController._instance.repelTurns = 3;
            DataCollectionController._instance.UpdateRepelUsed();
        }
        yield return new WaitForSeconds(0.05f);
    }

    IEnumerator RevealMap() {
        if (!isMoving) {
            CameraController._instance.RevealMapWrapper();
            DataCollectionController._instance.UpdateRevealUsed();
        }
        yield return new WaitForSeconds(5f);
    }

    IEnumerator ToggleConversion() {
        if (!isMoving) {
            DarkRoomController._instance.ToggleConversionAbility(isConversionActive);
            DataCollectionController._instance.UpdateConvertUsed();
        }
        yield return new WaitForSeconds(0.05f);
    }

    IEnumerator ExecuteActionList() {
        if (!isPerformingActions) {
            isPerformingActions = true;
            actions.Reverse();
            for (int i = actions.Count - 1; i >= 0; i--) {
                int cost = CheckCost(actions[i]);
                currNumOfActions--;
                actionPoints -= cost;
                estCost -= cost;
                OverlayController._instance.UpdateActionPoints(actionPoints, estCost);
                yield return StartCoroutine(actions[i]);
                actions.RemoveAt(i);
                OverlayController._instance.UpdateActionsList(actions, true, false);
            }
            actions.Clear();
            OverlayController._instance.EnableExtraActions();
            //StatsController._instance.UpdateActionsList(actions);
            TurnController._instance.EnemyTurn();
            isPerformingActions = false;
            if (isConversionActive) {
                estCost = 1;
                actionPoints--;
                OverlayController._instance.UpdateActionPoints(actionPoints, estCost);
            }
        }
    }

    IEnumerator Move(Vector2 source, Vector2 target) {
        DataCollectionController._instance.UpdateMovementUsed();
        lastPos = transform.position;
        isMoving = true;
        DarkRoomController._instance.SetPlayerMoving(isMoving);
        audioSrc.Play();
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
            moveDuration = 0.4f;
            moveWait = moveDuration + 0.05f;
        } else {
            moveDuration = 0.75f;
            moveWait = moveDuration + 0.1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall") {
            hitWall = true;
        }

        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyMap>().isAlive = false;
			MapStateController._instance.LoadCombatScene(collision.gameObject, SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Exit") {
            if (GameObject.FindObjectOfType<KeyController>() == null) {
                DataCollectionController._instance.UpdateIsWin(true);
            }
            MapStateController._instance.SaveEndOfLevelData();
            DataCollectionController._instance.WriteToFile();
            SceneManager.LoadScene("Hub");
        }

        if (collision.gameObject.tag == "Key1") {
            collision.gameObject.GetComponent<KeyController>().ShowLore();
        }

        if (collision.gameObject.tag == "Key2") {
            collision.gameObject.GetComponent<KeyController>().ShowLore();
        }

        if (collision.gameObject.tag == "Key3") {
            collision.gameObject.GetComponent<KeyController>().ShowLore();
        }

        if (collision.gameObject.tag == "Key4") {
            collision.gameObject.GetComponent<KeyController>().ShowLore();
        }
    }
}
    
