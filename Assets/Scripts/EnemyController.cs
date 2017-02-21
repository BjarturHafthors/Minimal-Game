using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {

    public float speed;
    public float RotateSpeed;
    public GameObject player;
    private bool hasStolenOrb;
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start () {
        hasStolenOrb = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        followPlayer();
    }

    void followPlayer()
    {
        Vector2 movementVector = new Vector2();

        if (player.transform.position.x > gameObject.transform.position.x)
        {
            movementVector.x = 1;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (player.transform.position.x > gameObject.transform.position.x)
        {
            movementVector.x = 0;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            movementVector.x = -1;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (player.transform.position.y > gameObject.transform.position.y)
        {
            movementVector.y = 1;
        }
        else if (player.transform.position.y > gameObject.transform.position.y)
        {
            movementVector.y = 0;
        }
        else
        {
            movementVector.y = -1;
        }

        rb2d.velocity = movementVector * speed;
    }
}
