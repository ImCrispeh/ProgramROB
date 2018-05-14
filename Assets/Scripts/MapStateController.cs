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
	public GameObject generator;
    public GameObject[] enemies;
    public GameObject[] keys;

    private int enemiesCount;
    private string mapFileName;
    private string combatFileName;
    private string upgradeFileName;
    private string levelEndFileName;

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
        if (scene.name == "Level1v2") {
            endImg = GameObject.FindGameObjectWithTag("EndImg");
            endText = endImg.GetComponentInChildren<Text>();
            player = GameObject.FindGameObjectWithTag("Player");
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            keys = GameObject.FindGameObjectsWithTag("Key");
            enemiesCount = enemies.Length;
            combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
            mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
            upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
            levelEndFileName = Path.Combine(Application.persistentDataPath, "LevelEndSaveData.json");
            LoadMapData();
            if (File.Exists(upgradeFileName)) {
                IntegrateUpgrades();
            }
        }

		if (scene.name == "Level1v2_Combat") {
            endImg = GameObject.FindGameObjectWithTag("EndImg");
            endText = endImg.GetComponentInChildren<Text>();
            //player = GameObject.FindGameObjectWithTag("Player");
			generator = GameObject.FindGameObjectWithTag ("Generator");
			combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
			mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
            //LoadCombatData();
        }
    }

    // Use this for initialization
    void Start () {
        endImg = GameObject.FindGameObjectWithTag("EndImg");
        endText = endImg.GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        keys = GameObject.FindGameObjectsWithTag("Key");
        enemiesCount = enemies.Length;
        mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
        combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
        upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
    }

    //private void Update() {
    //    if (Input.GetKeyDown(KeyCode.R)) {
    //        if (File.Exists(combatFileName)) {
    //            File.Delete(combatFileName);
    //        }

    //        if (File.Exists(mapFileName)) {
    //            File.Delete(mapFileName);
    //        }
    //        Time.timeScale = 1;
    //        SceneManager.LoadScene("UpgradeTest");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Escape)) {
    //        if (File.Exists(combatFileName)) {
    //            File.Delete(combatFileName);
    //        }

    //        if (File.Exists(mapFileName)) {
    //            File.Delete(mapFileName);
    //        }
    //        Time.timeScale = 1;
    //        Application.Quit();
    //    }
    //}

    public void IntegrateUpgrades() {
        PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
        string jsonSave = File.ReadAllText(upgradeFileName);

        UpgradeData loadedUpgradeData = JsonUtility.FromJson<UpgradeData>(jsonSave);

        //make initial stats consistent with last level
        LoadPreviousLevelData();

        playerCont.maxActionPoints += loadedUpgradeData.apIncrease;
        playerCont.maxHealth += loadedUpgradeData.healthIncrease;
        playerCont.currHealth = playerCont.maxHealth;
        playerCont.actionPoints = playerCont.maxActionPoints;
        playerCont.damage += loadedUpgradeData.damageIncrease;
        playerCont.visibilityMultiplier += loadedUpgradeData.visibilityIncrease;

        //save upgrades in the map data
        playerCont.lastPos = playerCont.transform.position;
        SaveMapData(null);

        CombatData combatData = new CombatData();
        combatData.health = playerCont.maxHealth;
        combatData.damage = playerCont.damage;
        string json = JsonUtility.ToJson(combatData);
        if (File.Exists(combatFileName)) {
            File.Delete(combatFileName);
        }
        combatData.health = playerCont.maxHealth;
        File.WriteAllText(combatFileName, json);

        File.Delete(upgradeFileName);
    }

	public void SaveMapData(GameObject details) {
        MapData data = new MapData();
        PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
        data.playerPos = playerCont.lastPos;
        data.playerAP = playerCont.actionPoints;
        data.playerMaxHealth = playerCont.maxHealth;
        data.playerMaxAP = playerCont.maxActionPoints;
        data.playerVisibility = playerCont.visibilityMultiplier;

        data.enemyPos = new Vector3[enemiesCount];
        data.enemyState = new bool[enemiesCount];
        data.keyState = new bool[keys.Length];

        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                EnemyMap enemyData = enemy.GetComponent<EnemyMap>();
                data.enemyPos[enemyData.enemyID] = enemy.transform.position;
                data.enemyState[enemyData.enemyID] = enemyData.isAlive;
            }
        }

        foreach (GameObject key in keys) {
            if (key != null) {
                KeyController keyData = key.GetComponent<KeyController>();
                data.keyState[keyData.keyID] = keyData.isCollected;
            }
        }

        if (details != null) {
            data.isEnemy = details.GetComponent<EnemyMap>().isEnemy;
            data.isSpider = details.GetComponent<EnemyMap>().isSpider;
            data.isTurret = details.GetComponent<EnemyMap>().isTurret;
            data.isTank = details.GetComponent<EnemyMap>().isTank;
            data.enemyAmt = details.GetComponent<EnemyMap>().enemyAmt;
            data.spiderAmt = details.GetComponent<EnemyMap>().spiderAmt;
            data.turretAmt = details.GetComponent<EnemyMap>().turretAmt;
            data.tankAmt = details.GetComponent<EnemyMap>().tankAmt;
        }

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
            playerCont.maxHealth = loadedMapData.playerMaxHealth;
            playerCont.maxActionPoints = loadedMapData.playerMaxAP;
            playerCont.visibilityMultiplier = loadedMapData.playerVisibility;

            foreach (GameObject enemy in enemies) {
                EnemyMap enemyData = enemy.GetComponent<EnemyMap>();
                enemy.transform.position = loadedMapData.enemyPos[enemyData.enemyID];
                enemyData.isAlive = loadedMapData.enemyState[enemyData.enemyID];
                if (!enemyData.isAlive) {
                    Destroy(enemy);
                }
            }

            foreach (GameObject key in keys) {
                if (key != null) {
                    KeyController keyData = key.GetComponent<KeyController>();
                    keyData.isCollected = loadedMapData.keyState[keyData.keyID];
                    if (keyData.isCollected) {
                        Destroy(key);
                        Destroy(keyData.door);
                    }
                }
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
        PlayerCombatController playerCont = player.GetComponent<PlayerCombatController>();
        data.health = playerCont.health;
        data.apToAdd = 5;
        data.upgradesToAdd = 5;

        if (File.Exists(combatFileName)) {
            string jsonSave = File.ReadAllText(combatFileName);
            CombatData loadedCombatData = JsonUtility.FromJson<CombatData>(jsonSave);
            data.damage = loadedCombatData.damage;
            Debug.Log(data.damage);
        }

        string upgradeCostFileName = Path.Combine(Application.persistentDataPath, "UpgradeCostSaveData.json");
        if (File.Exists(upgradeCostFileName)) {
            string jsonSave = File.ReadAllText(upgradeCostFileName);
            UpgradeCostData upgradeCostData = JsonUtility.FromJson<UpgradeCostData>(jsonSave);
            upgradeCostData.upgradePoints += 5;
            string jsonCosts = JsonUtility.ToJson(upgradeCostData);
            File.WriteAllText(upgradeCostFileName, jsonCosts);
        }
        string json = JsonUtility.ToJson(data);

        if (File.Exists(combatFileName)) {
            File.Delete(combatFileName);
        }

        File.WriteAllText(combatFileName, json);
    }

    public void LoadCombatData() {
		if (File.Exists(combatFileName) && File.Exists(mapFileName)) {
            string jsonSave = File.ReadAllText(combatFileName);
            CombatData loadedCombatData = JsonUtility.FromJson<CombatData>(jsonSave);
            //if (player != null) {
                PlayerCombatController playerCont = player.GetComponent<PlayerCombatController>();

                playerCont.health = loadedCombatData.health;
                playerCont.damage = loadedCombatData.damage;
            //}

            jsonSave = File.ReadAllText(mapFileName);
			MapData loadedMapData = JsonUtility.FromJson<MapData> (jsonSave);
            playerCont.transform.GetChild(2).GetComponent<Light>().spotAngle = 50 * (loadedMapData.playerVisibility - 0.05f);
            generator.GetComponent<CombatAreaGeneratorv3> ().isEnemy = loadedMapData.isEnemy;
			generator.GetComponent<CombatAreaGeneratorv3> ().isSpider = loadedMapData.isSpider;
			generator.GetComponent<CombatAreaGeneratorv3> ().isTurret = loadedMapData.isTurret;
            generator.GetComponent<CombatAreaGeneratorv3>().isTank = loadedMapData.isTank;
            generator.GetComponent<CombatAreaGeneratorv3> ().enemyAmt = loadedMapData.enemyAmt;
			generator.GetComponent<CombatAreaGeneratorv3> ().spiderAmt = loadedMapData.spiderAmt;
			generator.GetComponent<CombatAreaGeneratorv3> ().turretAmt = loadedMapData.turretAmt;
            generator.GetComponent<CombatAreaGeneratorv3>().tankAmt = loadedMapData.tankAmt;
        }
    }

    public void SaveEndOfLevelData() {
        LevelEndData LevelEndData = new LevelEndData();
        PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
        LevelEndData.playerMaxHealth = playerCont.maxHealth;
        LevelEndData.playerAP = playerCont.maxActionPoints;
        LevelEndData.playerDamage = playerCont.damage;
        LevelEndData.playerVisibility = playerCont.visibilityMultiplier;
        
        string json = JsonUtility.ToJson(LevelEndData);
        if (File.Exists(levelEndFileName)) {
            File.Delete(levelEndFileName);
        }

        File.WriteAllText(levelEndFileName, json);
    }

    public void LoadPreviousLevelData() {
        if (File.Exists(levelEndFileName)) {
            string jsonSave = File.ReadAllText(levelEndFileName);
            LevelEndData loadedLevelEndData = JsonUtility.FromJson<LevelEndData>(jsonSave);
            PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
            playerCont.maxActionPoints = loadedLevelEndData.playerAP;
            playerCont.maxHealth = loadedLevelEndData.playerMaxHealth;
            playerCont.visibilityMultiplier = loadedLevelEndData.playerVisibility;
            playerCont.damage = loadedLevelEndData.playerDamage;
        }
    }
    public void LoadCombatScene(GameObject details) {
        SaveMapData(details);
        SceneManager.LoadScene("Level1v2_Combat");
    }

    public void LoadMapScene() {
        SaveCombatData();
        SceneManager.LoadScene("Level1v2");
    }

    public void CheckEnemiesAlive() {
        StartCoroutine(EnemiesCheck());
    }

    IEnumerator EnemiesCheck() {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("check");
        GameObject[] check = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(check.Length);
        if (check.Length == 0) {
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
            DarkRoomController._instance.ToggleEffect(true);
        }
        Time.timeScale = 0;
    }
}

[Serializable]
class MapData {
    public Vector3 playerPos;
    public int playerAP;
    public int playerMaxHealth;
    public int playerMaxAP;
    public float playerVisibility;

    public Vector3[] enemyPos;
    public bool[] enemyState;

	public bool[] keyState;

	public bool isEnemy;
	public bool isSpider;
	public bool isTurret;
    public bool isTank;
	public int enemyAmt;
	public int spiderAmt;
	public int turretAmt;
    public int tankAmt;
}

[Serializable]
class CombatData {
    public int health;
    public int apToAdd;
    public int damage;
    public int upgradesToAdd;
}

//this is used to capture the player stats at the beginning of each level, before the chosen upgrades are applied
[Serializable]
class LevelEndData {
    public int playerAP;
    public int playerMaxHealth;
    public float playerVisibility;
    public int playerDamage;
}