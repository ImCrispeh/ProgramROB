using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour {

    public int pointsAction = 3;
    public int pointsHealth = 3;

    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Player"){

            if (IsHealth())
                AddHealth();

            if (IsActionPoint())
                AddActionPoints();

            Debug.Log("Collision: PowerUp");
            Destroy(gameObject);
        }
    }

    private bool IsHealth(){
        if (gameObject.tag == "PUHealth"){
            return true;
        }
        return false;
    }

    private bool IsActionPoint(){
        if (gameObject.tag == "PUActionPoints"){
            return true;
        }
        return false;
    }

    private void AddActionPoints(){
        FindObjectOfType<PlayerProgramController>().actionPoints += pointsAction;
        FindObjectOfType<StatsController>().UpdateActionPoints(FindObjectOfType<PlayerProgramController>().actionPoints);
    }

    private void AddHealth(){
        FindObjectOfType<PlayerProgramController>().currHealth += pointsHealth;
        FindObjectOfType<StatsController>().UpdateHealth(FindObjectOfType<PlayerProgramController>().currHealth);
    }
}
