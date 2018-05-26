using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttackController : MonoBehaviour {

    Animator anim;
	void Start () {
        anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }
}
