using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour {

    public float gravity = -10;

    public void Attract(GravityBody body)
    {
        Vector2 gravityUp = (body.transform.position - transform.position).normalized;
        Vector2 bodyUp = body.transform.up;

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.transform.rotation;
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, 100 * Time.deltaTime);
    }
}
