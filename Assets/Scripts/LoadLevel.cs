using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	public int sceneIndex;
	public bool isInRange;

	// Use this for initialization
	void Start () {
		isInRange = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isInRange) {
			if (Input.GetKeyDown (KeyCode.E)) {
				SceneManager.LoadScene (sceneIndex);
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D other) {
		isInRange = true;
	}

	private void OnTriggerExit2D (Collider2D other) {
		isInRange = false;
	}
}
