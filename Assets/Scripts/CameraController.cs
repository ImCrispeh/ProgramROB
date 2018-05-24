using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController _instance;
    public Camera mainCam;
    public Vector3 pos1;
    public Vector3 pos2;
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
        Camera.main.orthographicSize = horizontalResolution / currentAspect / 180;
    }

    void OnGUI() {
        if (!isMapRevealed) {
            float currentAspect = (float)Screen.width / (float)Screen.height;
            Camera.main.orthographicSize = horizontalResolution / currentAspect / 180;
        }
    }

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
        if (!isMapRevealed) {
            if (player.position.y < 4.5f) {
                mainCam.transform.position = pos1;
            } else {
                mainCam.transform.position = pos2;
            }
        }
    }
    
    public void RevealMapWrapper() {
        StartCoroutine(RevealMap());
    }

    IEnumerator RevealMap() {
        isMapRevealed = true;
        mainCam.transform.position = (pos1 + pos2) / 2;
        mainCam.orthographicSize *= 2;
        mapLight.enabled = true;
        playerLight.enabled = false;
        yield return new WaitForSeconds(5f);
        mainCam.orthographicSize /= 2;
        mapLight.enabled = false;
        playerLight.enabled = true;
        isMapRevealed = false;
    }
}
