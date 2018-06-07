using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapStateController : MonoBehaviour {
    public static MapStateController _instance;

    public GameObject player;
	public GameObject generator;
    public GameObject[] enemies;
    public PowerUpObject[] powerUps;
    public GameObject key;
    public int currLevelNo;

    public bool key1Collected;
    public bool key2Collected;
    public bool key3Collected;
    public bool key4Collected;
    public int numKeys;

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
        if (scene.name == "Title") {
            Destroy(this.gameObject);
        }

        if (scene.buildIndex >= 1 && scene.buildIndex <= 5) {
            player = GameObject.FindGameObjectWithTag("Player");
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            powerUps = GameObject.FindObjectsOfType<PowerUpObject>();
            key = GameObject.FindGameObjectWithTag("Key" + (scene.buildIndex - 1));

            if (key != null) {
                if (scene.name == "Stage1" && key1Collected) {
                    Destroy(key);
                }

                if (scene.name == "Stage2" && key2Collected) {
                    Destroy(key);
                }

                if (scene.name == "Stage3" && key3Collected) {
                    Destroy(key);
                }

                if (scene.name == "Stage4" && key4Collected) {
                    Destroy(key);
                }
            }

            enemiesCount = enemies.Length;
            combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
            mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
            upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
            levelEndFileName = Path.Combine(Application.persistentDataPath, "LevelEndSaveData.json");
            LoadPreviousLevelData();
            LoadMapData();
            IntegrateUpgrades();
        }

		if (scene.name == "Combat") {
			generator = GameObject.FindGameObjectWithTag ("Generator");
			combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
			mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
        }

        if (scene.name == "Hub") {
            combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
            mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");

            if (File.Exists(combatFileName)) {
                File.Delete(combatFileName);
            }

            if (File.Exists(mapFileName)) {
                File.Delete(mapFileName);
            }

        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        powerUps = GameObject.FindObjectsOfType<PowerUpObject>();
        key = GameObject.FindGameObjectWithTag("Key" + SceneManager.GetActiveScene().buildIndex);

        if (key != null) {
            if (SceneManager.GetActiveScene().name == "Stage1" && key1Collected) {
                Destroy(key);
            }

            if (SceneManager.GetActiveScene().name == "Stage2" && key2Collected) {
                Destroy(key);
            }

            if (SceneManager.GetActiveScene().name == "Stage3" && key3Collected) {
                Destroy(key);
            }

            if (SceneManager.GetActiveScene().name == "Stage4" && key4Collected) {
                Destroy(key);
            }
        }
        enemiesCount = enemies.Length;
        mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
        combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
        upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
    }

    public void IntegrateUpgrades() {
        PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
        if (File.Exists(upgradeFileName)) {
            string jsonSave = File.ReadAllText(upgradeFileName);

            UpgradeData loadedUpgradeData = JsonUtility.FromJson<UpgradeData>(jsonSave);

            //make initial stats consistent with last level

            playerCont.maxActionPoints += loadedUpgradeData.apIncrease;
            playerCont.maxHealth += loadedUpgradeData.healthIncrease;
            playerCont.currHealth = playerCont.maxHealth;
            playerCont.actionPoints = playerCont.maxActionPoints;
            playerCont.damage += loadedUpgradeData.damageIncrease;
            playerCont.visibilityMultiplier += loadedUpgradeData.visibilityIncrease;
            if (!playerCont.isRepelAvailable) {
                playerCont.isRepelAvailable = loadedUpgradeData.isRepelPurchased;
            }

            if (!playerCont.isRevealAvailable) {
                playerCont.isRevealAvailable = loadedUpgradeData.isRevealPurchased;
            }

            if (playerCont.isConvertAvailable) {
                playerCont.isConvertAvailable = loadedUpgradeData.isConvertPurchased;
            }

            //save upgrades in the map data
            playerCont.lastPos = playerCont.transform.position;
            SaveMapData(null);

            File.Delete(upgradeFileName);
        }
    }

	public void SaveMapData(GameObject details) {
        MapData data = new MapData();
        PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
        data.playerPos = playerCont.lastPos;
        data.playerAP = playerCont.actionPoints;
        data.playerMaxHealth = playerCont.maxHealth;
        data.playerMaxAP = playerCont.maxActionPoints;
        data.playerCurrAP = playerCont.actionPoints;
        Debug.Log(data.playerCurrAP);
        data.playerVisibility = playerCont.visibilityMultiplier;
        data.isRepelAvailable = playerCont.isRepelAvailable;
        data.isRevealAvailable = playerCont.isRevealAvailable;
        data.isConvertAvailable = playerCont.isConvertAvailable;

        data.enemyPos = new Vector3[enemiesCount];
        data.enemyState = new bool[enemiesCount];
        data.powerUpState = new bool[powerUps.Length];
        data.keyState = (key == null);
        data.isFastForward = TurnController._instance.speedChangeTgl.isOn;
        data.areHotkeysDisplayed = OverlayController._instance.areHotkeysDisplayed;

        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                EnemyMap enemyData = enemy.GetComponent<EnemyMap>();
                data.enemyPos[enemyData.enemyID] = enemy.transform.position;
                data.enemyState[enemyData.enemyID] = enemyData.isAlive;
            }
        }

        foreach (PowerUpObject power in powerUps) {
            if (power.gameObject != null) {
                data.powerUpState[power.id] = power.isCollected;
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

        CombatData combatData = new CombatData();
        combatData.health = playerCont.currHealth;
        combatData.damage = playerCont.damage;
        json = JsonUtility.ToJson(combatData);
        if (File.Exists(combatFileName)) {
            File.Delete(combatFileName);
        }
        File.WriteAllText(combatFileName, json);
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
            playerCont.actionPoints = loadedMapData.playerCurrAP;
            Debug.Log(loadedMapData.playerCurrAP);
            playerCont.visibilityMultiplier = loadedMapData.playerVisibility;
            playerCont.isRepelAvailable = loadedMapData.isRepelAvailable;
            playerCont.isRevealAvailable = loadedMapData.isRevealAvailable;
            playerCont.isConvertAvailable = loadedMapData.isConvertAvailable;
            TurnController._instance.speedChangeTgl.isOn = loadedMapData.isFastForward;
            TurnController._instance.ChangeSpeed(loadedMapData.isFastForward);
            OverlayController._instance.areHotkeysDisplayed = loadedMapData.areHotkeysDisplayed;

            foreach (GameObject enemy in enemies) {
                EnemyMap enemyData = enemy.GetComponent<EnemyMap>();
                enemy.transform.position = loadedMapData.enemyPos[enemyData.enemyID];
                enemyData.isAlive = loadedMapData.enemyState[enemyData.enemyID];
                if (!enemyData.isAlive) {
                    Destroy(enemy);
                }
            }

            foreach (PowerUpObject power in powerUps) {
                if (power.gameObject != null) {
                    power.isCollected = loadedMapData.powerUpState[power.id];
                    if (power.isCollected) {
                        power.gameObject.SetActive(false);
                    }
                }
            }

            if (key != null) {
                KeyController keyData = key.GetComponent<KeyController>();
                keyData.isCollected = loadedMapData.keyState;
                if (keyData.isCollected) {
                    Destroy(key);
                }
            }
        }

        if (File.Exists(combatFileName)) {
            string jsonSave = File.ReadAllText(combatFileName);
            CombatData loadedCombatData = JsonUtility.FromJson<CombatData>(jsonSave);
            playerCont.currHealth = loadedCombatData.health;
            playerCont.damage = loadedCombatData.damage;
            playerCont.actionPoints = playerCont.actionPoints + loadedCombatData.apToAdd;
            Debug.Log(loadedCombatData.apToAdd);
        }
    }

    public void SaveCombatData() {
        CombatData data = new CombatData();
        PlayerCombatController playerCont = player.GetComponent<PlayerCombatController>();
        data.health = playerCont.health;
        data.damage = playerCont.damage;
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
        } else {
            UpgradeCostData upgradeCostData = new UpgradeCostData();
            upgradeCostData.upgradePoints = 5;
            upgradeCostData.healthCost = 1;
            upgradeCostData.damageCost = 1;
            upgradeCostData.visibilityCost = 1;
            upgradeCostData.apCost = 1;
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
        if (File.Exists(combatFileName)) {
            string jsonSave = File.ReadAllText(combatFileName);
            CombatData loadedCombatData = JsonUtility.FromJson<CombatData>(jsonSave);
            //if (player != null) {
            PlayerCombatController playerCont = player.GetComponent<PlayerCombatController>();

            playerCont.health = loadedCombatData.health;
            playerCont.damage = loadedCombatData.damage;
            //}
        }

        if (File.Exists(mapFileName)) {

            string jsonSave = File.ReadAllText(mapFileName);
			MapData loadedMapData = JsonUtility.FromJson<MapData> (jsonSave);
            PlayerCombatController playerCont = player.GetComponent<PlayerCombatController>();
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
        //level is done so delete any data that was kept in case of losing and having to revert upgrades
        string upgradeCostRevertFileName = Path.Combine(Application.persistentDataPath, "UpgradeCostRevertData.json");
        string totalUpgradeRevertFileName = Path.Combine(Application.persistentDataPath, "TotalUpgradeRevertData.json");

        if (File.Exists(upgradeCostRevertFileName)) {
            File.Delete(upgradeCostRevertFileName);
        }

        if (File.Exists(totalUpgradeRevertFileName)) {
            File.Delete(totalUpgradeRevertFileName);
        }

        LevelEndData LevelEndData = new LevelEndData();
        PlayerProgramController playerCont = player.GetComponent<PlayerProgramController>();
        LevelEndData.playerMaxHealth = playerCont.maxHealth;
        LevelEndData.playerAP = playerCont.maxActionPoints;
        LevelEndData.playerDamage = playerCont.damage;
        LevelEndData.playerVisibility = playerCont.visibilityMultiplier;
        LevelEndData.isRepelAvailable = playerCont.isRepelAvailable;
        LevelEndData.isRevealAvailable = playerCont.isRevealAvailable;
        LevelEndData.isConvertAvailable = playerCont.isConvertAvailable;

        //save if key has been collected in level
        if (key == null) {
            if (SceneManager.GetActiveScene().buildIndex == 2) {
                if (!key1Collected) {
                    key1Collected = true;
                    numKeys++;
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 3) {
                if (!key2Collected) {
                    key2Collected = true;
                    numKeys++;
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 4) {
                if (!key3Collected) {
                    key3Collected = true;
                    numKeys++;
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 5) {
                if (!key4Collected) {
                    key4Collected = true;
                    numKeys++;
                }
            }
        }

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
            playerCont.isRepelAvailable = loadedLevelEndData.isRepelAvailable;
            playerCont.isRevealAvailable = loadedLevelEndData.isRevealAvailable;
            playerCont.isConvertAvailable = loadedLevelEndData.isConvertAvailable;
        }
    }

    public void LoadCombatScene(GameObject details, int levelNo) {
        SaveMapData(details);
        currLevelNo = levelNo;
        SceneManager.LoadScene("Combat");
    }

    public void LoadMapScene() {
        SaveCombatData();
        SceneManager.LoadScene(currLevelNo);
    }

    public void CheckEnemiesAlive() {
        StartCoroutine(EnemiesCheck());
    }

    IEnumerator EnemiesCheck() {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("check");
        GameObject[] check = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log(check.Length);
        if (check.Length == 0) {
            LoadMapScene();
        }
    }
}

[Serializable]
class MapData {
    public Vector3 playerPos;
    public int playerAP;
    public int playerMaxHealth;
    public int playerMaxAP;
    public int playerCurrAP;
    public float playerVisibility;
    public bool isRepelAvailable;
    public bool isRevealAvailable;
    public bool isConvertAvailable;

    public Vector3[] enemyPos;
    public bool[] enemyState;

    public bool[] powerUpState;

	public bool keyState;
    public bool isFastForward;
    public bool areHotkeysDisplayed;

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
    public bool isRepelAvailable;
    public bool isRevealAvailable;
    public bool isConvertAvailable;
}