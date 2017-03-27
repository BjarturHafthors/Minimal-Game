using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed;
    public GameObject player;
    private Rigidbody2D rb2d;
    protected float timeOfLastShot;
    public float shootCooldown;
    public GameObject bullet;
    protected float bulletSpawnOffset;
    public float leastPossibleDistanceToPlayer;
    public float health;
    public GameObject game;
    private float initialHealth;
    private bool hasShield;
    public int strength;
    public bool hasPickedUpOrb;

    // Use this for initialization
    public virtual void Start() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 1;
        initialHealth = health;
        hasShield = false;
        hasPickedUpOrb = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    protected void moveTowardsPlayer()
    {
        Vector3 distance = player.transform.position - transform.position;
        if (Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2)) > leastPossibleDistanceToPlayer)
        {
            rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;
        }
        else
        {
            rb2d.velocity = new Vector2();
        }
    }

    protected GameObject findNearestOrb()
    {
        GameObject nearestOrb = null;
        float nearest = 99999999;
        for (int i = game.GetComponent<GameController>().orbs.Count - 1; i >= 0; i--)
        {
            if (game.GetComponent<GameController>().orbs[i] != null)
            {
                GameObject orb = game.GetComponent<GameController>().orbs[i];
                Vector3 distance = orb.transform.position - transform.position;
                float dist = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));
                if (dist < nearest)
                {
                    nearest = dist;
                    nearestOrb = orb;
                }
            }
        }

        return nearestOrb;
    }

    protected GameObject findNearestMostValuableOrb()
    {
        GameObject bestOrb = null;
        float nearest = 99999999;
        float bestValue = 0;
        for (int i = game.GetComponent<GameController>().orbs.Count - 1; i >= 0; i--)
        {
            if (game.GetComponent<GameController>().orbs[i] != null)
            {
                GameObject orb = game.GetComponent<GameController>().orbs[i];
                Vector3 distance = orb.transform.position - transform.position;
                float dist = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));

                if (bestValue < orb.GetComponent<OrbController>().value)
                {
                    bestOrb = orb;
                    bestValue = orb.GetComponent<OrbController>().value;
                    nearest = dist;
                }
                else if (bestValue == orb.GetComponent<OrbController>().value)
                {
                    if (dist < nearest)
                    {
                        nearest = dist;
                        bestOrb = orb;
                    }
                }
            }
        }

        return bestOrb;
    }

    protected GameObject findNearestEnemy(GameObject self)
    {
        GameObject nearestEnemy = null;
        float nearest = 99999999;
        for (int i = game.GetComponent<GameController>().enemies.Count - 1; i >= 0; i--)
        {
            if (game.GetComponent<GameController>().enemies[i] != null)
            {
                GameObject enemy = game.GetComponent<GameController>().enemies[i];
                if (enemy != self && !enemy.GetComponent<EnemyController>().hasShield)
                {
                    Vector3 distance = enemy.transform.position - transform.position;
                    float dist = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));
                    if (dist < nearest)
                    {
                        nearest = dist;
                        nearestEnemy = enemy;
                    }
                }
            }
        }

        return nearestEnemy;
    }

    protected void moveTowardsNearestEnemy()
    {
        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;
    }

    protected void moveTowardsNearestOrb()
    {
        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;
    }

    protected void rotateTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected void rotateTowardsNearestEnemy(GameObject enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected void rotateAwayFromPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected void moveAwayFromPlayer()
    {
        if (isOnScreen())
        {
            rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected void rotateTowardsNearestOrb(GameObject orb)
    {
        Vector3 direction = orb.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected void shoot()
    {
        if (timeOfLastShot + shootCooldown <= Time.time)
        {
            GameObject shotBullet = Instantiate(bullet, transform.position + transform.forward * bulletSpawnOffset, transform.rotation);
            shotBullet.GetComponent<BulletController>().setParent(gameObject);
            shotBullet.GetComponent<BulletController>().strength = strength;
            timeOfLastShot = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet" && (other.GetComponent<BulletController>().getParent() != gameObject || Time.time > other.gameObject.GetComponent<BulletController>().getTimeOfSpawn() + 0.1f))
        {
            if (hasShield)
            {
                hasShield = false;
                transform.Find("Shield").GetComponent<SpriteRenderer>().enabled = false;
            }
            else if (health - 1 <= 0)
            {
                spawnOrb();
                Destroy(gameObject);
            }
            else
            {
                health--;
            }
        }
        else if (other.tag == "ShieldProjectile" && (other.GetComponent<BulletController>().getParent() != gameObject || Time.time > other.gameObject.GetComponent<BulletController>().getTimeOfSpawn() + 0.1f))
        {
            hasShield = true;
            transform.Find("Shield").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
            transform.Find("Shield").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public bool isOnScreen()
    {
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        return !(transform.position.x > stageDimensions.x || transform.position.x < -stageDimensions.x || transform.position.y > stageDimensions.y || transform.position.y < -stageDimensions.y);
    }

    public void setGame(GameObject game)
    {
        this.game = game;
    }

    public float getInitialHealth()
    {
        return initialHealth;
    }

    public void setStrength(int strength)
    {
        this.strength = strength;
    }

    public int getStrength()
    {
        return strength;
    }

    private void spawnOrb()
    {
        int lucky = Random.Range(0, 100);
        
        int orbStrength;

        if (lucky == 0)
        {
            orbStrength = strength + 2;
        }
        else
        {
            int semiLucky = Random.Range(0, 10);

            if(semiLucky == 0)
        {
                orbStrength = strength + 1;
            }
            else
            {
                orbStrength = strength;
            }
        }

        GameObject orb = getCorrectOrb(orbStrength);

        GameObject spawnedOrb = Instantiate(orb, transform.position, Quaternion.identity);
        game.GetComponent<GameController>().orbs.Add(spawnedOrb);
        game.GetComponent<GameController>().enemies.Remove(gameObject);
    }

    private GameObject getCorrectOrb(int orbStrength)
    {
        string path =  "Prefabs/";

        if (orbStrength == 1)
        {
            path += "BrownOrb";
        }
        else if (orbStrength == 2)
        {
            path += "PinkOrb";
        }
        else if (orbStrength == 3)
        {
            path += "GreenOrb";
        }
        else if (orbStrength == 4)
        {
            path += "YellowOrb";
        }
        else if (orbStrength == 5)
        {
            path += "RedOrb";
        }
        else
        {
            path += "WhiteOrb";
        }

        return Resources.Load<GameObject> (path);
    }
}
