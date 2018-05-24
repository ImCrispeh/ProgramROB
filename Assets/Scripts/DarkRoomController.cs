using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomController : MonoBehaviour {
    public static DarkRoomController _instance;
    public GameObject darkness;
    public GameObject maskObj;
    public Transform maskParent;
    public GameObject playerMask;
	public GameObject PowerGenMask;
	public GameObject Room2PowerGenMask;
    public GameObject[] enemyMasks;
    public GameObject[] enemies;
    public Transform player;
    public bool isPlayerMoving = false;
    public bool isEnemiesMoving = false;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player.transform.GetChild(0).GetComponent<Light>().spotAngle = 50 * (player.GetComponent<PlayerProgramController>().visibilityMultiplier - 0.05f);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ShowEnemies(false);
		//PowerGenMask.SetActive (false);
		//Room2PowerGenMask.SetActive (false);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ShowEnemies(bool toggle) {
        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                enemy.transform.GetChild(0).gameObject.SetActive(toggle);
            }
        }
    }

    public void ToggleEffect(bool isEnd) {
        //darkness.SetActive(toggle);
        if (isEnd) {
            player.transform.GetChild(0).GetComponent<Light>().type = LightType.Directional;
            player.transform.GetChild(0).GetComponent<Light>().intensity = 1;
        }
    }

    public void ToggleConversionAbility(bool isOn) {
        if (isOn) {
            player.transform.GetChild(0).GetComponent<Light>().spotAngle *= 1.5f;
        } else {
            player.transform.GetChild(0).GetComponent<Light>().spotAngle /= 1.5f;
        }
    }

    public void SetPlayerMoving(bool moving) {
        isPlayerMoving = moving;
    }

    public void SetEnemiesMoving(bool moving) {
        isEnemiesMoving = moving;
    }

	public void SetRoomsLight(bool choice){
		//PowerGenMask.SetActive (choice);
	}
	public void SetRoom2Light(bool choice){
		//Room2PowerGenMask.SetActive (choice);
	}
}
