using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadCombat : MonoBehaviour {

	private static bool created = false;

	private void Awake() {
		if (!created) {
			DontDestroyOnLoad (this.gameObject);
			created = true;
		} else {
			Destroy (this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (3);
		}
	}
}
