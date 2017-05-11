using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public float spawnCooldown;
    private float initialCooldown;
    private float timeOfLastSpawn;
    public GameObject player;
	public PlayerController pc;
    public List<GameObject> orbs;
    public List<GameObject> enemies;
    public float spawnAreaLength;
    public int level1SpawnRate;
    public int level2SpawnRate;
    public int level3SpawnRate;
    public int level4SpawnRate;

	public int enemiesWithShield;
	public int hitScore;
	private IEnumerator coroutine;
	private float evalutationTime = 10.0f;
	private float evaluationScore = 100.0f;

    // Use this for initialization
    void Start ()
    {
        timeOfLastSpawn = Time.time - spawnCooldown;
        orbs = new List<GameObject>();
        enemies = new List<GameObject>();
        initialCooldown = spawnCooldown;
        Cursor.visible = false;
		pc = player.GetComponent<PlayerController>();
		enemiesWithShield = 0;

		coroutine = EvaluateScore(evalutationTime);
        StartCoroutine(coroutine);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Time.time >= spawnCooldown + timeOfLastSpawn)
        {
            spawnEnemies();
            timeOfLastSpawn = Time.time;
        }
    }

	IEnumerator EvaluateScore(float seconds) {
		while(true) {
			int lastHealth = pc.health;
			float lastCooldown = spawnCooldown;

			yield return new WaitForSeconds(seconds);

			// 100 + change in health
			float healthChange = (float)(pc.health-lastHealth)/lastHealth* 100.0f;
			Debug.Log("health change: " + healthChange);
			evaluationScore = 100.0f + (healthChange/3);

			Debug.Log("health score: " +evaluationScore);
			evaluationScore += (float)hitScore * 0.5f;
			Debug.Log("hit score: " + evaluationScore);
			//number of enemies that recived shield in the given time
			evaluationScore -= (float)enemiesWithShield;// * 2.0f;
			Debug.Log("shield score: " + evaluationScore);
			evaluationScore -= (float)pc.engineOverheatsCounter * 5.0f;
			Debug.Log("overheat score: " + evaluationScore);
			
			hitScore = 0;
			pc.engineOverheatsCounter = 0;
			enemiesWithShield = 0;

			if (evaluationScore > 140.0f) {
				evaluationScore = 140.0f;
			} else if (evaluationScore < 75.0f) {
				evaluationScore = 75.0f;
			}

			if (spawnCooldown > 6.0f) {
				spawnCooldown = 6.0f;
			} else if (evaluationScore < 103.0f && evaluationScore > 97.0f && spawnCooldown > 3.0f) {
				spawnCooldown = initialCooldown;
			} else {
				Debug.Log("first spawn cooldown: " + spawnCooldown);
				spawnCooldown = spawnCooldown / (evaluationScore/100.0f);
				Debug.Log("spawn cooldown: " + spawnCooldown);
			}
		}
	}

    void spawnEnemies()
    {
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        Vector3 spawnArea = new Vector3(stageDimensions.x + spawnAreaLength, stageDimensions.y + spawnAreaLength, 0);

        int side = Random.Range(0, 4);
        Vector3 spawnLocation = new Vector3();

        // Left
        if (side == 0)
        {
            spawnLocation = new Vector3(Random.Range(-spawnArea.x, -stageDimensions.x), Random.Range(-spawnArea.y, spawnArea.y), 0);
        }
        // Right
        else if (side == 1)
        {
            spawnLocation = new Vector3(Random.Range(stageDimensions.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y), 0);
        }
        // Above
        else if (side == 2)
        {
            spawnLocation = new Vector3(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, -stageDimensions.y), 0);
        }
        //Below
        else
        {
            spawnLocation = new Vector3(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(stageDimensions.y, spawnArea.y), 0);
        }

        int enemyType = Random.Range(1, 6);
        int enemyLevel = Random.Range(0, getTotalSpawnRate());

        string path =  "Prefabs/Enemy" + enemyType;
		int difficulty = 0;

        if (enemyLevel <= level1SpawnRate)
        {
            path += "Green";
			difficulty = 1;

        }
        else if (enemyLevel <= level2SpawnRate+level1SpawnRate)
        {
            path += "Blue";
			difficulty = 2;
        }
        else if (enemyLevel <= level3SpawnRate+level2SpawnRate+level1SpawnRate)
        {
            path += "Orange";
			difficulty = 3;
        }
        else
        {
            path += "Dark";
			difficulty = 4;
        }

        updateSpawnRates();

        GameObject spawnedEnemy = Instantiate(Resources.Load<GameObject>(path), spawnLocation, Quaternion.identity);
		spawnedEnemy.GetComponent<EnemyController>().color = difficulty;

        spawnedEnemy.GetComponent<EnemyController>().player = player;
        spawnedEnemy.GetComponent<EnemyController>().setGame(gameObject);
        int playerDifficulty = player.GetComponent<PlayerController>().getDifficulty();
        enemies.Add(spawnedEnemy);

    }

    public int getTotalSpawnRate()
    {
        return level1SpawnRate + level2SpawnRate + level3SpawnRate + level4SpawnRate;
    }

    public void updateSpawnRates()
    {
        int playerDifficulty = player.GetComponent<PlayerController>().getDifficulty();

        if (playerDifficulty == 1)
        {
            level2SpawnRate += 2;
        }
        else if (playerDifficulty == 2)
        {
            level2SpawnRate += 1;
            level3SpawnRate += 2;
            spawnCooldown -= 0.01f;
        }
        else if (playerDifficulty == 3)
        {
            if (level1SpawnRate > 0)
            {
                level1SpawnRate -= 1;
            }

            level3SpawnRate += 1;
            level4SpawnRate += 2;
            spawnCooldown -= 0.02f;
        }
        else
        {
            if (level1SpawnRate >= 0)
            {
                level1SpawnRate -= 5;
                if (level1SpawnRate < 0)
                {
                    level1SpawnRate = 0;
                }
            }

            if (level2SpawnRate > 0)
            {
                level2SpawnRate -= 1;
            }
            
            level4SpawnRate += 4;
            if(spawnCooldown > 0.83f)
            {
                spawnCooldown -= 0.02f;
            }
        }

        if (playerDifficulty == 1 && level2SpawnRate >= 50)
        {
            player.GetComponent<PlayerController>().setDifficulty(2);
        }
        else if (playerDifficulty == 2 && level3SpawnRate >= 100)
        {
            player.GetComponent<PlayerController>().setDifficulty(3);
        }
        else if (playerDifficulty == 3 && level4SpawnRate >= 100)
        {
            player.GetComponent<PlayerController>().setDifficulty(4);
        }
    }
}
