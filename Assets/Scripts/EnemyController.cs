﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed;
    public GameObject player;
    private Rigidbody2D rb2d;
    private float timeOfLastShot;
    public float shootCooldown;
    public GameObject bullet;
    private float bulletSpawnOffset;
    public float leastPossibleDistanceToPlayer;
    public float health;
    public GameObject orb;
    public GameObject game;
    private float initialHealth;

    // Use this for initialization
    public virtual void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 1;
        initialHealth = health;
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
        GameObject nearestOrb = new GameObject();
        float nearest = 99999999;
        foreach (GameObject orb in game.GetComponent<GameController>().orbs)
        {
            Vector3 distance = orb.transform.position - transform.position;
            float dist = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));
            if (dist < nearest)
            {
                nearest = dist;
                nearestOrb = orb;
            }
        }

        return nearestOrb;
    }

    protected void moveTowardsNearestOrb(GameObject orb)
    {
        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;
    }

    protected void rotateTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
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
            if (health - 1 <= 0)
            {
                GameObject spawnedOrb = Instantiate(orb, transform.position, Quaternion.identity);
                game.GetComponent<GameController>().orbs.AddLast(spawnedOrb);
                Destroy(gameObject);
            }
            else
            {
                health -= 1;
            }
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
}
