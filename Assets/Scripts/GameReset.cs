using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour {
    public static GameReset _instance;

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

    // Use this for initialization
    void Start () {
        upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
        mapFileName = Path.Combine(Application.persistentDataPath, "MapSaveData.json");
        combatFileName = Path.Combine(Application.persistentDataPath, "CombatSaveData.json");
        levelEndFileName = Path.Combine(Application.persistentDataPath, "LevelEndSaveData.json");
        upgradeCostFileName = Path.Combine(Application.persistentDataPath, "UpgradeCostSaveData.json");
        totalUpgradeFileName = Path.Combine(Application.persistentDataPath, "TotalUpgradeSaveData.json");
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
            SceneManager.LoadScene("UpgradeScreen");
        }

        if (Input.GetKeyDown(KeyCode.R)) {
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
            SceneManager.LoadScene("UpgradeScreen");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
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
            Application.Quit();
        }
    }
}
