using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrap : MonoBehaviour {

    Collider2D electricol;
	void Start () {
        electricol = this.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCol()
    {
        electricol.enabled = false;
    }
    public void OffCol()
    {
        electricol.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}
