using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangCameraPos : MonoBehaviour {
	public GameObject camera;
	new Vector3 cameraPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			GetComponent<BoxCollider2D> ().enabled = false;
			DarkRoomController._instance.SetRoomsLight (false);
			camera.transform.position = new Vector3 (15, 9, -10);
			//Destroy (this);
		}
	}

}
