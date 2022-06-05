using UnityEngine;

public class Player : MonoBehaviour
{
    public float stepSize = 1f;
    public float speed = 5f;
    public Rigidbody rb;
    public bool walk;

    //Local Vars
    public ObjFollowMouse obf;



    private void OnEnable()
    {
        UpdateManager.onUpdate += MyUpdate;
        UpdateManager.onFixedUpdate += MyFixedUpdate;
    }
    private void OnDisable()
    {
        UpdateManager.onUpdate -= MyUpdate;
        UpdateManager.onFixedUpdate -= MyFixedUpdate;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void MyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            walk = true;


    }
    void MyFixedUpdate()
    {
        MoveByPhysics();
    }


    void MoveByPhysics()
    {
        if (!walk) { return; }
        // float dist = Vector3.Distance(rb.position, obf.targetMovementPos);
        // if (dist > 0.1f)
        // {
        //     rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
        // }
        // else if (dist <= 0.1f)
        // {
        //     rb.velocity = Vector3.zero;
        //     walk = false;

        // }
      
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
