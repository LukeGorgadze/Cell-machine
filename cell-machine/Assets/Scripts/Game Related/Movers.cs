using UnityEngine;
using DG.Tweening;

public class Movers : MonoBehaviour
{
    public string name;
    public float stepSize = 1f;
    public float speed = 5f;
    public Rigidbody rb;
    public bool walk = true;
    public LayerMask layerMask;

    //Local Vars
    public ObjFollowMouse obf;
    bool moveIsComplete = true;
    protected bool isMoving;



    protected virtual void OnEnable()
    {
        UpdateManager.onUpdate += MyUpdate;
        //UpdateManager.onFixedUpdate += MyFixedUpdate;
    }
    protected virtual void OnDisable()
    {
        UpdateManager.onUpdate -= MyUpdate;
        //UpdateManager.onFixedUpdate -= MyFixedUpdate;
    }
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void MyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            walk = true;

        CheckTheFront();


    }
    //

    public virtual void Move(Vector3 dir)
    {
        if (!walk || !moveIsComplete) { return; }
        isMoving = true;
        transform.DOMove(transform.position + dir, speed).OnComplete(AfterStep);
        walk = false;
        moveIsComplete = false;

    }
    public void RotateMeRight()
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation;

        if (currentRotation == targetRotation)
        {
            targetRotation *= Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }

        float angle = transform.rotation.y;
        Quaternion curRot = transform.rotation;
        transform.rotation = targetRotation;
        //transform.DORotate(new Vector3(0, 90, 0), 1, RotateMode.WorldAxisAdd);
    }

    protected virtual void AfterStep()
    {
        obf.UpdateTargetPosition();
        moveIsComplete = true;
        isMoving = false;
    }

    public virtual void TakeMySuperPower(Movers mover)
    {

    }
    public virtual void CheckTheFront()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);

        bool inFront = Physics.Raycast(transform.position, transform.forward, out hit, .5f, layerMask);
        if (inFront)
        {
            Movers obj = hit.transform.GetComponent<Movers>();
            obj.Move(transform.forward);
            // switch (obj.name)
            // {
            //     case "rotateBox":
            //         obj.TakeMySuperPower(this);
            //         break;
            //     default:
                    
            //         break;
            // }
        }
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(obf.targetMovementPos, .1f);
    }
}
