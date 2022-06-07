using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBox : Movers
{
    // public override void TakeMySuperPower(Movers mover)
    // {
    //     mover.RotateMeRight();
    //     print("power granted");
    // }
    override protected void MyUpdate()
    {
        CheckTheFront();
    }
}
