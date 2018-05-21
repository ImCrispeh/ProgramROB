using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DataCollectionController : MonoBehaviour {
    public static DataCollectionController _instance;
    public string filePath;
    public string directory;
    private DataCollector data;

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
        directory = Application.dataPath + @"\SessionData";
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        int i = 0;
        while (File.Exists(directory + @"\session" + i + ".txt")) {
            i++;
        }
        filePath = directory + @"\session" + i + ".txt";
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("playtime_(s),ranged_attacks,melee_attacks,melee_damage,ranged_damage,turret_damage,tankspawn_damage,movement_used,health_upgrades,damage_upgrade,visibility_upgrade,ap_upgrade");
        writer.Close();
        data = new DataCollector();
    }
	
	// Update is called once per frame
	void Update () {
        data.playtime += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.U)) {
            WriteToFile();
        }
	}

    public void UpdateRangedAttacks() {
        data.rangedAttacks++;
    }

    public void UpdateMeleeAttacks() {
        data.meleeAttacks++;
    }

    public void UpdateMeleeDamage(int damage) {
        data.meleeDamage += damage;
    }

    public void UpdateRangedDamage(int damage) {
        data.rangedDamage += damage;
    }

    public void UpdateTurretDamage(int damage) {
        data.turretDamage += damage;
    }

    public void UpdateTankSpawnDamage(int damage) {
        data.tankSpawnDamage += damage;
    }

    public void UpdateMovementUsed() {
        data.movementUsed++;
    }

    public void UpdateHealthUpgrade() {
        data.healthUpgrade++;
    }

    public void UpdateDamageUpgrade() {
        data.damageUpgrade++;
    }

    public void UpdateVisibilityUpgrade() {
        data.visibilityUpgrade++;
    }

    public void UpdateApUpgrade() {
        data.apUpgrade++;
    }

    public void UpdateIsWin(bool didWin) {
        data.isWin = didWin;
    }

    public void WriteToFile() {
        //if (!Application.isEditor) {
        FileStream appendToFile = File.Open(filePath, FileMode.Append);
        StreamWriter writer = new StreamWriter(appendToFile);
        data.level = 1; //will change this later with more levels
        string output = "\n" + data.playtime.ToString("F2");
        output += "," + data.level;
        output += "," + data.rangedAttacks;
        output += "," + data.meleeAttacks;
        output += "," + data.meleeDamage;
        output += "," + data.rangedDamage;
        output += "," + data.turretDamage;
        output += "," + data.tankSpawnDamage;
        output += "," + data.movementUsed;
        output += "," + data.healthUpgrade;
        output += "," + data.damageUpgrade;
        output += "," + data.visibilityUpgrade;
        output += "," + data.apUpgrade;
        output += "," + data.isWin;
        writer.WriteLine(output);
        writer.Close();
        data = new DataCollector();
        //}
    }
}

class DataCollector {
    public float playtime;
    public int level;
    public int rangedAttacks;
    public int meleeAttacks;
    public int meleeDamage;
    public int rangedDamage;
    public int turretDamage;
    public int tankSpawnDamage;
    public int movementUsed;
    public int healthUpgrade;
    public int damageUpgrade;
    public int visibilityUpgrade;
    public int apUpgrade;
    public bool isWin;
}