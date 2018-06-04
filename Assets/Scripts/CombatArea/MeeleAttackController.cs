using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttackController : MonoBehaviour {
    Animator anim;
    private PlayerCombatController player;
    public GameObject particle;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = transform.parent.parent.parent.GetComponent<PlayerCombatController>();
    }
    void Update() {
        if (player.enabled) {
            if (Input.GetButtonDown("Fire1")) {
                anim.SetBool("isMelee", true);
            } else {
                anim.SetBool("isMelee", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>() != null) {
                other.gameObject.GetComponent<Enemy>().damaged(player.damage * 2);
                Instantiate(particle, other.transform.position, Quaternion.identity);
            }
            else if (other.gameObject.GetComponent<FireEnemy>() != null)
            {
                other.gameObject.GetComponent<FireEnemy>().damaged(player.damage * 2);
                Instantiate(particle, other.transform.position, Quaternion.identity);
            }
            else if (other.gameObject.GetComponent<Turrets>() != null)
            {
                other.gameObject.GetComponent<Turrets>().damaged(player.damage * 2);
                Instantiate(particle, other.transform.position, Quaternion.identity);
            }
            else if (other.gameObject.GetComponent<TankEnemy>() != null)
            {
                other.gameObject.GetComponent<TankEnemy>().damaged(player.damage * 2);
                Instantiate(particle, other.transform.position, Quaternion.identity);
            }
           
            
        }
        else if (other.gameObject.tag == "Wall")
        {
            anim.SetBool("isMelee", false);
        }
    }
}
