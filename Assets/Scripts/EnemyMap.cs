using UnityEngine;
using System.Collections;

public class EnemyMap : MovingObject
{
    public int enemyID;
    public bool isAlive;
    public bool isMoving = false;
    public float moveDist = 1.5f;
    public Transform target;

	public bool isEnemy;
	public bool isSpider;
	public bool isTurret;
    public bool isTank;
	public int enemyAmt;
	public int spiderAmt;
	public int turretAmt;
    public int tankAmt;

    protected override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ChangeSpeed(TurnController._instance.speedChangeTgl.isOn);
        base.Start();
    }

    void Update(){
    }

    protected override void AttemptMove<T>(float xDir, float yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
    }

    public IEnumerator MoveEnemy()
    {
        //These values allow us to choose between the cardinal directions: up, down, left and right.
        float xDir = 0;
        float yDir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) >= 0 && Mathf.Abs(target.position.x - transform.position.x) <= moveDist/2)
        {
            yDir = target.position.y > transform.position.y ? moveDist : moveDist * -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? moveDist : moveDist * -1;
        }
		
		if (TurnController._instance.isPlayerRepelling) {
			yDir = -yDir;
			xDir = -xDir;
            base.blockingLayer = (1 << LayerMask.NameToLayer("Obstacle")) | (1 << LayerMask.NameToLayer("Player"));
		} else {
            base.blockingLayer = 1 << LayerMask.NameToLayer("Obstacle");
        }
        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<Player>(xDir, yDir);
        yield return new WaitForSeconds(base.moveWait);
    }

    //OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
    protected override void OnCantMove<T>(T component, bool posX, bool negX, bool posY, bool negY) {
        if ((posY || negY) && !posX && !negX) {
            float xDir = target.position.x > transform.position.x ? moveDist : moveDist * -1;
            AttemptMove<Player>(xDir, 0);
        } else if ((posY || negY) && !posX) {
            AttemptMove<Player>(moveDist, 0);
        } else if ((posY || negY) && !negX) {
            AttemptMove<Player>(moveDist * -1, 0);
        } else if ((posX || negX) && !posY && !negY) {
            float yDir = target.position.y > transform.position.y ? moveDist : moveDist * -1;
            AttemptMove<Player>(0, yDir);
        } else if ((posX || negX) && !posY) {
            AttemptMove<Player>(0, moveDist);
        } else if ((posX || negX) && !negY) {
            AttemptMove<Player>(0, moveDist * -1);
        }
    }

    //Essentially clear the last position so it can backtrack on the next turn
    public void ResetAfterTurn() {
        base.lastPos = new Vector2(-9999, -9999);
    }

    public void ChangeSpeed(bool isFast) {
        if (isFast) {
            base.moveDuration = 0.4f;
            base.moveWait = base.moveDuration + 0.05f;
        } else {
            base.moveDuration = 0.75f;
            base.moveWait = base.moveDuration + 0.1f;
        }
    }
}