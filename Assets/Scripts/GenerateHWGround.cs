using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHWGround : MonoBehaviour {

	private Transform ground;
	private GameObject child;
	public GameObject[] obj;

	// Use this for initialization
	void Start () {
		ground = GameObject.Find ("Ground").transform;

		for (int y = -19; y <= 19; y++) {
			for (int x = -19; x <= 19; x++) {
				child = Instantiate (obj[Random.Range (0, 2)], new Vector3 (x, y, 0f), Quaternion.identity);
				child.transform.localScale = new Vector3 (6.25f, 6.25f, 1f);
				child.transform.SetParent (ground);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
