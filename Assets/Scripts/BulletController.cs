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
        EnemyController parentController = parent.gameObject.GetComponent<EnemyController>();

        if (parentController is Enemy5Controller)
        {
            strength = parentController.getStrength();
        }

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

        if (parent != null && parent.tag == "Player" && timeOfSpawn < Time.time - timeBeforeHome)
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

        if (transform.position.x > stageDimensions.x || transform.position.x < -stageDimensions.x || transform.position.y > stageDimensions.y || transform.position.y < -stageDimensions.y)
        {
            Destroy(gameObject);
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

    public int getStrength()
    {
        return strength;
    }

    public void home()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, homeRadius);

        if (nearbyEnemies.Length > 0)
        {
            if (nearbyEnemies[0].gameObject.tag == "Enemy" && nearbyEnemies[0].gameObject.GetComponent<EnemyController>().isOnScreen())
            {
                Vector2 direction = nearbyEnemies[0].transform.position - transform.position;
		        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

                float distanceToNearest = Vector3.Distance(nearbyEnemies[0].gameObject.transform.position, transform.position);

                if (distanceToNearest > 1)
                {
            		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), homeSpeed*(homeRadius - distanceToNearest)*1.5f);
            	}

            	return;
            }	
                
        }

    }
}
