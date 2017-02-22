using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public float spawnCooldown;
    private float timeOfLastSpawn;
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;
    public List<GameObject> orbs;
    public List<GameObject> enemies;
    public float spawnAreaLength;

    public Sprite enemy1Orange;
    public Sprite enemy1Green;
    public Sprite enemy1Blue;
    public Sprite enemy1Dark;
    public Sprite enemy2Orange;
    public Sprite enemy2Green;
    public Sprite enemy2Blue;
    public Sprite enemy2Dark;
    public Sprite enemy3Orange;
    public Sprite enemy3Green;
    public Sprite enemy3Blue;
    public Sprite enemy3Dark;
    public Sprite enemy4Orange;
    public Sprite enemy4Green;
    public Sprite enemy4Blue;
    public Sprite enemy4Dark;
    public Sprite enemy5Orange;
    public Sprite enemy5Green;
    public Sprite enemy5Blue;
    public Sprite enemy5Dark;

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
        Sprite enemySprite;
        int playerStrength;
        int playerHealth = player.GetComponent<PlayerController>().health;

        if (playerHealth <= 50)
        {
            playerStrength = 1;
        }
        else if (playerHealth > 50 && playerHealth <= 500)
        {
            playerStrength = 2;
        }
        else if (playerHealth > 500 && playerHealth < 5000)
        {
            playerStrength = 3;
        }
        else
        {
            playerStrength = 4;
        }

        GameObject enemyToBeSpawned;

        if (enemyType == 1)
        {
            enemyToBeSpawned = enemy1;

            if (playerStrength == 1)
            {
                enemySprite = enemy1Green;
            }
            else if (playerStrength == 2)
            {
                enemySprite = enemy1Blue;
            }
            else if (playerStrength == 3)
            {
                enemySprite = enemy1Orange;
            }
            else
            {
                enemySprite = enemy1Dark;
            }
        }
        else if (enemyType == 2)
        {
            enemyToBeSpawned = enemy2;

            if (playerStrength == 1)
            {
                enemySprite = enemy2Green;
            }
            else if (playerStrength == 2)
            {
                enemySprite = enemy2Blue;
            }
            else if (playerStrength == 3)
            {
                enemySprite = enemy2Orange;
            }
            else
            {
                enemySprite = enemy2Dark;
            }
        }
        else if (enemyType == 3)
        {
            enemyToBeSpawned = enemy3;

            if (playerStrength == 1)
            {
                enemySprite = enemy3Green;
            }
            else if (playerStrength == 2)
            {
                enemySprite = enemy3Blue;
            }
            else if (playerStrength == 3)
            {
                enemySprite = enemy3Orange;
            }
            else
            {
                enemySprite = enemy3Dark;
            }
        }
        else if (enemyType == 4)
        {
            enemyToBeSpawned = enemy4;

            if (playerStrength == 1)
            {
                enemySprite = enemy4Green;
            }
            else if (playerStrength == 2)
            {
                enemySprite = enemy4Blue;
            }
            else if (playerStrength == 3)
            {
                enemySprite = enemy4Orange;
            }
            else
            {
                enemySprite = enemy4Dark;
            }
        }
        else 
        {
            enemyToBeSpawned = enemy5;

            if (playerStrength == 1)
            {
                enemySprite = enemy5Green;
            }
            else if (playerStrength == 2)
            {
                enemySprite = enemy5Blue;
            }
            else if (playerStrength == 3)
            {
                enemySprite = enemy5Orange;
            }
            else
            {
                enemySprite = enemy5Dark;
            }
        }

        GameObject spawnedEnemy = Instantiate(enemyToBeSpawned, spawnLocation, Quaternion.identity);
        spawnedEnemy.GetComponent<EnemyController>().player = player;
        spawnedEnemy.GetComponent<EnemyController>().setGame(gameObject);
        spawnedEnemy.GetComponent<SpriteRenderer>().sprite = enemySprite;
        enemies.Add(spawnedEnemy);
    }
}
