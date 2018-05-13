using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour {

    public int pointsAction = 0;
    public int pointsHealth = 0;
    public int pointsDamage = 0;
    public PlayerProgramController playerMap;
    public PlayerCombatController playerCombat;

    public float visionBuffDuration = 0;
    public int visionIncrease;
    public float visionNorm;

    void Start()
    {
        //dmgNormal = FindObjectOfType<PlayerCombatController>().damage; //temp value for original damage
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

    private void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Player"){

            if (IsHealth())
                IncHealth();

            if (IsActionPoint())
                IncActionPoints();

            if (IsDamageBuff()){
                IncDamageBuff();
            }

            if (IsVision())
                IncVision();

            Debug.Log("Collision: PowerUp");
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 6f);
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
        FindObjectOfType<StatsController>().UpdateActionPoints(FindObjectOfType<PlayerProgramController>().actionPoints, FindObjectOfType<PlayerProgramController>().currNumOfActions);
    }

    private void IncHealth(){
        if (FindObjectOfType<PlayerProgramController>() != null) {
            FindObjectOfType<PlayerProgramController>().currHealth += pointsHealth;
            FindObjectOfType<StatsController>().UpdateHealth(FindObjectOfType<PlayerProgramController>().currHealth);
        } else {
            FindObjectOfType<PlayerCombatController>().health += pointsHealth;
        }
    }

    private void IncDamageBuff(){
        FindObjectOfType<PlayerCombatController>().damage += pointsDamage;
    }

    private void IncVision(){
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProgramController>() != null) {
            GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Light>().spotAngle += visionIncrease;
        } else {
            Debug.Log("test");
            visionNorm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).GetComponent<Light>().spotAngle;
            StartCoroutine(CombatVisionIncrease());
        }
    }

    IEnumerator CombatVisionIncrease() {
        GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).GetComponent<Light>().spotAngle += visionIncrease;
        yield return new WaitForSeconds(visionBuffDuration);
        GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).GetComponent<Light>().spotAngle = visionNorm;
    }

    private void RmVisionBuff(){ //for removing vision buff after buff duration ends
        
    }


}
