using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apache : MonoBehaviour
{
    [SerializeField]
    private Transform tr;
    [SerializeField]
    float h, v, ud,rot;
    public float movespeed = 20f;
    public float rotspeed = 100f;
    public bool isGetIn;
    private Rigidbody rbody;
    public bool isGround;

    void Start()
    {
        tr = GetComponent<Transform>();
        isGetIn = false;
        rbody = GetComponent<Rigidbody>();
        isGround = true;
    }

    void Update()
    { 
       ApacheControl();

    }

    void ApacheControl()
    {
        if (isGetIn == true)
        {
            rbody.useGravity = false;
            rbody.mass = 1;
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            ud = Input.GetAxis("UpDown");
            rot = Input.GetAxis("Mouse X");
            Vector3 moveApache = (Vector3.forward * v) + (Vector3.right * h)
                + (Vector3.up * ud);
            tr.Translate(moveApache.normalized * Time.deltaTime * movespeed);
            tr.Rotate(Vector3.up * rot * Time.deltaTime * rotspeed);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        isGround = true;

    }
    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }

}
