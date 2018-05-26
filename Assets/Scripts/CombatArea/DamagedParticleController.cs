using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedParticleController : MonoBehaviour {

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, 1);
    }
}
