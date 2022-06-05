using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float stepSize = 1f;
    public float speed = 5f;
    public Rigidbody rb;



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

    }
    void MyFixedUpdate()
    {

    }

    void Move()
    {

    }
}
