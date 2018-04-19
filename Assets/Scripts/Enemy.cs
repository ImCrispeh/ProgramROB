using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform Player;

    int moveSpeed = 1;
    int minDist = 1; //prevent the enemy from merging with the player object

    void Start(){

    }

    void Update(){
        CheckPosition();
    }

    void CheckPosition(){
        if (Vector2.Distance(transform.position, Player.position) > minDist){
            //transform.LookAt(Player.position);


            if (Player.position.y > transform.position.y)
                MoveUp();
            else if (Player.position.x < transform.position.x)
                MoveLeft();
            else if (Player.position.y < transform.position.y)
                MoveDown();
            else if (Player.position.x > transform.position.x)
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