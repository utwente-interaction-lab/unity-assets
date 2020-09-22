using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class IdleTillContact : MonoBehaviour
{
    //variables
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    //release object if something is thrown against it
    private void OnCollisionEnter(Collision collision)
    {
        ReleaseObject();
    }

    //release object if it is being let go by the player
    private void Update()
    {
        var v = rb.velocity;
        if ((v.x + v.y + v.z) > 0)
            ReleaseObject();
    }

    //enable gravity on the object
    private void ReleaseObject()
    {
        rb.useGravity = true;
    }
}
