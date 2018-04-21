using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {
    public int amount;
    private void OnCollisionEnter2D(Collision2D target)
    {
        if(target.gameObject.tag == "Enemy")
        {
            target.gameObject.GetComponent<Enemy>().damaged(amount);
        }
    }
}
