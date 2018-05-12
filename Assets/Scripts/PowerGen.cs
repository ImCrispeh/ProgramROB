using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGen : MonoBehaviour {
	
	// Use this for initialization
	//public GameObject Generator;
	public Collider2D dos;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			GetComponent<BoxCollider2D> ().enabled = false;
			DarkRoomController._instance.SetRoomsLight (true);
			//Destroy (this);
		}
	}

}


