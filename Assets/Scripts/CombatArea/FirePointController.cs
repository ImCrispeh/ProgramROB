using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointController : MonoBehaviour {

	private Transform playerPos;
	private PlayerCombatController player;
    public GameObject particle;
	// Use this for initialization
	void Start () {
		playerPos = transform.parent.transform;
		player = transform.parent.parent.GetComponent<PlayerCombatController> ();
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

}
