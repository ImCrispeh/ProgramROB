using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAreaGeneratorv1 : MonoBehaviour {

    //Combat Map max size
    public int width;
    public int height;
    //Floor Tile information
    public List<Vector3> genFloor;
    public List<Vector3> genWalls;
    public int tileAmount;
    public float tileSize;
    private GameObject child;
    //Movement possibility
    public float chanceUp;
    public float chanceRight;
    public float chanceLeft;
    //Enemy information
    public int spiderAmt;
    public int enemyAmt;
    public int turrentAmt;
    //Test parameter
    public float waitTime;
    public Vector3 testVec;

    // Use this for initialization
    void Start () {
		MapGen ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void MapGen()
    { /*IEnumerator MapGen () {*/
        int i = 0;
        while (i < tileAmount)
        {
            float chance = Random.Range(0f, 1f);

            i += FloorGen();
            PossibleMovement(chance);

            //yield return new WaitForSeconds (waitTime);
        }

        //StartCoroutine (WallGen ());
        WallGen();
        //yield return 0;
    }

    private void PossibleMovement(float randir)
    {

        if (randir < chanceUp)
        {
            NextTile(0);
        }
        else if (randir < chanceRight)
        {
            NextTile(1);
        }
        else if (randir < chanceLeft)
        {
            NextTile(2);
        }
        else
        {
            NextTile(3);
        }
    }

    private void NextTile(int dir)
    {
        switch (dir)
        {
            case 0:
                if (transform.position.y + tileSize < height / 2)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + tileSize, transform.position.z);
                }
                break;
            case 1:
                if (transform.position.x + tileSize < width / 2)
                {
                    transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, transform.position.z);
                }
                break;
            case 2:
                if (transform.position.y - tileSize > -(height / 2))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - tileSize, transform.position.z);
                }
                break;
            case 3:
                if (transform.position.x - tileSize > -(width / 2))
                {
                    transform.position = new Vector3(transform.position.x - tileSize, transform.position.y, transform.position.z);
                }
                break;
        }
    }

    private int FloorGen()
    {
        if (!genFloor.Contains(transform.position))
        {
            child = Instantiate(Resources.Load("Test_Floor_8") as GameObject, transform.position, transform.rotation);
            child.transform.localScale = new Vector3(6.25f, 6.25f, 1f);
            child.transform.parent = GameObject.Find("Floor").transform;
            genFloor.Add(child.transform.position);
            return 1;
        }

        return 0;
    }

    private void WallGen()
    {
        foreach (Vector3 floorTile in genFloor)
        {
            PossibleWall(floorTile);
        }

        //yield return 0;
    }

    private void PossibleWall(Vector3 floorTile)
    {
        if (!genFloor.Contains(new Vector3(floorTile.x - tileSize, floorTile.y + tileSize, floorTile.z)))
        { //top-left
            if (!genWalls.Contains(new Vector3(floorTile.x - tileSize, floorTile.y + tileSize, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x - tileSize, floorTile.y + tileSize, floorTile.z));
            }
        }
        if (!genFloor.Contains(new Vector3(floorTile.x, floorTile.y + tileSize, floorTile.z)))
        { //top
            if (!genWalls.Contains(new Vector3(floorTile.x, floorTile.y + tileSize, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x, floorTile.y + tileSize, floorTile.z));
            }
        }
        if (!genFloor.Contains(new Vector3(floorTile.x + tileSize, floorTile.y + tileSize, floorTile.z)))
        { //top-right
            if (!genWalls.Contains(new Vector3(floorTile.x + tileSize, floorTile.y + tileSize, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x + tileSize, floorTile.y + tileSize, floorTile.z));
            }
        }
        if (!genFloor.Contains(new Vector3(floorTile.x + tileSize, floorTile.y, floorTile.z)))
        { //right
            if (!genWalls.Contains(new Vector3(floorTile.x + tileSize, floorTile.y, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x + tileSize, floorTile.y, floorTile.z));
            }
        }
        if (!genFloor.Contains(new Vector3(floorTile.x + tileSize, floorTile.y - tileSize, floorTile.z)))
        { //bottom-right
            if (!genWalls.Contains(new Vector3(floorTile.x + tileSize, floorTile.y - tileSize, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x + tileSize, floorTile.y - tileSize, floorTile.z));
            }
        }
        if (!genFloor.Contains(new Vector3(floorTile.x, floorTile.y - tileSize, floorTile.z)))
        { //bottom
            if (!genWalls.Contains(new Vector3(floorTile.x, floorTile.y - tileSize, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x, floorTile.y - tileSize, floorTile.z));
            }
        }
        if (!genFloor.Contains(new Vector3(floorTile.x - tileSize, floorTile.y - tileSize, floorTile.z)))
        { //bottom-left
            if (!genWalls.Contains(new Vector3(floorTile.x - tileSize, floorTile.y - tileSize, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x - tileSize, floorTile.y - tileSize, floorTile.z));
            }
        }
        if (!genFloor.Contains(new Vector3(floorTile.x - tileSize, floorTile.y, floorTile.z)))
        { //left
            if (!genWalls.Contains(new Vector3(floorTile.x - tileSize, floorTile.y, floorTile.z)))
            {
                PlaceWall(new Vector3(floorTile.x - tileSize, floorTile.y, floorTile.z));
            }
        }
    }

    private void PlaceWall(Vector3 emptyTile)
    {
        child = Instantiate(Resources.Load("Test_Walls_3") as GameObject, emptyTile, transform.rotation);
        child.transform.localScale = new Vector3(6.25f, 6.25f, 1f);
        child.transform.parent = GameObject.Find("Walls").transform;
        genWalls.Add(child.transform.position);
    }
}
