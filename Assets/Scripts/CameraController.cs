﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Camera mainCam;
    public Vector3 pos1;
    public Vector3 pos2;
    public Transform player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCam = Camera.main;
        if (player.position.y < 4.5f) {
            mainCam.transform.position = pos1;
        } else {
            mainCam.transform.position = pos2;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (player.position.y < 4.5f) {
            mainCam.transform.position = pos1;
        } else {
            mainCam.transform.position = pos2;
        }
    }
}
