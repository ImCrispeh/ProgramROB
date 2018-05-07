using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeController : MonoBehaviour {
    public static UpgradeController _instance;
    private string upgradeFileName;
    private UpgradeData data;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
        data = new UpgradeData();
    }

    public void AddVisibility() {
        data.visibilityIncrease += 0.1f;
    }

    public void AddAP() {
        data.apIncrease += 5;
    }

    public void AddDamage() {
        data.damageIncrease++;
    }

    public void AddHealth() {
        data.healthIncrease += 3;
    }

    public void ResetUpgrades() {
        data.visibilityIncrease = 0;
        data.apIncrease = 0;
        data.damageIncrease = 0;
        data.healthIncrease = 0;
    }

    public void SaveUpgrades() {
        Debug.Log(data.visibilityIncrease);
        Debug.Log(data.apIncrease);
        Debug.Log(data.damageIncrease);
        Debug.Log(data.healthIncrease);
        string json = JsonUtility.ToJson(data);

        if (File.Exists(upgradeFileName)) {
            File.Delete(upgradeFileName);
        }

        File.WriteAllText(upgradeFileName, json);

        SceneManager.LoadScene("Level1");
    }
}

[Serializable]
class UpgradeData {
    public float visibilityIncrease;
    public int apIncrease;
    public int damageIncrease;
    public int healthIncrease;
}