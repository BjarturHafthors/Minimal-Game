﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public float speed;
    public float despawnTime;
    private float timeOfSpawn;

    // Use this for initialization
    void Start()
    {
        timeOfSpawn = Time.time;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (timeOfSpawn + despawnTime <= Time.time)
        {
            Destroy(gameObject);
        }

        checkIfOffscreen();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
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
