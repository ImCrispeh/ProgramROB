using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour {
    public static OverlayController _instance;
    public Text actionPointsText;
    public int previousQueueSize;
    public Vector3[] actionImagesOffsets;
    public GameObject actionImagesSpawn;
    public GameObject movementImage;
    public GameObject otherActionImage;
    public GameObject instructionsPanel;
    public Text maxActionsText;
    public Text healthText;
    public PlayerProgramController player;
    public GameObject[] actionBtns;
    public Toggle fastForwardTgl;
    public bool areHotkeysDisplayed;
    public Button repelBtn;
    public Button revealBtn;
    public Button convertBtn;

    //texts to display/hide hotkeys
    public Text moveUpText;
    public Text moveDownText;
    public Text moveLeftText;
    public Text moveRightText;
    public Text repelText;
    public Text revealText;
    public Text convertText;
    public Text clearMoveText;
    public Text executeMoveText;
    public Text instructionBtnText;
    public Text fastForwardText;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProgramController>();
        actionBtns = GameObject.FindGameObjectsWithTag("ActionBtn");
        fastForwardTgl = GameObject.FindGameObjectWithTag("FFToggle").GetComponent<Toggle>();
        DisplayHotkeys();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H)) {
            areHotkeysDisplayed = !areHotkeysDisplayed;
            DisplayHotkeys();
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            DisplayInstructions();
        }
    }

    public void UpdateActionPoints(int actionPoints, int estCost) {
        actionPointsText.text = "Remaining action points:" + "\n" + actionPoints + " (" + estCost + " queued to be used)";
    }

    public void UpdateActionsList(List<string> actions, bool isExecuted, bool isMovement) {
        if (actions.Count > previousQueueSize) {
            string newAction = actions[actions.Count - 1];
            GameObject newActionImage;
            if (isMovement) {
                newActionImage = Instantiate(movementImage, actionImagesSpawn.transform) as GameObject;
            } else {
                newActionImage = Instantiate(otherActionImage, actionImagesSpawn.transform) as GameObject;
            }
            newActionImage.transform.localPosition = actionImagesOffsets[actions.Count - 1];
            if (newAction == "MoveUp") {
                newActionImage.transform.rotation = Quaternion.identity;
            } else if (newAction == "MoveLeft") {
                newActionImage.transform.rotation = Quaternion.Euler(0, 0, 90);
            } else if (newAction == "MoveRight") {
                newActionImage.transform.rotation = Quaternion.Euler(0, 0, -90);
            } else if (newAction == "MoveDown") {
                newActionImage.transform.rotation = Quaternion.Euler(0, 0, 180);
            } else if (newAction == "RepelEnemies") {
                newActionImage.GetComponentInChildren<Text>().text = "Repel" + "\n" + "enemies";
            } else if (newAction == "RevealMap") {
                newActionImage.GetComponentInChildren<Text>().text = "Reveal" + "\n" + "map";
            } else if (newAction == "ToggleConversion") {
                newActionImage.GetComponentInChildren<Text>().text = "Toggle" + "\n" + "conversion";
            }
        } else if (actions.Count < previousQueueSize) {
            if (isExecuted) {
                Destroy(actionImagesSpawn.transform.GetChild(0).gameObject);
            } else {
                Destroy(actionImagesSpawn.transform.GetChild(actions.Count).gameObject);
            }
        }

        previousQueueSize = actions.Count;
    }

    public void UpdateHealth(int health) {
        healthText.text = "Health: " + health;
    }

    public void DisplayInstructions() {
        instructionsPanel.SetActive(!instructionsPanel.activeInHierarchy);
        if (instructionsPanel.activeInHierarchy) {
            Time.timeScale = 0;
            player.enabled = false;
            foreach (GameObject btn in actionBtns) {
                btn.GetComponent<Button>().interactable = false;
            }
            fastForwardTgl.interactable = false;
            repelBtn.interactable = false;
            revealBtn.interactable = false;
            convertBtn.interactable = false;
        } else {
            Time.timeScale = 1;
            player.enabled = true;
            foreach (GameObject btn in actionBtns) {
                btn.GetComponent<Button>().interactable = true;
            }
            fastForwardTgl.interactable = true;
            EnableExtraActions();
        }
    }

    public void EnableExtraActions() {
        PlayerProgramController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProgramController>();
        bool isRepel = player.isRepelAvailable;
        bool isReveal = player.isRevealAvailable;
        bool isRevealUsed = player.isRevealUsed;
        bool isConvert = player.isConvertAvailable;

        repelBtn.interactable = isRepel;
        revealBtn.interactable = (isReveal && !isRevealUsed);
        convertBtn.interactable = isConvert;
    }

    public void DisplayHotkeys() {
        if (areHotkeysDisplayed) {
            moveUpText.text = "W or ↑" + "\n" + "(1 AP)";
            moveDownText.text = "S or ↓" + "\n" + "(1 AP)";
            moveRightText.text = "D or →" + "\n" + "(1 AP)";
            moveLeftText.text = "A or ←" + "\n" + "(1 AP)";
            repelText.text = "Repel enemies" + "\n" + "(Lasts for 2 turns)" + "\n" + "(E) (3 AP)";
            revealText.text = "Reveal entire map" + "\n" + "(One use per map)" + "\n" + "(M) (5 AP)";
            convertText.text = "Convert energy to" + "\n" + "vision (Toggle)" + "\n" + "(1 use per turn)" + "\n" + "(C) (1 AP per turn)";
            clearMoveText.text = "Clear last action" + "\n" + "(Backspace)";
            executeMoveText.text = "Perform actions" + "\n" + "(Spacebar)";
            instructionBtnText.text = "Instructions (I)";
            fastForwardText.text = "Fast forward (F)";
        } else {
            moveUpText.text = "(1 AP)";
            moveDownText.text = "(1 AP)";
            moveRightText.text = "(1 AP)";
            moveLeftText.text = "(1 AP)";
            repelText.text = "Repel enemies" + "\n" + "(Lasts for 2 turns)" + "\n" + "(3 AP)";
            revealText.text = "Reveal entire map" + "\n" + "(One use per map)" + "\n" + "(5 AP)";
            convertText.text = "Convert energy to" + "\n" + "vision (Toggle)" + "\n" + "(1 use per turn)" + "\n" + "(1 AP per turn)";
            clearMoveText.text = "Clear last action";
            executeMoveText.text = "Perform actions";
            instructionBtnText.text = "Instructions";
            fastForwardText.text = "Fast forward";
        }
    }

    public void DisplayMax() {
        StartCoroutine(DisplayMaxMessage());
    }

    IEnumerator DisplayMaxMessage() {
        maxActionsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        maxActionsText.gameObject.SetActive(false);
    }
}
