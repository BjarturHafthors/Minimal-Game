using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float speed;
    public float RotateSpeed;
    public int score;
    private float timeOfLastShot;
    public float shootCooldown;
    public GameObject bullet;
    private float bulletSpawnOffset;

	// Use this for initialization
	void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        score = 0;
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 0.75f;
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, 0, 1) * -RotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(0, 0, 1) * RotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space) && timeOfLastShot + shootCooldown <= Time.time)
        {
            Instantiate(bullet, transform.position + transform.forward*bulletSpawnOffset, transform.rotation);
            timeOfLastShot = Time.time;
        }

        checkIfOffscreen();
    }

    // Update is called once per frame right before physics happen
    void FixedUpdate () {
        //Store the current vertical input in the float moveVertical.
        float move = Input.GetAxis("Vertical");

        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed * move;
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
}
