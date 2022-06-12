using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : Movers
{
    public Animator anim;

    //Locals
    public bool oneTime = true;
    bool oneHit = true;
    public bool ContinuousRun;
    // delegate void HitTheFuckingTarget();
    // HitTheFuckingTarget hitTheFuckingTarget;

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
                case boxType.ROTATION:
                    if (MyType == boxType.PLAYER)
                    {
                        RotateMeRight();
                        objToMove = null;
                    }
                    break;
                case boxType.EXPLODER:
                    HitTheTarget();
                    break;
                default:
                    break;
            }
        }
    }

    public override void Move()
    {
        if (!moveIsComplete || iAmStander || !walk) { return; }
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
}
