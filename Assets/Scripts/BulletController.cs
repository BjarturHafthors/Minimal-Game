using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private Rigidbody2D rb2d;

    public int strength;
    public float speed;
    public float despawnTime;
    public int homeRadius;
    public float homeSpeed;
    public float timeBeforeHome;
    private float timeOfSpawn;
    private GameObject parent;
    private float angle;


    // Use this for initialization
    void Start()
    {
        timeOfSpawn = Time.time;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        homeRadius = 3;
        homeSpeed = .1f;
        timeBeforeHome = .02f;
    }

    void Update()
    {
        if (timeOfSpawn + despawnTime <= Time.time)
        {
            Destroy(gameObject);
        }

        if (parent.tag == "Player" && timeOfSpawn < Time.time - timeBeforeHome)
        {
            home();
        }

        checkIfOffscreen();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject != parent || Time.time > timeOfSpawn + 0.1f) && (other.gameObject.GetComponent<EnemyController>() is EnemyController || other.gameObject.GetComponent<PlayerController>() is PlayerController))
        {
            Destroy(gameObject);
        }
    }

    void checkIfOffscreen()
    {
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        if (transform.position.x > stageDimensions.x)
        {
            transform.position = new Vector3(-stageDimensions.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -stageDimensions.x)
        {
            transform.position = new Vector3(stageDimensions.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y > stageDimensions.y)
        {
            transform.position = new Vector3(transform.position.x, -stageDimensions.y, transform.position.z);
        }
        else if (transform.position.y < -stageDimensions.y)
        {
            transform.position = new Vector3(transform.position.x, stageDimensions.y, transform.position.z);
        }
    }

    public float getTimeOfSpawn()
    {
        return timeOfSpawn;
    }

    public void setParent(GameObject parent)
    {
        this.parent = parent;
    }

    public GameObject getParent()
    {
        return parent;
    }

    public void home()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, homeRadius);

        for (int i = 0; i < nearbyEnemies.Length; i++)
        {
            if (nearbyEnemies[i].gameObject.tag == "Enemy" && nearbyEnemies[i].gameObject.GetComponent<EnemyController>().isOnScreen())
            {
                Vector2 direction = nearbyEnemies[i].transform.position - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), homeSpeed);
                return;
            }
        }

    }
}
