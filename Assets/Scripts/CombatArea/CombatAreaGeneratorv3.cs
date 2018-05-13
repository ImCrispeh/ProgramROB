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

	//Usuable Prefabs
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject[] enemy;
	public GameObject player;

	//Gameobject holders
	//public Transform player;
	private Transform map;
	private Transform floor;
	private Transform walls;
	private Transform enemies;
	private List<Vector3> gridPosition = new List<Vector3> ();

	//What enemies to spawn
	public bool isEnemy;
	public bool isSpider;
	public bool isTurret;

	// Use this for initialization
	void Start () {
		InitHolders ();	//Keeps Hierarchy clean
		InitList ();	//Position to generate random layout
		GridGen ();		//Outerwall and floor
		RandomLayout (player, "player", 1, 1); //Random places player
		RandomLayout (wallTile, "wall", wallAmt.minimum, wallAmt.maximum);	//Random places wall tiles
		if (isEnemy) RandomLayout (enemy[0], "enemy", enemyAmt, enemyAmt);	//Random places enemies
		//if (isSpider) RandomLayout (enemy[1], "enemy", spiderAmt, spiderAmt);	//Random places spiders
		//if (isTurret) RandomLayout (enemy[2], "enemy", turretAmt, turretAmt);	//Random places turrets
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitList () {
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
			for (int y = -1; y < rows + 1; y++) {
				GameObject toGenerate = floorTile;
				if (x == -1 || x == columns || y == -1 || y == rows)
					toGenerate = wallTile;
				GameObject child = Instantiate (toGenerate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				child.transform.localScale = new Vector3 (6.25f, 6.25f, 1f);

				if (x == -1 || x == columns || y == -1 || y == rows)
					child.transform.SetParent (walls);
				else
					child.transform.SetParent (floor);
			}
		}
	}

	private Vector3 RandomPosition (string type) {
		bool isDistance = false;
		Vector3 pos = new Vector3 (0f, 0f, 0f);
		while (!isDistance) {
			int randomIndex = Random.Range (0, gridPosition.Count);
			pos = gridPosition [randomIndex];
			if (type != "player") {
				Transform playerPos = GameObject.FindGameObjectWithTag ("Player").transform;
				if ((Vector3.Distance (playerPos.position, pos) > 7 && type == "enemy") || (Vector3.Distance (playerPos.position, pos) > 1.5f && type == "wall")) {
					isDistance = true;
				}
			} else {
				isDistance = true;
			}
			if (isDistance)
				gridPosition.RemoveAt (randomIndex);
		}
		return pos;
	}

	private void RandomLayout (GameObject obj, string type, int min, int max) {
		int objAmt = Random.Range (min, max);

		for (int i = 0; i < objAmt; i++) {
			Vector3 randomPos = RandomPosition (type);
			GameObject child = Instantiate (obj, randomPos, Quaternion.identity);
			if (type == "wall") {
				child.transform.localScale = new Vector3 (6.25f, 6.25f, 1f);
				child.transform.SetParent (walls);
			} else if (type == "enemy") {
				child.transform.SetParent (enemies);
			}
		}
	}
}
