using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour {
    public static StatsController _instance;
    public Text actionPointsText;
    public int previousQueueSize;
    public Vector3[] actionImagesOffsets;
    public GameObject actionImagesSpawn;
    public GameObject movementImage;
    public Text maxActionsText;
    public Text healthText;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateActionPoints(int actionPoints, int currActions) {
        actionPointsText.text = "Remaining action points:" + "\n" + actionPoints + " (" + currActions + " queued to be used)";
    }

    public void UpdateActionsList(List<string> actions) {
        if (actions.Count > previousQueueSize) {
            string newAction = actions[actions.Count - 1];
            GameObject newActionImage = Instantiate(movementImage, actionImagesSpawn.transform) as GameObject;
            newActionImage.transform.localPosition = actionImagesOffsets[actions.Count - 1];
            if (newAction == "MoveUp") {
                newActionImage.transform.rotation = Quaternion.identity;
            } else if (newAction == "MoveLeft") {
                newActionImage.transform.rotation = Quaternion.Euler(0, 0, 90);
            } else if (newAction == "MoveRight") {
                newActionImage.transform.rotation = Quaternion.Euler(0, 0, -90);
            } else if (newAction == "MoveDown") {
                newActionImage.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        } else if (actions.Count < previousQueueSize) {
            Destroy(actionImagesSpawn.transform.GetChild(0).gameObject);
        }

        previousQueueSize = actions.Count;
    }

    public void UpdateHealth(int health) {
        healthText.text = "Health: " + health;
    }

    public void DisplayMax() {
        StartCoroutine(DisplayMaxMessage());
    }

    IEnumerator DisplayMaxMessage() {
        maxActionsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        maxActionsText.gameObject.SetActive(false);
    }
}
