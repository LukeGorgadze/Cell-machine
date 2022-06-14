using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class Movers : MonoBehaviour
{
    public bool iAmStander;
    public enum boxType
    {
        PLAYER,
        ROTATION,
        PAUSE,
        EXPLODER,

    }
    public boxType MyType;
    public float stepSize = 1f;
    public float speed = 5f;
    public Rigidbody rb;
    public bool walk = true;
    public LayerMask layerMask;
    public LayerMask EnemyLayer;
    public GameObject ExplosionEffect;

    //Local Vars
    public ObjFollowMouse obf;
    protected bool moveIsComplete = false;
    protected bool isMoving;
    public Vector3 moveDirection;
    protected bool inFront;
    public float yAngle = 0;
    protected Movers objToMove;



    protected virtual void OnEnable()
    {
        UpdateManager.onUpdate += MyUpdate;
    }
    protected virtual void OnDisable()
    {
        UpdateManager.onUpdate -= MyUpdate;
    }
    protected virtual void Start()
    {
        moveDirection = transform.forward;
    }

    protected virtual void MyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            walk = true;
            if (!iAmStander)
            {
                Move();
                moveIsComplete = true;
            }
        }



        CheckTheFront();
        checkEnemies();
    }
    public virtual void CheckTheFront()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, moveDirection * 1.2f, Color.red);
        inFront = Physics.Raycast(transform.position, moveDirection, out hit, 1.2f, layerMask);
        if (inFront)
        {
            objToMove = hit.transform.GetComponent<Movers>();
            if (objToMove.MyType != boxType.PLAYER)
                objToMove.moveDirection = this.moveDirection;
            switch (objToMove.MyType)
            {
                case boxType.ROTATION:
                    if (MyType == boxType.PLAYER)
                    {
                        RotateMeRight();
                        objToMove = null;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public virtual void Move()
    {
        if (!moveIsComplete || iAmStander || !walk) { return; }
        isMoving = true;
        if (inFront && objToMove != null)
        {
            transform.DOKill();
            objToMove.MoveTo(transform.forward);
            transform.DOMove(transform.position + transform.forward, speed).OnComplete(AfterStep);
        }
        else
        {
            transform.DOMove(transform.position + transform.forward, speed).OnComplete(AfterStep);
        }
        walk = false;
        moveIsComplete = false;

    }
    public void MoveTo(Vector3 dir)
    {
        if (!walk) { return; }
        isMoving = true;
        if (inFront && objToMove != null)
        {
            if (objToMove.MyType != boxType.PLAYER)
            {
                objToMove.moveDirection = this.moveDirection;
                objToMove.MoveTo(moveDirection);
            }

        }
        transform.DOMove(transform.position + dir, speed).OnComplete(AfterStep);
        walk = false;

    }
    protected virtual void AfterStep()
    {
        obf.UpdateTargetPosition(moveDirection);
        moveIsComplete = true;
        isMoving = false;
        walk = true;
    }

    protected void RotateMeRight()
    {
        yAngle = (yAngle + 90) % 360;
        transform.rotation = Quaternion.Euler(0, yAngle, 0);
        moveDirection = transform.forward;
    }

    protected void checkEnemies()
    {
        RaycastHit hit;
        bool enemyInFront = Physics.Raycast(transform.position, moveDirection, out hit, 1.2f, EnemyLayer);
        if (enemyInFront)
        {
            Destroy(hit.transform.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(obf.targetMovementPos, .1f);
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>

    public void Explode()
    {
        transform.DOKill();
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
