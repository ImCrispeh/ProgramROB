using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomController : MonoBehaviour {
    public static DarkRoomController _instance;
    public Transform darkRoomEffect;
    public Transform player;
    private bool isMoving = false;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        darkRoomEffect.position = new Vector3(player.position.x, player.position.y, darkRoomEffect.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
            darkRoomEffect.position = new Vector3(player.position.x, player.position.y, darkRoomEffect.position.z);
        }
	}

    public void ToggleEffect(bool toggle) {
        darkRoomEffect.gameObject.SetActive(toggle);
    }

    public void SetMoving(bool moving) {
        isMoving = moving;
    }
}
