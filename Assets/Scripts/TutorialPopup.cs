using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialPopup : MonoBehaviour {
    public static TutorialPopup _instance;
    public Text popupText;
    public Image playerImg;
    public GameObject player;
    public GameObject enemy;
    public GameObject[] pages;
    public int pageNum;
    public GameObject nextBtn;
    public GameObject closeBtn;
    public bool isStart;
    public Button instructions;
    public GameObject popup;
    public bool isEnded;

    public void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Use this for initialization
    void Start () {
        if (!isEnded) {
            CameraController._instance.LightMap();
            popup.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isStart) {
            isStart = true;
            OverlayController._instance.DisplayInstructions();
            OverlayController._instance.instructionsPanel.SetActive(false);
            instructions.interactable = false;
        }

        if (SceneManager.GetActiveScene().name == "Hub") {
            Destroy(gameObject);
        }
	}

    public void Next() {
        if (pageNum < 4) {
            pageNum++;
            pages[pageNum].SetActive(true);
            pages[pageNum-1].SetActive(false);
        }

        if (pageNum == 4) {
            closeBtn.SetActive(true);
            nextBtn.SetActive(false);
        }
        
    }

    public void Back() {
        if (pageNum > 0) {
            pageNum--;
            pages[pageNum].SetActive(true);
            pages[pageNum+1].SetActive(false);
        }

        if (pageNum == 3) {
            closeBtn.SetActive(false);
            nextBtn.SetActive(true);
        }
    }

    public void Close() {
        OverlayController._instance.instructionsPanel.SetActive(true);
        OverlayController._instance.DisplayInstructions();
        instructions.interactable = true;
        popup.SetActive(false);
        isEnded = true;
    }
}
