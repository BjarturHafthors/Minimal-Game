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
    public GameObject orb;
    public GameObject game;
    private float initialHealth;
    private bool hasShield;
    private int strength;

    public GameObject brownOrb;
    public GameObject pinkOrb;
    public GameObject greenOrb;
    public GameObject yellowOrb;
    public GameObject redOrb;
    public GameObject whiteOrb;

    // Use this for initialization
    public virtual void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 1;
        initialHealth = health;
        hasShield = false;
    }

    // Update is called once per frame
    public virtual void Update ()
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

    protected GameObject findNearestEnemy(GameObject self)
    {
        GameObject nearestEnemy = null;
        float nearest = 99999999;
        for (int i = game.GetComponent<GameController>().enemies.Count-1; i >= 0; i--)
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
                GameObject spawnedOrb = Instantiate(orb, transform.position, Quaternion.identity);
                game.GetComponent<GameController>().orbs.Add(spawnedOrb);
                game.GetComponent<GameController>().enemies.Remove(gameObject);
                Destroy(gameObject);
            }
            else
            {
                health -= 1;
            }
        }
        else if (other.tag == "ShieldProjectile" && (other.GetComponent<BulletController>().getParent() != gameObject || Time.time > other.gameObject.GetComponent<BulletController>().getTimeOfSpawn() + 0.1f))
        {
            hasShield = true;
            transform.Find("Shield").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
            transform.Find("Shield").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    protected bool isOnScreen()
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
}
