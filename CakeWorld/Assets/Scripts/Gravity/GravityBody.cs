using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour {
    public GravityAttractor attractor;
    private Transform myTransform;

    public Rigidbody2D RigidBodyAccess
    {
        get
        {
            return GetComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        RigidBodyAccess.constraints = RigidbodyConstraints2D.FreezeRotation;
        myTransform = transform;
    }

    protected virtual void Update()
    {
        if(attractor != null)
        {
            attractor.Attract(this);
        }
    }

}
