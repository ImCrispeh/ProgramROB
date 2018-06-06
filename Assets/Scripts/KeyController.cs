using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour {
    public bool isCollected;
    public GameObject loreScreen;
    public Button closeBtn;
    PlayerProgramController player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProgramController>();
    }

    public void ShowLore() {
        player.enabled = false;
        loreScreen.SetActive(true);
        foreach (Button btn in GameObject.FindObjectsOfType<Button>()) {
            btn.interactable = false;
        }

        foreach (Toggle tgl in GameObject.FindObjectsOfType<Toggle>()) {
            tgl.interactable = false;
        }

        closeBtn.interactable = true;
    }

    public void CloseLore() {
        player.enabled = true;
        loreScreen.SetActive(false);
        foreach (Button btn in GameObject.FindObjectsOfType<Button>()) {
            btn.interactable = true;
        }

        foreach (Toggle tgl in GameObject.FindObjectsOfType<Toggle>()) {
            tgl.interactable = true;
        }

        OverlayController._instance.EnableExtraActions();

        Destroy(this.gameObject);
    }
}
