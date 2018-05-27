using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController _instance;
    public Camera mainCam;
    public Transform player;
    public float horizontalResolution = 1920;
    public bool isMapRevealed;
    public Light playerLight;
    public Light mapLight;


    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
        float currentAspect = (float)Screen.width / (float)Screen.height;
        Camera.main.orthographicSize = horizontalResolution / currentAspect / 160;
    }

    void OnGUI() {
        if (!isMapRevealed) {
            float currentAspect = (float)Screen.width / (float)Screen.height;
            Camera.main.orthographicSize = horizontalResolution / currentAspect / 160;
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

    }
    
    public void LightMap() {
        isMapRevealed = true;
        mapLight.enabled = true;
        playerLight.enabled = false;
    }

    public void DimMap() {
        mapLight.enabled = false;
        playerLight.enabled = true;
        isMapRevealed = false;
    }

    public void RevealMapWrapper() {
        StartCoroutine(RevealMap());
    }

    IEnumerator RevealMap() {
        isMapRevealed = true;
        mapLight.enabled = true;
        playerLight.enabled = false;
        yield return new WaitForSeconds(5f);
        mapLight.enabled = false;
        playerLight.enabled = true;
        isMapRevealed = false;
    }
}
