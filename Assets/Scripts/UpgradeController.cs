using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeController : MonoBehaviour {
    public static UpgradeController _instance;
    private string upgradeFileName;
    private string upgradeCostFileName;
    private string totalUpgradeFileName;
    private UpgradeData upgradeData;
    private UpgradeCostData upgradeCostData;
    private UpgradeData totalUpgradeData;

    public Text healthText;
    public Text apText;
    public Text damageText;
    public Text visibilityText;
    public Text upgradePointsText;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start() {
        upgradeFileName = Path.Combine(Application.persistentDataPath, "UpgradeSaveData.json");
        upgradeCostFileName = Path.Combine(Application.persistentDataPath, "UpgradeCostSaveData.json");
        totalUpgradeFileName = Path.Combine(Application.persistentDataPath, "TotalUpgradeSaveData.json");
        upgradeData = new UpgradeData();

        SetCosts();
        SetTotals();
        UpdateHealthText();
        UpdateAPText();
        UpdateDamageText();
        UpdateVisibilityText();
        UpdateUpgradePointsText();
    }

    public void AddVisibility() {
        if (upgradeCostData.upgradePoints >= upgradeCostData.visibilityCost) {
            upgradeData.visibilityIncrease += 0.1f;
            totalUpgradeData.visibilityIncrease += 0.1f;
            upgradeCostData.upgradePoints -= upgradeCostData.visibilityCost;
            upgradeCostData.visibilityCost++;
            UpdateVisibilityText();
            UpdateUpgradePointsText();
        }
    }

    public void AddAP() {
        if (upgradeCostData.upgradePoints >= upgradeCostData.apCost) {
            upgradeData.apIncrease += 5;
            totalUpgradeData.apIncrease += 5;
            upgradeCostData.upgradePoints -= upgradeCostData.apCost;
            upgradeCostData.apCost++;
            UpdateAPText();
            UpdateUpgradePointsText();
        }
    }

    public void AddDamage() {
        if (upgradeCostData.upgradePoints >= upgradeCostData.damageCost) {
            upgradeData.damageIncrease++;
            totalUpgradeData.damageIncrease++;
            upgradeCostData.upgradePoints -= upgradeCostData.damageCost;
            upgradeCostData.damageCost++;
            UpdateDamageText();
            UpdateUpgradePointsText();
        }
    }

    public void AddHealth() {
        if (upgradeCostData.upgradePoints >= upgradeCostData.healthCost) {
            upgradeData.healthIncrease += 3;
            totalUpgradeData.healthIncrease += 3;
            upgradeCostData.upgradePoints -= upgradeCostData.healthCost;
            upgradeCostData.healthCost++;
            UpdateHealthText();
            UpdateUpgradePointsText();
        }
    }

    public void ResetUpgrades() {
        upgradeData = new UpgradeData();
        SetCosts();
        SetTotals();
        UpdateHealthText();
        UpdateAPText();
        UpdateDamageText();
        UpdateVisibilityText();
        UpdateUpgradePointsText();
    }

    public void SaveUpgrades() {
        string json = JsonUtility.ToJson(upgradeData);

        if (File.Exists(upgradeFileName)) {
            File.Delete(upgradeFileName);
        }

        File.WriteAllText(upgradeFileName, json);

        json = JsonUtility.ToJson(totalUpgradeData);
        File.WriteAllText(totalUpgradeFileName, json);

        DataCollectionController._instance.UpdateHealthUpgrade(totalUpgradeData.healthIncrease / 3);
        DataCollectionController._instance.UpdateDamageUpgrade(totalUpgradeData.damageIncrease);
        DataCollectionController._instance.UpdateVisibilityUpgrade((int)(totalUpgradeData.visibilityIncrease * 10));
        DataCollectionController._instance.UpdateApUpgrade(totalUpgradeData.apIncrease / 5);

        json = JsonUtility.ToJson(upgradeCostData);
        File.WriteAllText(upgradeCostFileName, json);

        SceneManager.LoadScene(1);
    }

    public void SetCosts() {
        if (File.Exists(upgradeCostFileName)) {
            string jsonSave = File.ReadAllText(upgradeCostFileName);
            upgradeCostData = JsonUtility.FromJson<UpgradeCostData>(jsonSave);
        } else {
            upgradeCostData = new UpgradeCostData();
            upgradeCostData.upgradePoints = 10;
            upgradeCostData.healthCost = 1;
            upgradeCostData.damageCost = 1;
            upgradeCostData.visibilityCost = 1;
            upgradeCostData.apCost = 1;
        }
    }

    public void SetTotals() {
        if (File.Exists(totalUpgradeFileName)) {
            string jsonSave = File.ReadAllText(totalUpgradeFileName);
            totalUpgradeData = JsonUtility.FromJson<UpgradeData>(jsonSave);
        } else {
            totalUpgradeData = new UpgradeData();
        }
    }

    public void UpdateHealthText() {
        healthText.text = "Total Health Increase: " + totalUpgradeData.healthIncrease + "\n"
                        + "Cost: " + upgradeCostData.healthCost;
    }

    public void UpdateAPText() {
        apText.text = "Total AP Increase: " + totalUpgradeData.apIncrease + "\n"
                        + "Cost: " + upgradeCostData.apCost;
    }

    public void UpdateDamageText() {
        damageText.text = "Total Damage Increase: " + totalUpgradeData.damageIncrease + "\n"
                        + "Cost: " + upgradeCostData.damageCost;
    }

    public void UpdateVisibilityText() {
        visibilityText.text = "Total Visibility Increase: " + (int)(totalUpgradeData.visibilityIncrease * 100) + "% \n"
                        + "Cost: " + upgradeCostData.visibilityCost;
    }

    public void UpdateUpgradePointsText() {
        upgradePointsText.text = "Upgrade Points Remaining: " + upgradeCostData.upgradePoints;
    }
}

[Serializable]
class UpgradeData {
    public float visibilityIncrease;
    public int apIncrease;
    public int damageIncrease;
    public int healthIncrease;
}

[Serializable]
class UpgradeCostData {
    public int upgradePoints;
    public int visibilityCost;
    public int apCost;
    public int damageCost;
    public int healthCost;
}