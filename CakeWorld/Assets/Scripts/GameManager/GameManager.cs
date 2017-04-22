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

    private List<GameObject> activeMunchers = new List<GameObject>();
    private List<Cake> activeCakes = new List<Cake>();
    private bool spawningMunchers = false;
    private bool spawningCakes = false;
    public static GameManager Instance { get { return _instance; } }
    public Text worldHpText;
    public Text gameOverText;
    public Text scoreText;
    public Text highscoreText;
    private int score = 0;
    private bool gameOver = false;

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
	}
	
	// Update is called once per frame
	void Update () {
        if(activeMunchers.Count < muncherLimit && !spawningMunchers)
        {
            spawningMunchers = true;
            StartCoroutine(SpawnMunchers(muncherLimit - activeMunchers.Count));
            spawningMunchers = false;
        }

        if (activeCakes.Count < cakeLimit && !spawningCakes)
        {
            spawningCakes = true;
            StartCoroutine(SpawnCakes(cakeLimit - activeCakes.Count));
            spawningCakes = false;
        }

        if(!gameOver)
        {
            IncreaseScore(1);

            if (World.Instance.CakeHealth > 0)
            {
                worldHpText.text = "Cake-Health: " + World.Instance.CakeHealth + "%";
            }
            else
            {
                StartCoroutine(GameOver());
            }
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreText.text = "Score: " + score;
    }

    IEnumerator GameOver()
    {
        worldHpText.text = "Cake-Health: 0%";
        scoreText.gameObject.SetActive(false);
        gameOver = true;
        if(ScoreHolder.highscore < score)
        {
            ScoreHolder.highscore = score;
            gameOverText.text = "Game Over, New Highscore: " + score + " Points";
        }
        else
        {
            gameOverText.text = "Game Over Score: " + score + " Points";
        }
        gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(0);
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
        GameObject muncher = munchers[Random.Range(0, munchers.Count)];

        GameObject newMuncher = Instantiate(muncher);
        newMuncher.transform.position = randomMuncherPosition();

        activeMunchers.Add(newMuncher);

    }

    void SpawnCake()
    {
        Cake cake = cakes[Random.Range(0, cakes.Count)];

        Cake newCake = Instantiate(cake);
        Vector3 cakePosition = randomCakePosition();
        newCake.transform.position = cakePosition;
        var heading = World.Instance.transform.position - cakePosition;
        newCake.direction = heading.normalized;
        newCake.direction.Scale(new Vector2(Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f)));

        activeCakes.Add(newCake);
    }


    public void RemoveMuncher(GameObject muncher)
    {
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

        worldPosition.x +=  (Random.Range(0,2) == 0 ? 1 : -1) * (Random.Range(20, 50) + worldSize.x);
        worldPosition.y +=  (Random.Range(0,2) == 0 ? 1 : -1) * (Random.Range(20, 50) + worldSize.y);

        return worldPosition;
    }

    Vector3 randomCakePosition()
    {
        Vector3 worldPosition = World.Instance.transform.position;
        Vector3 worldSize = World.Instance.GetComponent<Renderer>().bounds.size;

        worldPosition.x += (Random.Range(0, 2) == 0 ? 1 : -1) * (Random.Range(5, 10) + worldSize.x);
        worldPosition.y += (Random.Range(0, 2) == 0 ? 1 : -1) * (Random.Range(5, 10) + worldSize.y);

        return worldPosition;
    }
}
