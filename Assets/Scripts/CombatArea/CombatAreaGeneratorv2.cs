using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAreaGeneratorv2 : MonoBehaviour {

	//Map Parameters
	public int width;
	public int height;
	public int[,] mapGrid;
	private int cx;
	private int cy;
	//Tile Parameters
	public int i;
	public int tileAmt;
	public float tileSize;
	private GameObject child;
	private GameObject floor;
	private GameObject wall;
	//Random Movement parameters
	public float chanceUp;
	public float chanceRight;
	public float chanceLeft;

	// Use this for initialization
	void Start () {
		mapGrid = new int[width, height];
		cx = width - 1;
		cy = height - 1;
		floor = Resources.Load ("Test_Floor_8") as GameObject;
		wall = Resources.Load ("Test_Walls_3") as GameObject;

		MapGen ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void MapGen () {
		StartCoroutine (FloorGen ());
		//WallGen ();
	}

	IEnumerator FloorGen () {
		i = 0;
		while (i < tileAmt) {
			if (mapGrid [(int)transform.position.x, (int)transform.position.y] != 1) {
				mapGrid [(int)transform.position.x, (int)transform.position.y] = 1;
				TilePlacement (floor, "Floor", transform.position.x, transform.position.y);
				i++;
			}

			RandomMovement ();

			yield return new WaitForSeconds (0f);
		}

		StartCoroutine (WallGen ());
		yield return 0;
	}

	private void RandomMovement () {
		/*float chance = Random.Range (0f, 1f);
		Debug.Log (chance);
		if (chance < chanceUp) {
			if (transform.position.y + 1 < cy) {
				transform.position += Vector3.up;
			}
		} else if (chance < chanceRight) {
			if (transform.position.x + 1 < cx) {
				transform.position += Vector3.right;
			}
		} else if (chance < chanceLeft) {
			if (transform.position.x - 1 > 0) {
				transform.position += Vector3.left;
			}
		} else {
			if (transform.position.y - 1 < 0) {
				transform.position += Vector3.down;
			}
		}*/
		int chance = Random.Range (0, 3);

		if (chance == 0) {
			if (transform.position.y + 1 < cy) {
				transform.position += Vector3.up;
			}
		} else if (chance == 1) {
			if (transform.position.x + 1 < cx) {
				transform.position += Vector3.right;
			}
		} else if (chance == 2) {
			if (transform.position.x - 1 > 0) {
				transform.position += Vector3.left;
			}
		} else {
			if (transform.position.y - 1 < 0) {
				transform.position += Vector3.down;
			}
		}
	}

	//private void WallGen () {
	IEnumerator WallGen () {
		for (int i = 1; i < cy; i++) {
			for (int j = 1; j < cx; j++) {
				if (mapGrid [j, i] == 1) {
					SpaceCheck (j, i);
					yield return new WaitForSeconds (0f);
				}
			}
		}

		yield return 0;
	}

	private void SpaceCheck (int x, int y) {
		if (mapGrid [x - 1, y + 1] == 0) TilePlacement (wall, "Walls", x - 1, y + 1); //top-left
		if (mapGrid [x, y + 1] == 0) TilePlacement (wall, "Walls", x, y + 1); //top
		if (mapGrid [x + 1, y + 1] == 0) TilePlacement (wall, "Walls", x + 1, y + 1); //top-right
		if (mapGrid [x + 1, y] == 0) TilePlacement (wall, "Walls", x + 1, y); // right
		if (mapGrid [x + 1, y - 1] == 0) TilePlacement (wall, "Walls", x + 1, y - 1); //bottom-right
		if (mapGrid [x, y - 1] == 0) TilePlacement (wall, "Walls", x, y - 1); //bottom
		if (mapGrid [x - 1, y - 1] == 0) TilePlacement (wall, "Walls", x - 1, y - 1); //bottom-left
		if (mapGrid [x - 1, y] == 0) TilePlacement (wall, "Walls", x - 1, y); //left
	}

	private void TilePlacement (GameObject tile, string parent, float x, float y) {
		child = Instantiate (tile, new Vector3 (x * tileSize, y * tileSize, 0f), transform.rotation);
		child.transform.localScale = new Vector3 (4, 4, 1);
		child.transform.parent = GameObject.Find (parent).transform;
	}
}