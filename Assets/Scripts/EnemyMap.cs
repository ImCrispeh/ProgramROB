using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyMap : MonoBehaviour
{
    public Transform player;
    //public Scene battleScene;
    public int moveSpeed = 1; //movement speed
    public float triggerDist = 1f; //battle scene trigger distance

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update(){
        followPlayer();
        triggerBattle();
    }

    void triggerBattle(){
        if (Vector2.Distance(transform.position, player.position) <= triggerDist){
            SceneManager.LoadScene(1);
        }
    }

    void followPlayer(){
        if (Vector2.Distance(transform.position, player.position) > triggerDist){
            if (player.position.y > transform.position.y)
                MoveUp();
            else if (player.position.x < transform.position.x)
                MoveLeft();
            else if (player.position.y < transform.position.y)
                MoveDown();
            else if (player.position.x > transform.position.x)
                MoveRight();
        }
    }

    void MoveUp(){
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        print("enemy_command: up");
    }

    void MoveDown(){
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        print("enemy_command: down");
    }

    void MoveRight(){
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        print("enemy_command: right");
    }

    void MoveLeft() {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        print("enemy_command: left");
    }
}