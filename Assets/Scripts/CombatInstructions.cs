using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatInstructions : MonoBehaviour {
    public GameObject instructionsPanel;
    public Text instructionBtnText;
    public bool areHotkeysDisplayed;
    public PlayerCombatController player;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H)) {
            areHotkeysDisplayed = !areHotkeysDisplayed;
            DisplayHotkeys();
        }
    }

    public void DisplayHotkeys() {
        if (areHotkeysDisplayed) {
            instructionBtnText.text = "Instructions (I)";
        } else {
            instructionBtnText.text = "Instructions";
        }
    }

    public void DisplayInstructions() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatController>();
        }
        instructionsPanel.SetActive(!instructionsPanel.activeInHierarchy);
        if (instructionsPanel.activeInHierarchy) {
            Time.timeScale = 0;
            player.enabled = false;
        } else {
            Time.timeScale = 1;
            player.enabled = true;
        }
    }
}
