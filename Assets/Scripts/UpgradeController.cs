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
    private string upgradeCostRevertFileName;
    private string totalUpgradeRevertFileName;
    private UpgradeData upgradeData;
    private UpgradeCostData upgradeCostData;
    private UpgradeData totalUpgradeData;

    public Text healthText;
    public Text apText;
    public Text damageText;
    public Text visibilityText;
    public Text repelText;
    public Text revealText;
    public Text convertText;
    public Text upgradePointsText;

    public Button repelBtn;
    public Button revealBtn;
    public Button convertBtn;

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
        upgradeCostRevertFileName = Path.Combine(Application.persistentDataPath, "UpgradeCostRevertData.json");
        totalUpgradeRevertFileName = Path.Combine(Application.persistentDataPath, "TotalUpgradeRevertData.json");
        upgradeData = new UpgradeData();

        SetCosts();
        SetTotals();
        UpdateHealthText();
        UpdateAPText();
        UpdateDamageText();
        UpdateVisibilityText();
        UpdateRepelText();
        UpdateRevealText();
        UpdateConvertText();
        UpdateUpgradePointsText();
    }

    public void AddVisibility() {
        if (upgradeCostData.upgradePoints >= upgradeCostData.visibilityCost) {
            upgradeData.visibilityIncrease += 0.125f;
            totalUpgradeData.visibilityIncrease += 0.125f;
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

    public void PurchaseRepel() {
        if (upgradeCostData.upgradePoints >= 5) {
            upgradeCostData.upgradePoints -= 5;
            upgradeData.isRepelPurchased = true;
            totalUpgradeData.isRepelPurchased = true;
            UpdateRepelText();
            UpdateUpgradePointsText();
        }
    }

    public void PurchaseReveal() {
        if (upgradeCostData.upgradePoints >= 5) {
            upgradeCostData.upgradePoints -= 5;
            upgradeData.isRevealPurchased = true;
            totalUpgradeData.isRevealPurchased = true;
            UpdateRevealText();
            UpdateUpgradePointsText();
        }
    }

    public void PurchaseConvert() {
        if (upgradeCostData.upgradePoints >= 5) {
            upgradeCostData.upgradePoints -= 5;
            upgradeData.isConvertPurchased = true;
            totalUpgradeData.isConvertPurchased = true;
            UpdateConvertText();
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
        UpdateRepelText();
        UpdateRevealText();
        UpdateConvertText();
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
        DataCollectionController._instance.UpdateVisibilityUpgrade((int)(totalUpgradeData.visibilityIncrease * 10 / 1.25f));
        DataCollectionController._instance.UpdateApUpgrade(totalUpgradeData.apIncrease / 5);

        json = JsonUtility.ToJson(upgradeCostData);
        File.WriteAllText(upgradeCostFileName, json);

        SceneManager.LoadScene("UpgradeArea");
    }

    public void SetCosts() {
        if (File.Exists(upgradeCostRevertFileName)) {
            string jsonSave = File.ReadAllText(upgradeCostRevertFileName);
            upgradeCostData = JsonUtility.FromJson<UpgradeCostData>(jsonSave);
        } else if (File.Exists(upgradeCostFileName)) {
            string jsonSave = File.ReadAllText(upgradeCostFileName);
            upgradeCostData = JsonUtility.FromJson<UpgradeCostData>(jsonSave);
        } else {
            upgradeCostData = new UpgradeCostData();
            upgradeCostData.upgradePoints = 5;
            upgradeCostData.healthCost = 1;
            upgradeCostData.damageCost = 1;
            upgradeCostData.visibilityCost = 1;
            upgradeCostData.apCost = 1;
        }
        string json = JsonUtility.ToJson(upgradeCostData);
        File.WriteAllText(upgradeCostRevertFileName, json);
        
    }

    public void SetTotals() {
        if (File.Exists(totalUpgradeRevertFileName)) {
            string jsonSave = File.ReadAllText(totalUpgradeRevertFileName);
            totalUpgradeData = JsonUtility.FromJson<UpgradeData>(jsonSave);
            upgradeData.isRepelPurchased = totalUpgradeData.isRepelPurchased;
            upgradeData.isRevealPurchased = totalUpgradeData.isRevealPurchased;
            upgradeData.isConvertPurchased = totalUpgradeData.isConvertPurchased;
        } else if (File.Exists(totalUpgradeFileName)) {
            string jsonSave = File.ReadAllText(totalUpgradeFileName);
            totalUpgradeData = JsonUtility.FromJson<UpgradeData>(jsonSave);
            upgradeData.isRepelPurchased = totalUpgradeData.isRepelPurchased;
            upgradeData.isRevealPurchased = totalUpgradeData.isRevealPurchased;
            upgradeData.isConvertPurchased = totalUpgradeData.isConvertPurchased;
        } else {
            totalUpgradeData = new UpgradeData();
        }

        string json = JsonUtility.ToJson(totalUpgradeData);
        File.WriteAllText(totalUpgradeRevertFileName, json);
    }

    public void UpdateHealthText() {
        healthText.text = "Total Health Increase: " + totalUpgradeData.healthIncrease + "\n" + "Cost: " + upgradeCostData.healthCost;
    }

    public void UpdateAPText() {
        apText.text = "Total AP Increase: " + totalUpgradeData.apIncrease + "\n" + "Cost: " + upgradeCostData.apCost;
    }

    public void UpdateDamageText() {
        damageText.text = "Total Damage Increase: " + totalUpgradeData.damageIncrease + "\n" + "Cost: " + upgradeCostData.damageCost;
    }

    public void UpdateVisibilityText() {
        visibilityText.text = "Total Visibility Increase: " + (totalUpgradeData.visibilityIncrease * 100) + "% \n" + "Cost: " + upgradeCostData.visibilityCost;
    }

    public void UpdateRepelText() {
        if (upgradeData.isRepelPurchased) {
            repelText.text = "Uses 3 AP" + "\n" + "Cost: N/A";
        } else {
            repelText.text = "Uses 3 AP" + "\n" + "Cost: 5";
        }
        repelBtn.interactable = !upgradeData.isRepelPurchased;
    }

    public void UpdateRevealText() {
        if (upgradeData.isRevealPurchased) {
            revealText.text = "Uses 5 AP" + "\n" + "Cost: N/A";
        } else {
            revealText.text = "Uses 5 AP" + "\n" + "Cost: 5";
        }
        revealBtn.interactable = !upgradeData.isRevealPurchased;
    }

    public void UpdateConvertText() {
        if (upgradeData.isConvertPurchased) {
            convertText.text = "Drains 1 AP per turn" + "\n" + "Cost: N/A";
        } else {
            convertText.text = "Drains 1 AP per turn" + "\n" + "Cost: 5";
        }
        convertBtn.interactable = !upgradeData.isConvertPurchased;
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
    public bool isRepelPurchased;
    public bool isRevealPurchased;
    public bool isConvertPurchased;
}

[Serializable]
class UpgradeCostData {
    public int upgradePoints;
    public int visibilityCost;
    public int apCost;
    public int damageCost;
    public int healthCost;
}