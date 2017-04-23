using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

    private static CameraController _instance;
    public static CameraController Instance { get { return _instance; } }
    private Camera camera;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        camera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        Camera camera = GetComponent<Camera>();
        if (Debug.isDebugBuild && Input.GetKey(KeyCode.Equals) && camera.orthographicSize > 1)
        {
            camera.orthographicSize -= 0.5f;
        }
        else if(Debug.isDebugBuild && Input.GetKey(KeyCode.Minus))
        {
            camera.orthographicSize += 0.5f;
        }
        else if(Debug.isDebugBuild && Input.GetKey(KeyCode.N))
        {
            World.Instance.DecreaseSize(50);
        }
        else if (Debug.isDebugBuild && Input.GetKey(KeyCode.M))
        {
            World.Instance.IncreaseSize(50);
        }
        else if (GameManager.Instance.gameOver && Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }
        else if (!GameManager.Instance.gameRunning && Input.GetKey(KeyCode.Return))
        {
            GameManager.Instance.StartGame();
        }
        else if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
