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
    public int actionPoints = 30;
    public List<String> actions;
    public Text actionsText;
    public Text actionPointsText;
    public Rigidbody2D rigid;

	public bool isCombat;

	//Free Movement Combat Variables
	public float speed;
	private Transform firePoint;
	public float damage = 1;
	public float fireRate = 0;
	public float timeToFire = 0;
	public LayerMask allowHit;

	// Use this for initialization
	void Start () {
        actions = new List<String>();
        rigid = GetComponent<Rigidbody2D>();
		firePoint = transform.GetChild(0).GetChild(0);
		if (firePoint == null) {
			Debug.Log ("Fire Point not Found");
		}
        UpdateActionPointsText(0);

	}

	void FixedUpdate () {
		if (isCombat) {
			FreeMovement ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isCombat) {
			// Key inputs here for testing before implementing on buttons
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				AddAction ("MoveRight");
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				AddAction ("MoveLeft");
			}

			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				AddAction ("MoveUp");
			}

			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				AddAction ("MoveDown");
			}

			if (Input.GetKeyDown (KeyCode.Space)) {
				LoadActionList ();
			}

			if (Input.GetKeyDown (KeyCode.Backspace)) {
				RemoveAction ();
			}
		} else {
			if (fireRate == 0) {
				if (Input.GetButtonDown ("Fire2")) {
					Shooting ();
				}
			} else {
				if (Input.GetButton ("Fire2") && Time.time > timeToFire) {
					timeToFire = Time.time + 1 / fireRate;
					Shooting ();
				}
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
        if (TurnController._instance.GetIsPlayerTurn()) {
            if (currNumOfActions > 0) {
                currNumOfActions--;
                actions.RemoveAt(currNumOfActions);
                UpdateActionText();
                UpdateActionPointsText(1);
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
                    UpdateActionText();
                    UpdateActionPointsText(-1);
                } else {
                    Debug.Log("Maximum amount of actions reached");
                }
            } else {
                Debug.Log("No action points remaining");
            }
        }
    }

    public void UpdateActionText() {
        actionsText.text = "";
        foreach (String action in actions) {
            actionsText.text += action + "\n";
        }
    }

    public void UpdateActionPointsText(int pointsChange) {
        actionPoints += pointsChange;
        actionPointsText.text = "Remaining action points:" + "\n" + actionPoints;
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

	//Free Movement Combat Functions
	private void FreeMovement () {
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");

		rigid.velocity = new Vector2 (moveX * speed, moveY * speed);
	}

	private void Shooting () {
		Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPos = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPos, mousePos - firePointPos, 100, allowHit);
		Debug.DrawLine (firePointPos, mousePos);
		if (hit.collider != null) {
			Debug.DrawLine (firePointPos, hit.point, Color.red);
			hit.collider.gameObject.GetComponent<TestEnemyController> ().Damage (damage);
		}
	}
}
    
