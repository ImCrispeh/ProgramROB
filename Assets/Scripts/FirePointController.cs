using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointController : MonoBehaviour {

	private Transform player;

	// Use this for initialization
	void Start () {
		player = transform.parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 playerPos = player.position;
		Vector2 dir = mousePos - playerPos;
		transform.position = dir * 1f + playerPos;*/
		Vector2 dir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - player.position;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		player.rotation = Quaternion.Slerp (player.rotation, rotation, 1f);
	}
}
