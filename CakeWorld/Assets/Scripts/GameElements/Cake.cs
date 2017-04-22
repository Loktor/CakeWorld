using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour {

    public Vector2 direction;
    Rigidbody2D body;
    public int growthFactor = 50;
    public float RotationSpeed = 60f;
    public float Score = 50;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        body.MovePosition(body.position + direction);
        //transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, World.Instance.transform.position) > 30)
        {
            GameManager.Instance.RemoveCake(this.gameObject);
        }
    }
}
