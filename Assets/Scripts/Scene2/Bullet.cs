using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int amount;
    private void Update()
    {
        Destroy(gameObject, 1);
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            target.gameObject.GetComponent<Player>().damaged(amount);
        }
    }
}
