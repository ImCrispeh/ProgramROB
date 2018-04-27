using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour {
    public static StatsController _instance;
    public Text actionPointsText;
    public Text actionsText;
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

    public void UpdateActionPoints(int actionPoints) {
        actionPointsText.text = "Remaining action points:" + "\n" + actionPoints;
    }

    public void UpdateActionsList(List<string> actions) {
        actionsText.text = "";
        foreach (string action in actions) {
            actionsText.text += action + "\n";
        }
    }

    public void UpdateHealth(int health) {
        healthText.text = "Health: " + health;
    }
}
