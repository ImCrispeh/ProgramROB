using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour {
    public static BGMController _instance;
    public AudioClip[] bgm;
    public AudioSource audioSrc;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Title" || scene.name == "Hub") {
            audioSrc.clip = bgm[0];
        } else if (scene.buildIndex >= 1 && scene.buildIndex <= 5 && scene.buildIndex % 2 == 0) {
            audioSrc.clip = bgm[1];
        } else if (scene.buildIndex >= 1 && scene.buildIndex <= 5 && scene.buildIndex % 2 == 1) {
            audioSrc.clip = bgm[2];
        } else if (scene.name == "Combat") {
            audioSrc.clip = bgm[3];
        }

        audioSrc.Play();
    }
}
