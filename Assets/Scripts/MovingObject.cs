using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
    public float moveDuration = 1f;
    public float moveWait;
    public float moveTimer = 0f;
    //public float moveTime = 0.1f;           
    public LayerMask blockingLayer;
    public bool posX, negX, posY, negY = false;

    public Vector2 lastPos;
    private BoxCollider2D boxCollider;      
    private Rigidbody2D rb2D;               
    private float inverseMoveTime;          


    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        moveWait = moveDuration + 0.2f;
    //inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(float xDir, float yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null && end != lastPos)
        {
            lastPos = start;
            StartCoroutine(SmoothMovement(start, end));

            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector2 source, Vector2 target)
    {
        while (moveTimer < moveDuration) {
            moveTimer += Time.deltaTime;
            rb2D.MovePosition(Vector2.Lerp(source, target, moveTimer / moveDuration));
            yield return null;
        }
        transform.position = target;
        moveTimer = 0f;
        posX = false;
        negX = false;
        posY = false;
        negY = false;
    }

    //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
    protected virtual void AttemptMove<T>(float xDir, float yDir)
        where T : Component
    {
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        Transform hitComponent = hit.transform;
        //If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
        if (!canMove) {
            if (xDir > 0) {
                posX = true;
            }

            if (xDir < 0) {
                negX = true;
            }

            if (yDir > 0) {
                posY = true;
            }

            if (yDir < 0) {
                negY = true;
            }
            OnCantMove(hitComponent, posX, negX, posY, negY);
        }
    }

    //OnCantMove will be overriden by functions in the inheriting classes.
    protected abstract void OnCantMove<T>(T component, bool posX, bool negX, bool posY, bool negY)
        where T : Component;
}