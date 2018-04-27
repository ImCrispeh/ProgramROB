using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    private Transform target;

    protected override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    void Update(){
        if (Vector2.Distance(gameObject.transform.position, target.transform.position) > 1){
            MoveEnemy();
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
    }

    public void MoveEnemy()
    {
        //These values allow us to choose between the cardinal directions: up, down, left and right.
        int xDir = 0;
        int yDir = 0;

        //If the difference in positions is approximately zero (Epsilon) do the following:
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<Player>(xDir, yDir);
    }

    //OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
    }
}