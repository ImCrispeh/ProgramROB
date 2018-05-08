using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour {

    public int pointsAction = 3;
    public int pointsHealth = 3;
    public int pointsDamage = 2;

    public int dmgBuffDuration = 0;
    private int dmgCap = 5; //damage cap
    private int dmgNormal;
    private bool dmgBuffActive = false;

    void Start()
    {
        dmgNormal = FindObjectOfType<PlayerCombatController>().damage; //temp value for original damage
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Player"){

            if (IsHealth())
                AddHealth();

            if (IsActionPoint())
                AddActionPoints();

            if (IsDamageBuff()){
                dmgBuffDuration = 3;
                dmgBuffActive = true;

            }

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

    private bool IsDamageBuff()
    {
        if (gameObject.tag == "PUDamage"){
            return true;
        }
        return false;
    }

    private bool DamageBuffActive(){
        if(dmgBuffDuration > 0){
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

    private void AddDamageBuff()
    {
        if(FindObjectOfType<PlayerCombatController>().damage <= dmgCap)
            FindObjectOfType<PlayerCombatController>().damage += pointsDamage;

    }

    private void RemoveDamageBuff() //for removing damage buff after turn duration ends
    {
        if (FindObjectOfType<PlayerCombatController>().damage > dmgNormal && dmgBuffActive == false)
            FindObjectOfType<PlayerCombatController>().damage = dmgNormal;
    }


}
