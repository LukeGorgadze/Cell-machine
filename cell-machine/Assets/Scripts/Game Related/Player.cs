using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : Movers
{
    public Transform myBat;
    public bool hasBat = false;
    public Animator anim;

    //Locals
    public bool oneTime = true;
    bool oneHit = true;
    public bool ContinuousRun;
    // delegate void HitTheFuckingTarget();
    // HitTheFuckingTarget hitTheFuckingTarget;

    protected override void MyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            walk = true;
            if (!iAmStander)
            {
                moveIsComplete = true;
            }
        }
        Move();


        CheckTheFront();
        checkEnemies();
    }

    public override void CheckTheFront()
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
                case boxType.ROTATIONR:
                    if (MyType == boxType.PLAYER)
                    {
                        RotateMe(90f);
                        objToMove = null;
                    }
                    break;
                case boxType.ROTATIONL:
                    if (MyType == boxType.PLAYER)
                    {
                        RotateMe(180f);
                        objToMove = null;
                    }
                    break;
                case boxType.EXPLODER:
                    if(hasBat)
                        HitTheTarget();
                    break;
                default:
                    break;
            }
        }
    }

    public override void Move()
    {
        if (!moveIsComplete || iAmStander) { return; }
        isMoving = true;
        if (!ContinuousRun)
            anim.SetTrigger("Running");
        else if (oneTime)
        {
            oneTime = false;
            anim.SetTrigger("Running");
        }

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
    protected override void AfterStep()
    {
        obf.UpdateTargetPosition(moveDirection);
        if (!ContinuousRun)
            anim.SetTrigger("DynIdle");
        moveIsComplete = true;
        isMoving = false;
        walk = true;
        // oneTime = true;
    }

    void HitTheTarget()
    {
        if (oneHit)
        {
            print("Hit");
            oneHit = false;
            resetAllTriggers();
            anim.SetTrigger("Throw");
            StartCoroutine(boxExplosion(0.25f));
        }


    }

    private bool isCoroutineExecuting = false;

    IEnumerator boxExplosion(float time)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;


        yield return new WaitForSeconds(time);

        // Code to execute after the delay

        oneHit = true;
        isCoroutineExecuting = false;
        resetAllTriggers();
        anim.SetTrigger("Running");
        if (objToMove != null)
            objToMove.Explode();
    }


    void resetAllTriggers()
    {
        anim.ResetTrigger("DynIdle");
        anim.ResetTrigger("Running");
        anim.ResetTrigger("Throw");
    }

    void runAnim()
    {
        anim.SetTrigger("Running");
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Bat"))
        {
            hasBat = true;
            myBat.gameObject.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}
