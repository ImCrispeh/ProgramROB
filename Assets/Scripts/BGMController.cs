using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BGMController : MonoBehaviour {
    public static BGMController _instance;
    public AudioClip[] bgm;
    public AudioSource audioSrc;
    public AudioSource allAudioSrc;
    public Button volumeBtn;
    public Sprite[] volumeImgs;
    public bool isMuted;
    public bool newScene;

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
        newScene = true;
        volumeBtn = GameObject.FindGameObjectWithTag("Volume").GetComponent<Button>();

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

    private void Update() {
        if (newScene) {
            newScene = false;
            if (!isMuted) {
                UnmuteVol();
            } else {
                MuteVol();
            }
        }
    }

    public void MuteVol() {
        isMuted = true;
        volumeBtn.image.sprite = volumeImgs[1];
        foreach (AudioSource src in GameObject.FindObjectsOfType<AudioSource>()) {
            src.volume = 0;
        }
        volumeBtn.onClick.RemoveAllListeners();
        volumeBtn.onClick.AddListener(UnmuteVol);
    }

    public void UnmuteVol() {
        isMuted = false;
        volumeBtn.image.sprite = volumeImgs[0];
        foreach (AudioSource src in GameObject.FindObjectsOfType<AudioSource>()) {
            src.volume = 0.1f;
        }
        volumeBtn.onClick.RemoveAllListeners();
        volumeBtn.onClick.AddListener(MuteVol);
    }
}
