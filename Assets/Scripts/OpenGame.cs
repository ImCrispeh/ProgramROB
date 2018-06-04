using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGame : MonoBehaviour {
    public int sceneIndex;

    public void StartGame() {
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
