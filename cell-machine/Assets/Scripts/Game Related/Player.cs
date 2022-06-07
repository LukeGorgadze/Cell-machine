using UnityEngine;
using DG.Tweening;

public class Player : Movers
{
    override protected void MyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            walk = true;

        Move();
        CheckTheFront();
    }
   
}
