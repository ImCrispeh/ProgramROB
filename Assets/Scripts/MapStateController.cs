using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapStateController : MonoBehaviour {
    public static MapStateController _instance;

    public GameObject endImg;
    public Text endText;

    public GameObject player;
    public GameObject[] enemies;
    public GameObject key;

    private int enemiesCount;
    private string mapFileName;
    private string combatFileName;

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
        endImg = GameObject.FindGameObjectWithTag("EndImg");
        endText = endImg.GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (scene.buildIndex == 0) {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            key = GameObject.FindGameObjectWithTag("Key");
            enemiesCount = enemies.Length;
            combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
            mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
            LoadMapData();
        }

        if (scene.buildIndex == 1) {
            combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
            LoadCombatData();
        }
    }

    // Use this for initialization
    void Start () {
        endImg = GameObject.FindGameObjectWithTag("EndImg");
        endText = endImg.GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        key = GameObject.FindGameObjectWithTag("Key");
        enemiesCount = enemies.Length;
        mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
        combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (File.Exists(combatFileName)) {
                File.Delete(combatFileName);
            }

            if (File.Exists(mapFileName)) {
                File.Delete(mapFileName);
            }
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (File.Exists(combatFileName)) {
                File.Delete(combatFileName);
            }

            if (File.Exists(mapFileName)) {
                File.Delete(mapFileName);
            }
            Time.timeScale = 1;
            Application.Quit();
        }
    }

    public void SaveMapData() {
        MapData data = new MapData();

        data.playerPos = player.GetComponent<PlayerProgramController>().lastPos;
        data.playerAP = player.GetComponent<PlayerProgramController>().actionPoints;

        data.enemyPos = new Vector3[enemiesCount];
        data.enemyState = new bool[enemiesCount];

        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                EnemyMap enemyData = enemy.GetComponent<EnemyMap>();
                data.enemyPos[enemyData.enemyID] = enemy.transform.position;
                data.enemyState[enemyData.enemyID] = enemyData.isAlive;
            }
        }

        data.keyState = (key != null);

        string json = JsonUtility.ToJson(data);

        if (File.Exists(mapFileName)) {
            File.Delete(mapFileName);
        }

        File.WriteAllText(mapFileName, json);
    }

    public void LoadMapData() {
        PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
        if (File.Exists(mapFileName)) {
            string jsonSave = File.ReadAllText(mapFileName);

            MapData loadedMapData = JsonUtility.FromJson<MapData>(jsonSave);

            player.transform.position = loadedMapData.playerPos;
            playerCont.actionPoints = loadedMapData.playerAP;

            foreach (GameObject enemy in enemies) {
                EnemyMap enemyData = enemy.GetComponent<EnemyMap>();
                enemy.transform.position = loadedMapData.enemyPos[enemyData.enemyID];
                enemyData.isAlive = loadedMapData.enemyState[enemyData.enemyID];
                if (!enemyData.isAlive) {
                    Destroy(enemy);
                }
            }

            if (!loadedMapData.keyState) {
                Destroy(key);
                Destroy(GameObject.FindGameObjectWithTag("Door"));
            }
        }

        if (File.Exists(combatFileName)) {
            string jsonSave = File.ReadAllText(combatFileName);
            CombatData loadedCombatData = JsonUtility.FromJson<CombatData>(jsonSave);
            playerCont.currHealth = loadedCombatData.health;
            playerCont.actionPoints = playerCont.actionPoints + loadedCombatData.apToAdd;
        }
    }

    public void SaveCombatData() {
        CombatData data = new CombatData();

        data.health = player.GetComponent<PlayerCombatController>().health;
        data.apToAdd = 5;

        string json = JsonUtility.ToJson(data);

        if (File.Exists(combatFileName)) {
            File.Delete(combatFileName);
        }

        File.WriteAllText(combatFileName, json);
    }

    public void LoadCombatData() {
        if (File.Exists(combatFileName)) {
            string jsonSave = File.ReadAllText(combatFileName);
            CombatData loadedCombatData = JsonUtility.FromJson<CombatData>(jsonSave);

            player.GetComponent<PlayerCombatController>().health = loadedCombatData.health;
        }
    }

    public void LoadCombatScene() {
        SaveMapData();
        SceneManager.LoadScene(1);
    }

    public void LoadMapScene() {
        SaveCombatData();
        SceneManager.LoadScene(0);
    }

    public void CheckEnemiesAlive() {
        Debug.Log("check");
        GameObject[] check = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(check.Length);
        if (check.Length == 1) {
            LoadMapScene();
        }
    }

    public void EndGame(bool isWin, String reason) {
        if (isWin) {
            endText.text = "You Win!";
        } else {
            endText.text = "You Lose" + "\n" + reason;
        }
        endImg.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        if (DarkRoomController._instance != null) {
            DarkRoomController._instance.ToggleEffect(false);
        }
        Time.timeScale = 0;
    }
}

[Serializable]
class MapData {
    public Vector3 playerPos;
    public int playerAP;

    public Vector3[] enemyPos;
    public bool[] enemyState;

    public bool keyState;
}

[Serializable]
class CombatData {
    public int health;
    public int apToAdd;
}