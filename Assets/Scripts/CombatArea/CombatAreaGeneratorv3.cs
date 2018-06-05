using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAreaGeneratorv3 : MonoBehaviour {

	[System.Serializable]
	public class Amount {
		public int minimum;
		public int maximum;

		public Amount (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	//Map Size
	public int columns = 8;
	public int rows = 8;

	//Amount ranges for random layout
	public Amount wallAmt = new Amount (5, 9);
	public int enemyAmt;
	public int spiderAmt;
	public int turretAmt;
    public int tankAmt;

	//Usuable Prefabs
	public GameObject floorTile;
	public GameObject[] wallTile;
	public GameObject obstacleTile;
	public GameObject[] enemy;
	public GameObject player;

	//Gameobject holders
	//public Transform player;
	private Transform map;
	private Transform floor;
	private Transform walls;
	private Transform enemies;
	private Transform playerPos;
	private List<Vector3> enemyPosition = new List<Vector3> ();
	private List<Vector3> gridPosition = new List<Vector3> ();

	//What enemies to spawn
	public bool isEnemy;
	public bool isSpider;
	public bool isTurret;
    public bool isTank;

    // Use this for initialization
    void Start () {
		InitHolders ();	//Keeps Hierarchy clean
		InitList ();	//Position to generate random layout
		GridGen ();		//Outerwall and floor
		RandomLayout (player, "player", 1, 1); //Random places player
        MapStateController._instance.player = GameObject.FindGameObjectWithTag("Player");
        MapStateController._instance.LoadCombatData();
        if (isEnemy) RandomLayout (enemy[0], "enemy", enemyAmt, enemyAmt);	//Random places enemies
		if (isSpider) RandomLayout (enemy[1], "enemy", spiderAmt, spiderAmt);	//Random places spiders
		if (isTurret) RandomLayout (enemy[2], "enemy", turretAmt, turretAmt);	//Random places turrets
        if (isTank) RandomLayout(enemy[3], "enemy", tankAmt, tankAmt);
		map.transform.position = new Vector3(map.transform.position.x, map.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z + 1f);
		RandomLayout (obstacleTile, "wall", wallAmt.minimum, wallAmt.maximum);	//Random places wall tiles
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitList () {
		enemyPosition.Clear ();
		gridPosition.Clear ();
		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPosition.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	private void InitHolders () {
		map = new GameObject ("Map").transform;
		map.transform.SetParent (transform);
		floor = new GameObject ("Floor").transform;
		floor.transform.SetParent (map);
		walls = new GameObject ("Walls").transform;
		walls.transform.SetParent (map);
		enemies = new GameObject ("Enemies").transform;
		enemies.transform.SetParent (transform);
	}

	private void GridGen () {
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -2; y < rows + 2; y++) {
				GameObject toGenerate = floorTile;
				if (x == -1 || x == columns || y <= -1 || y >= rows)
					toGenerate = WallGen (x, y);
				GameObject child = Instantiate (toGenerate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				if (x == -1 || x == columns || y <= -1 || y >= rows)
					child.transform.SetParent (walls);
				else
					child.transform.SetParent (floor);
				if ((y == -1 || y == rows) && !(x == -1 || x == columns)) {
					child = Instantiate (floorTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					child.transform.SetParent (floor);
				}
			}
		}
	}

	private GameObject WallGen (int x, int y) {
		if (x == -1 && y == rows + 1)
			return wallTile [0];
		if (x == columns && y == rows + 1)
			return wallTile [1];

		if (x == -1 && y == rows)
			return wallTile [2];
		if (x == columns && y == rows)
			return wallTile [3];

		if (x == -1 && y == -1)
			return wallTile [5];
		if (x == columns && y == -1)
			return wallTile [6];

		if (x == -1 && y == -2)
			return wallTile [7];
		if (x == columns && y == -2)
			return wallTile [9];

		if (y == -1 || y == rows + 1)
			return wallTile [10];
		if (y == rows)
			return wallTile [11];
		if (y == -2)
			return wallTile [8];
		if (x == -1 || x == columns)
			return wallTile [4];

		return null;
	}

	private Vector3 RandomPosition (string type) {
		bool isDistance = false;
		Vector3 pos = new Vector3 (0f, 0f, 0f);
		while (!isDistance) {
			int randomIndex = Random.Range (0, gridPosition.Count);
			pos = gridPosition [randomIndex];

			isDistance = CheckDistance (type, pos);

			if (isDistance)
				gridPosition.RemoveAt (randomIndex);
		}
		return pos;
	}

	private bool CheckDistance (string type, Vector3 pos) {
		if (type != "player") {
			//Transform playerPos = GameObject.FindGameObjectWithTag ("Player").transform;
			if (type == "enemy") { //Enemy check
				if (Vector3.Distance (playerPos.position, pos) > 7) {
					return true;
				} else {
					return false;
				}
			}
			if (type == "wall") { //Wall check
				foreach (Vector3 enemyPos in enemyPosition) {
					Debug.Log ("Enemy distance: " + Vector3.Distance (enemyPos, pos));
					Debug.Log ("Player distance: " + Vector3.Distance (playerPos.position, pos));
					if (Vector3.Distance (enemyPos, pos) < 1.5f || Vector3.Distance (playerPos.position, pos) < 1.5f) {
						return false;
					}
				}
				return true;
			}
		} else { //For Player
			return true;
		}
		return false;
	}

	private void RandomLayout (GameObject obj, string type, int min, int max) {
		int objAmt = Random.Range (min, max);

		for (int i = 0; i < objAmt; i++) {
			Vector3 randomPos = RandomPosition (type);
			GameObject child = Instantiate (obj, randomPos, Quaternion.identity);
			if (type == "wall") {
				child.transform.SetParent (walls);
			} else if (type == "enemy") {
				child.transform.SetParent (enemies);
				enemyPosition.Add (child.transform.position);
			} else if (type == "player") {
				playerPos = child.transform;
			}
		}
	}
}
