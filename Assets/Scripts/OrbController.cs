using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {

    public int value;
    public float hoverHeight;
    public float hoverSpeed;
    private Rigidbody2D rb2d;
    private float yOrigin;
    private int lastFLoatDirection;
    private float timeOfSpawn;
    private bool destroy;

    // Use this for initialization
    void Start ()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        yOrigin = gameObject.transform.position.y;
        lastFLoatDirection = -1;
        timeOfSpawn = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    void FixedUpdate()
    {
        hover();
    }

    void hover()
    {
        if (gameObject.transform.position.y - yOrigin >= hoverHeight)
        {
            rb2d.velocity = new Vector2(0, -hoverSpeed);
            lastFLoatDirection = -1;
        }
        else if (yOrigin - gameObject.transform.position.y >= hoverHeight)
        {
            rb2d.velocity = new Vector2(0, hoverSpeed);
            lastFLoatDirection = 1;
        }
        else
        {
            rb2d.velocity = new Vector2(0, lastFLoatDirection * hoverSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && Time.time >= timeOfSpawn + 0.1f)
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.health += value;
            pc.game.GetComponent<GameController>().orbs.Remove(gameObject);

            if (pc.health <= 50)
            {
                pc.setSprite(1);
            }
            else if (pc.health > 50 && pc.health <= 350)
            {
                pc.setSprite(2);
            }
            else if (pc.health > 350 && pc.health < 2500)
            {
                pc.setSprite(3);
            }
            else
            {
                pc.setSprite(4);
            }
            
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy" && Time.time >= timeOfSpawn + 0.1f)
        {
            other.GetComponent<EnemyController>().hasPickedUpOrb = true;
            other.GetComponent<EnemyController>().game.GetComponent<GameController>().orbs.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
