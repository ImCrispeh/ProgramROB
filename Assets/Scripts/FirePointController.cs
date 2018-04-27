using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointController : MonoBehaviour {

	private Transform playerPos;
	private PlayerProgramController player;

	// Use this for initialization
	void Start () {
		playerPos = transform.parent.transform;
		player = transform.parent.parent.GetComponent<PlayerProgramController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Positioning ();
	}

	private void Positioning () {
		Vector2 dir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - playerPos.position;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		playerPos.rotation = Quaternion.Slerp (playerPos.rotation, rotation, 1f);
	}

	private void OnTriggerStay2D (Collider2D other) {
		if (Input.GetButtonDown ("Fire1") && other.transform.tag == "Enemy") {
			other.gameObject.GetComponent<TestEnemyController> ().Damage (player.damage);
			Debug.Log ("Hit");
		}
	}
}
