using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapStateController : MonoBehaviour {
    public static MapStateController _instance;

    public GameObject player;
    public GameObject[] enemies;
    public GameObject key;

    private int enemiesCount;
    private string fileName;

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
        if (scene.buildIndex == 0) {
            player = GameObject.FindGameObjectWithTag("Player");
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            key = GameObject.FindGameObjectWithTag("Key");
            enemiesCount = enemies.Length;
            fileName = Path.Combine(Application.persistentDataPath, "SaveData.json");
            LoadMapData();
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        key = GameObject.FindGameObjectWithTag("Key");
        enemiesCount = enemies.Length;
        fileName = Path.Combine(Application.persistentDataPath, "SaveData.json");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                SaveMapData();
                SceneManager.LoadScene(1);
            } else {
                SceneManager.LoadScene(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            if (File.Exists(fileName)) {
                File.Delete(fileName);
                SceneManager.LoadScene(0);
            }
        }
    }

    public void SaveMapData() {
        MapData data = new MapData();

        data.playerPos = player.transform.position;
        data.playerAP = player.GetComponent<PlayerProgramController>().actionPoints;

        data.enemyPos = new Vector3[enemiesCount];
        data.enemyState = new bool[enemiesCount];

        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                EnemySaveTest enemyData = enemy.GetComponent<EnemySaveTest>();
                data.enemyPos[enemyData.enemyID] = enemy.transform.position;
                data.enemyState[enemyData.enemyID] = enemyData.isAlive;
            }
        }

        data.keyState = (key != null);

        string json = JsonUtility.ToJson(data);

        if (File.Exists(fileName)) {
            File.Delete(fileName);
        }

        File.WriteAllText(fileName, json);
    }

    public void LoadMapData() {
        if (File.Exists(fileName)) {
            string jsonSave = File.ReadAllText(fileName);

            MapData loadedData = JsonUtility.FromJson<MapData>(jsonSave);

            player.transform.position = loadedData.playerPos;
            player.GetComponent<PlayerProgramController>().actionPoints = loadedData.playerAP;

            foreach (GameObject enemy in enemies) {
                EnemySaveTest enemyData = enemy.GetComponent<EnemySaveTest>();
                enemy.transform.position = loadedData.enemyPos[enemyData.enemyID];
                enemyData.isAlive = loadedData.enemyState[enemyData.enemyID];
                if (!enemyData.isAlive) {
                    Destroy(enemy);
                }
            }

            if (!loadedData.keyState) {
                GameObject.FindGameObjectWithTag("Door").SetActive(false);
            }
        }
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