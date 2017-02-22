using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public float spawnCooldown;
    private float timeOfLastSpawn;
    public GameObject player;
    public List<GameObject> orbs;
    public List<GameObject> enemies;
    public float spawnAreaLength;

    public GameObject enemy1Orange;
    public GameObject enemy1Green;
    public GameObject enemy1Blue;
    public GameObject enemy1Dark;
    public GameObject enemy2Orange;
    public GameObject enemy2Green;
    public GameObject enemy2Blue;
    public GameObject enemy2Dark;
    public GameObject enemy3Orange;
    public GameObject enemy3Green;
    public GameObject enemy3Blue;
    public GameObject enemy3Dark;
    public GameObject enemy4Orange;
    public GameObject enemy4Green;
    public GameObject enemy4Blue;
    public GameObject enemy4Dark;
    public GameObject enemy5Orange;
    public GameObject enemy5Green;
    public GameObject enemy5Blue;
    public GameObject enemy5Dark;

    // Use this for initialization
    void Start ()
    {
        timeOfLastSpawn = Time.time - spawnCooldown;
        orbs = new List<GameObject>();
        enemies = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time >= spawnCooldown + timeOfLastSpawn)
        {
            spawnEnemies();
            timeOfLastSpawn = Time.time;
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
        int playerStrength = player.GetComponent<PlayerController>().getStrength();
        int playerHealth = player.GetComponent<PlayerController>().health;

        GameObject enemyToBeSpawned;

        if (enemyType == 1)
        {
            if (playerStrength == 1)
            {
                enemyToBeSpawned = enemy1Green;
            }
            else if (playerStrength == 2)
            {
                enemyToBeSpawned = enemy1Blue;
            }
            else if (playerStrength == 3)
            {
                enemyToBeSpawned = enemy1Orange;
            }
            else
            {
                enemyToBeSpawned = enemy1Dark;
            }
        }
        else if (enemyType == 2)
        {
            if (playerStrength == 1)
            {
                enemyToBeSpawned = enemy2Green;
            }
            else if (playerStrength == 2)
            {
                enemyToBeSpawned = enemy2Blue;
            }
            else if (playerStrength == 3)
            {
                enemyToBeSpawned = enemy2Orange;
            }
            else
            {
                enemyToBeSpawned = enemy2Dark;
            }
        }
        else if (enemyType == 3)
        {
            if (playerStrength == 1)
            {
                enemyToBeSpawned = enemy3Green;
            }
            else if (playerStrength == 2)
            {
                enemyToBeSpawned = enemy3Blue;
            }
            else if (playerStrength == 3)
            {
                enemyToBeSpawned = enemy3Orange;
            }
            else
            {
                enemyToBeSpawned = enemy3Dark;
            }
        }
        else if (enemyType == 4)
        {
            if (playerStrength == 1)
            {
                enemyToBeSpawned = enemy4Green;
            }
            else if (playerStrength == 2)
            {
                enemyToBeSpawned = enemy4Blue;
            }
            else if (playerStrength == 3)
            {
                enemyToBeSpawned = enemy4Orange;
            }
            else
            {
                enemyToBeSpawned = enemy4Dark;
            }
        }
        else 
        {
            if (playerStrength == 1)
            {
                enemyToBeSpawned = enemy5Green;
            }
            else if (playerStrength == 2)
            {
                enemyToBeSpawned = enemy5Blue;
            }
            else if (playerStrength == 3)
            {
                enemyToBeSpawned = enemy5Orange;
            }
            else
            {
                enemyToBeSpawned = enemy5Dark;
            }
        }

        GameObject spawnedEnemy = Instantiate(enemyToBeSpawned, spawnLocation, Quaternion.identity);
        spawnedEnemy.GetComponent<EnemyController>().player = player;
        spawnedEnemy.GetComponent<EnemyController>().setGame(gameObject);
        enemies.Add(spawnedEnemy);
    }
}
