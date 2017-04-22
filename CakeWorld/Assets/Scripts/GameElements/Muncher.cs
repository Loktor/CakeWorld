using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muncher : GravityBody {
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Cake"))
        {
            Physics2D.IgnoreCollision(coll.collider, GetComponent<Collider2D>());
        }
    }
}
