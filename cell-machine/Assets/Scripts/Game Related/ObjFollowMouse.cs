using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFollowMouse : MonoBehaviour
{
    private PlaceObjectOnGrid placeObjectOnGrid;
    public bool isChosen;
    public Node nodeITook;
    public Vector3 initPos;
    public Vector3 targetMovementPos;
    void Start()
    {
        placeObjectOnGrid = FindObjectOfType<PlaceObjectOnGrid>();
    }

    void Update()
    {
        if (isChosen)
        {
            transform.position = placeObjectOnGrid.smoothMousePosition + new Vector3(0, 0f, 0);
        }
        else
        {
            initPos = transform.position;

        }
    }

    public void UpdateTargetPosition(Vector3 dir)
    {
        targetMovementPos = transform.position + dir;
    }
}
