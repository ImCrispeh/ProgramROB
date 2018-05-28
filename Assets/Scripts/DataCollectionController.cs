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
        //this makes it so it will only capture data in the built game (leave this commented for testing)
        //if (!Application.isEditor) {
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
            writer.WriteLine("playtime_(s),level,ranged_attacks,melee_attacks,melee_damage,ranged_damage,turret_damage,tankspawn_damage,movement_used,repel_used,reveal_used,convert_used,health_upgrades,damage_upgrade,visibility_upgrade,ap_upgrade,did_win");
            writer.Close();
        //}
        data = new DataCollector();
    }
	
	// Update is called once per frame
	void Update () {
        data.playtime += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.U)) {
            MapStateController._instance.EndGame(true, "");
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

    public void UpdateRepelUsed() {
        data.repelUsed++;
    }

    public void UpdateRevealUsed() {
        data.revealUsed++;
    }

    public void UpdateConvertUsed() {
        data.convertUsed++;
    }

    public void UpdateHealthUpgrade(int amount) {
        data.healthUpgrade += amount;
    }

    public void UpdateDamageUpgrade(int amount) {
        data.damageUpgrade += amount;
    }

    public void UpdateVisibilityUpgrade(int amount) {
        data.visibilityUpgrade += amount;
    }

    public void UpdateApUpgrade(int amount) {
        data.apUpgrade += amount;
    }

    public void UpdateIsWin(bool didWin) {
        data.isWin = didWin;
    }

    private void OnApplicationQuit() {
        WriteToFile();
    }

    public void WriteToFile() {
        //if (!Application.isEditor) {
            FileStream appendToFile = File.Open(filePath, FileMode.Append);
            StreamWriter writer = new StreamWriter(appendToFile);
            data.level = SceneManager.GetActiveScene().name;
            string output = "\n" + data.playtime.ToString("F2");
            output += "," + data.level;
            output += "," + data.rangedAttacks;
            output += "," + data.meleeAttacks;
            output += "," + data.meleeDamage;
            output += "," + data.rangedDamage;
            output += "," + data.turretDamage;
            output += "," + data.tankSpawnDamage;
            output += "," + data.movementUsed;
            output += "," + data.repelUsed;
            output += "," + data.revealUsed;
            output += "," + data.convertUsed;
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
    public string level;
    public int rangedAttacks;
    public int meleeAttacks;
    public int meleeDamage;
    public int rangedDamage;
    public int turretDamage;
    public int tankSpawnDamage;
    public int movementUsed;
    public int repelUsed;
    public int revealUsed;
    public int convertUsed;
    public int healthUpgrade;
    public int damageUpgrade;
    public int visibilityUpgrade;
    public int apUpgrade;
    public bool isWin;
}