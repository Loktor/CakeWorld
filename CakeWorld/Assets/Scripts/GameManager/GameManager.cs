using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public List<GameObject> munchers = new List<GameObject>();
    public List<Cake> cakes = new List<Cake>();
    public int cakeLimit = 2;
    public int muncherLimit = 3;
    public int muncherLimitInternal;

    private List<GameObject> activeMunchers = new List<GameObject>();
    private List<Cake> activeCakes = new List<Cake>();
    private bool spawningMunchers = false;
    private bool spawningCakes = false;
    public static GameManager Instance { get { return _instance; } }
    public Text worldHpText;
    public Text gameOverText;
    public Text scoreText;
    public Text highscoreText;
    public Text helpText;
    public Text headlineText;
    private int score = 0;
    public bool gameOver = false;
    public bool gameRunning = false;

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
        if(ScoreHolder.HasHighScore())
        {
            highscoreText.text = "Highscore: " + ScoreHolder.highscore;
            highscoreText.gameObject.SetActive(true);
        }
        muncherLimitInternal = muncherLimit;

    }
	
	// Update is called once per frame
	void Update () {
        if(gameRunning)
        {
            muncherLimitInternal = muncherLimit + (int)(score / 1500);

            if (activeMunchers.Count < muncherLimitInternal && !spawningMunchers)
            {
                spawningMunchers = true;
                StartCoroutine(SpawnMunchers(muncherLimitInternal - activeMunchers.Count));
                spawningMunchers = false;
            }

            if (activeCakes.Count < cakeLimit && !spawningCakes)
            {
                spawningCakes = true;
                StartCoroutine(SpawnCakes(cakeLimit - activeCakes.Count));
                spawningCakes = false;
            }

            if (!gameOver)
            {
                IncreaseScore(1);

                if (World.Instance.CakeHealth > 0)
                {
                    worldHpText.text = "Cookie-Health: " + (int)World.Instance.CakeHealth;
                }
                else
                {
                    GameOver();
                }
            }
        }
    }

    internal void StartGame()
    {
        gameRunning = true;
        gameOverText.gameObject.SetActive(false);
        headlineText.gameObject.SetActive(false);
        helpText.gameObject.SetActive(true);
    }

    public void IncreaseScore(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreText.text = "Score: " + score;
    }

    void GameOver()
    {
        headlineText.text = "Game Over";
        worldHpText.text = "Cookie-Health: 0";
        scoreText.gameObject.SetActive(false);
        gameOver = true;
        if(ScoreHolder.highscore < score)
        {
            ScoreHolder.highscore = score;
            gameOverText.text = "New Highscore: " + score + " Points\n" + "Press Enter to go back to the menu";
        }
        else
        {
            gameOverText.text = "Score: " + score + " Points\n" + "Press Enter to go back to the menu";
        }
        gameOverText.gameObject.SetActive(true);
        headlineText.gameObject.SetActive(true);
    }

    IEnumerator SpawnMunchers(int count)
    {
        for(int i = 0; i < count; i++)
        {
            SpawnMuncher();
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnCakes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnCake();
            yield return new WaitForSeconds(5);
        }
    }

    void SpawnMuncher()
    {
        GameObject muncher = munchers[UnityEngine.Random.Range(0, munchers.Count)];

        GameObject newMuncher = Instantiate(muncher);
        newMuncher.transform.position = randomMuncherPosition();

        activeMunchers.Add(newMuncher);

    }

    void SpawnCake()
    {
        Cake cake = cakes[UnityEngine.Random.Range(0, cakes.Count)];

        Cake newCake = Instantiate(cake);
        Vector3 cakePosition = randomCakePosition();
        newCake.transform.position = cakePosition;
        var heading = World.Instance.transform.position - cakePosition;
        newCake.direction = heading.normalized;
        newCake.direction.Scale(new Vector2(UnityEngine.Random.Range(0.05f, 0.2f), UnityEngine.Random.Range(0.05f, 0.2f)));

        activeCakes.Add(newCake);
    }


    public void RemoveMuncher(GameObject muncher)
    {
        World.Instance.MuncherDead(muncher);
        muncher.SetActive(false);
        activeMunchers.Remove(muncher);
        Destroy(muncher);
    }

    public void RemoveCake(GameObject cake)
    {
        cake.SetActive(false);
        activeCakes.Remove(cake.GetComponent<Cake>());
        Destroy(cake);
    }

    Vector3 randomMuncherPosition()
    {
        Vector3 worldPosition = World.Instance.transform.position;
        Vector3 worldSize = World.Instance.GetComponent<Renderer>().bounds.size;

        worldPosition.x +=  (UnityEngine.Random.Range(0,2) == 0 ? 1 : -1) * (UnityEngine.Random.Range(5, 30) + worldSize.x);
        worldPosition.y +=  (UnityEngine.Random.Range(0,2) == 0 ? 1 : -1) * (UnityEngine.Random.Range(5, 30) + worldSize.y);

        return worldPosition;
    }

    Vector3 randomCakePosition()
    {
        Vector3 worldPosition = World.Instance.transform.position;
        Vector3 worldSize = World.Instance.GetComponent<Renderer>().bounds.size;

        worldPosition.x += (UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1) * (UnityEngine.Random.Range(5, 10) + worldSize.x);
        worldPosition.y += (UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1) * (UnityEngine.Random.Range(5, 10) + worldSize.y);

        return worldPosition;
    }
}
