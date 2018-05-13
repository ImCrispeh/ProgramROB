using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour {

    public int pointsAction = 0;
    public int pointsHealth = 0;
    public int pointsDamage = 0;
    public PlayerProgramController playerMap;
    public PlayerCombatController playerCombat;

    public int visionBuffDuration = 0;
    private Vector3 visionNormal;
    private bool visionBuffActive = false;

    void Start()
    {
        //dmgNormal = FindObjectOfType<PlayerCombatController>().damage; //temp value for original damage
        visionNormal = FindObjectOfType<DarkRoomController>().transform.localScale;
    }

    void Update()
    {
        //if (visionBuffActive && FindObjectOfType<TurnController>().GetIsPlayerTurn() == false)
        //{ // decrement from the buff duration every turn 
        //    visionBuffDuration -= 1;
        //}

        //if (!visionBuffActive && visionBuffDuration <= 0)
        //{ //all buff duration expires, remove buff
        //    RmVisionBuff();
        //    visionBuffActive = false;
        //}
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Player"){

            if (IsHealth())
                IncHealth();

            if (IsActionPoint())
                IncActionPoints();

            //if (IsDamageBuff()){
            //    IncDamageBuff();
            //}

            if (IsVision())
                IncVision();

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

    private bool IsDamageBuff(){
        if (gameObject.tag == "PUDamage"){
            return true;
        }
        return false;
    }

    private bool IsVision(){
        if(gameObject.tag == "PUVision"){
            return true;
        }
        return false;
    }

    private void IncActionPoints(){
        FindObjectOfType<PlayerProgramController>().actionPoints += pointsAction;
        //FindObjectOfType<StatsController>().UpdateActionPoints(FindObjectOfType<PlayerProgramController>().actionPoints);
    }

    private void IncHealth(){
        FindObjectOfType<PlayerProgramController>().currHealth += pointsHealth;
        FindObjectOfType<StatsController>().UpdateHealth(FindObjectOfType<PlayerProgramController>().currHealth);
    }

    //private void IncDamageBuff(){
    //  FindObjectOfType<PlayerCombatController>().damage += pointsDamage;
    //}

    private void IncVision(){
        FindObjectOfType<DarkRoomController>().transform.localScale += new Vector3(0.5f,0.5f,0.5f);
        visionBuffDuration = 3;
    }

    private void RmVisionBuff(){ //for removing vision buff after buff duration ends
        FindObjectOfType<DarkRoomController>().transform.localScale = visionNormal;
    }


}
