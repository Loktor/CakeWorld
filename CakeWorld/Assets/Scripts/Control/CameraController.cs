using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Camera camera = GetComponent<Camera>();
        if (Input.GetKey(KeyCode.Equals) && camera.orthographicSize > 1)
        {
            camera.orthographicSize -= 0.5f;
        }
        else if(Input.GetKey(KeyCode.Minus))
        {
            camera.orthographicSize += 0.5f;
        }
        else if(Input.GetKey(KeyCode.N))
        {
            World.Instance.DecreaseSize(50);
        }
        else if (Input.GetKey(KeyCode.M))
        {
            World.Instance.IncreaseSize(50);
        }
    }
}
