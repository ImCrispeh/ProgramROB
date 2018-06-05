using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour {
    public static GameReset _instance;

    public GameObject pauseEndScreen;
    public Button continueBtn;
    public Button resetBtn;
    public Text resetText;
    public Button exitBtn;
    public Text pauseEndText;
    public bool isPaused;
    public bool endOfGame;
    public bool gameOver;

    private string mapFileName;
    private string combatFileName;
    private string upgradeFileName;
    private string levelEndFileName;
    private string upgradeCostFileName;
    private string totalUpgradeFileName;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name != "Title") {
            pauseEndScreen = GameObject.FindGameObjectWithTag("PauseScreen");
            continueBtn = GameObject.FindGameObjectWithTag("ContBtn").GetComponent<Button>();
            resetBtn = GameObject.FindGameObjectWithTag("ResetBtn").GetComponent<Button>();
            exitBtn = GameObject.FindGameObjectWithTag("ExitBtn").GetComponent<Button>();
            resetText = GameObject.FindGameObjectWithTag("ResetText").GetComponent<Text>();
            pauseEndText = GameObject.FindGameObjectWithTag("PauseText").GetComponent<Text>();
            continueBtn.onClick.AddListener(UnpauseGame);
            resetBtn.onClick.AddListener(ResetGame);
            exitBtn.onClick.AddListener(ExitGame);
            pauseEndScreen.SetActive(false);
        } else {
            Destroy(this.gameObject);
        }
    }

        // Use this for initialization
    void Start () {
        upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
        mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
        combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
        levelEndFileName = Path.Combine(Application.persistentDataPath, "LevelEndSaveData.json");
        upgradeCostFileName = Path.Combine(Application.persistentDataPath, "UpgradeCostSaveData.json");
        totalUpgradeFileName = Path.Combine(Application.persistentDataPath, "TotalUpgradeSaveData.json");
    }

    private void OnApplicationQuit() {
        if (File.Exists(combatFileName)) {
            File.Delete(combatFileName);
        }

        if (File.Exists(mapFileName)) {
            File.Delete(mapFileName);
        }

        if (File.Exists(upgradeFileName)) {
            File.Delete(upgradeFileName);
        }

        if (File.Exists(levelEndFileName)) {
            File.Delete(levelEndFileName);
        }

        if (File.Exists(upgradeCostFileName)) {
            File.Delete(upgradeCostFileName);
        }

        if (File.Exists(totalUpgradeFileName)) {
            File.Delete(totalUpgradeFileName);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            MapStateController._instance.SaveEndOfLevelData();

            if (File.Exists(combatFileName)) {
                File.Delete(combatFileName);
            }

            if (File.Exists(mapFileName)) {
                File.Delete(mapFileName);
            }

            if (File.Exists(upgradeFileName)) {
                File.Delete(upgradeFileName);
            }

            Time.timeScale = 1;
            SceneManager.LoadScene(5);
        }

        if (SceneManager.GetActiveScene().name == "Tutorial") {
            if (Input.GetKeyDown(KeyCode.X)) {
                if (File.Exists(combatFileName)) {
                    File.Delete(combatFileName);
                }

                if (File.Exists(mapFileName)) {
                    File.Delete(mapFileName);
                }
                Time.timeScale = 1;
                SceneManager.LoadScene("Hub");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!gameOver) {
                if (!isPaused) {
                    PauseGame();
                } else {
                    UnpauseGame();
                }
            }
        }
    }

    public void PauseGame() {
        isPaused = true;
        pauseEndScreen.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PlayerCombatController>() != null) {
            player.GetComponent<PlayerCombatController>().enabled = false;
        }

        if (player.GetComponent<PlayerProgramController>() != null) {
            player.GetComponent<PlayerProgramController>().enabled = false;
        }

        foreach (Button btn in GameObject.FindObjectsOfType<Button>()) {
            btn.interactable = false;
        }

        continueBtn.interactable = true;
        resetBtn.interactable = true;
        exitBtn.interactable = true;

        foreach (Toggle tgl in GameObject.FindObjectsOfType<Toggle>()) {
            tgl.interactable = false;
        }

        Time.timeScale = 0;
    }

    public void UnpauseGame() {
        isPaused = false;
        pauseEndScreen.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PlayerCombatController>() != null) {
            player.GetComponent<PlayerCombatController>().enabled = true;
        }

        if (player.GetComponent<PlayerProgramController>() != null) {
            player.GetComponent<PlayerProgramController>().enabled = true;
        }

        foreach (Button btn in GameObject.FindObjectsOfType<Button>()) {
            btn.interactable = true;
        }

        foreach (Toggle tgl in GameObject.FindObjectsOfType<Toggle>()) {
            tgl.interactable = true;
        }

        if (GameObject.FindObjectOfType<OverlayController>() != null) {
            OverlayController._instance.EnableExtraActions();
        }

        Time.timeScale = 0;
    }

    public void ResetGame() {
        if (endOfGame) {

            endOfGame = false;

            if (File.Exists(combatFileName)) {
                File.Delete(combatFileName);
            }

            if (File.Exists(mapFileName)) {
                File.Delete(mapFileName);
            }

            if (File.Exists(upgradeFileName)) {
                File.Delete(upgradeFileName);
            }

            if (File.Exists(levelEndFileName)) {
                File.Delete(levelEndFileName);
            }

            if (File.Exists(upgradeCostFileName)) {
                File.Delete(upgradeCostFileName);
            }

            if (File.Exists(totalUpgradeFileName)) {
                File.Delete(totalUpgradeFileName);
            }

            Time.timeScale = 1;
            Destroy(MapStateController._instance.gameObject);
            DataCollectionController._instance.WriteToFile();
            Destroy(DataCollectionController._instance.gameObject);
            SceneManager.LoadScene("Title");
        } else {
            if (File.Exists(combatFileName)) {
                File.Delete(combatFileName);
            }

            if (File.Exists(mapFileName)) {
                File.Delete(mapFileName);
            }

            if (File.Exists(upgradeFileName)) {
                File.Delete(upgradeFileName);
            }

            Time.timeScale = 1;
            DataCollectionController._instance.WriteToFile();
            SceneManager.LoadScene("Hub");
        }
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void EndGame(bool isWin, string reason) {
        endOfGame = isWin;
        if (isWin) {
            pauseEndText.text = "You Win!";
        } else {
            pauseEndText.text = "You Lose" + "\n" + reason;
            resetText.text = "Return to hub";
        }

        pauseEndScreen.SetActive(true);
        continueBtn.gameObject.SetActive(false);
        if (DarkRoomController._instance != null) {
            DarkRoomController._instance.ToggleEffect(true);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PlayerCombatController>() != null) {
            player.GetComponent<PlayerCombatController>().enabled = false;
        }

        if (player.GetComponent<PlayerProgramController>() != null) {
            player.GetComponent<PlayerProgramController>().enabled = false;
        }

        foreach (Button btn in GameObject.FindObjectsOfType<Button>()) {
            btn.interactable = false;
        }

        resetBtn.interactable = true;
        exitBtn.interactable = true;

        foreach (Toggle tgl in GameObject.FindObjectsOfType<Toggle>()) {
            tgl.interactable = false;
        }

        Time.timeScale = 0;
    }
}
