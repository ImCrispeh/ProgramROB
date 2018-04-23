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
	public GameObject player;


	//for wall collision
	private Boolean isWall=false;
	private Vector3 currentPosition;
	private Boolean hitWall = false;
	// Use this for initialization
	void Start () {
        actions = new List<String>();
        rigid = GetComponent<Rigidbody2D>();
        UpdateActionPointsText(0);
		currentPosition=player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        // Key inputs here for testing before implementing on buttons
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
			//currentPosition=player.transform.position;
			AddAction("MoveRight");
			//if(isWall=true){
		    //player.transform.position=currentPosition;
			//}

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
                UpdateActionPointsText(1);
            }
        }
    }

    public void LoadActionList() {
        if (TurnController._instance.GetIsPlayerTurn()&& hitWall==false) {
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
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Wall") {
			Debug.Log ("wall");
			isWall = true;
		}
	}


	IEnumerator MoveRight() {

		//currentPosition=player.transform.position;


		if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x + moveDist, transform.position.y)));
        }
        

		//if(isWall==true){
		//player.transform.position=currentPosition;
		//}

		yield return new WaitForSeconds(1.2f);

    }

    IEnumerator MoveLeft() {
		//currentPosition=player.transform.position;

        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x - moveDist, transform.position.y)));
        }
        
		//if(isWall==true){
		//	player.transform.position=currentPosition;
		//}

		yield return new WaitForSeconds(1.2f);

    }

    IEnumerator MoveUp() {
		//currentPosition=player.transform.position;

        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x, transform.position.y + moveDist)));
        }

		//if(isWall==true){
			
		//	player.transform.position=currentPosition;

		//}


        yield return new WaitForSeconds(1.2f);
    }




    IEnumerator MoveDown() {
		//currentPosition=player.transform.position;

        if (!isMoving) {
            StartCoroutine(Move(transform.position, new Vector2(transform.position.x, transform.position.y - moveDist)));
        }

		//if(isWall==true){
		//	player.transform.position=currentPosition;
		//}

        yield return new WaitForSeconds(1.2f);
    }

    IEnumerator ExecuteActionList() {
        DarkRoomController._instance.SetMoving(true);
        foreach (String actionFunc in actions) {
            Debug.Log(actionFunc);
            yield return StartCoroutine(actionFunc);
        }
        actions.Clear();
        currNumOfActions = 0;
        UpdateActionText();
        DarkRoomController._instance.SetMoving(false);
        TurnController._instance.EnemyTurn();
    }

    IEnumerator Move(Vector2 source, Vector2 target) {
		currentPosition=player.transform.position;
        isMoving = true;
        while (moveTimer < moveDuration) {
            moveTimer += Time.deltaTime;
            rigid.MovePosition(Vector2.Lerp(source, target, moveTimer / moveDuration));
            yield return null;
        }
		while(isWall==true){

			player.transform.position=currentPosition;
			isWall = false;
		}

        //transform.position = target
        isMoving = false;
        moveTimer = 0f;
    }
}
    
