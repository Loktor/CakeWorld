using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : GravityAttractor {  
    private static World _instance;

    private float growthFactor = 0.001f;
    public static World Instance { get { return _instance; } }

    private List<GameObject> collidingObjects = new List<GameObject>();

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

    public float CakeHealth
    {
        get
        {
            return (100.0f + (100.0f - (1.5f * 100.0f / World.Instance.transform.localScale.x)));
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(!collidingObjects.Contains(coll.gameObject) && coll.gameObject.tag == "Muncher")
        {
            collidingObjects.Add(coll.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (collidingObjects.Contains(coll.gameObject))
        {
            collidingObjects.Remove(coll.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		if(collidingObjects.Count > 0)
        {
            DecreaseSize(collidingObjects.Count);
        }
	}

    public void DecreaseSize(int factor)
    {
        ChangeWorldSize(-growthFactor * 0.5f * factor);
    }

    public void IncreaseSize(int factor)
    {
        ChangeWorldSize(growthFactor * factor);
    }

    private void ChangeWorldSize(float growthFactor)
    {
        Vector3 scale = transform.localScale;
        if(scale.x + growthFactor < 0)
        {
            transform.localScale = new Vector3(0, 0);
        }
        else
        {
            transform.localScale = new Vector3(scale.x + growthFactor, scale.y + growthFactor);
        }

        if(growthFactor != 0)
        {
            GameObject repellor = transform.GetChild(0).gameObject;
            PointEffector2D pointEffector = repellor.GetComponent<PointEffector2D>();
            pointEffector.forceMagnitude = growthFactor * 500;
            repellor.SetActive(true);
            StartCoroutine(DeactiveRepellorInSec(repellor, 0.2f));
        }
    }
    
    IEnumerator DeactiveRepellorInSec(GameObject gameObject, float sec)
    {
        yield return new WaitForSeconds(sec);

        gameObject.SetActive(false);
        //Do Function here...
    }
}
