﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float speed;
    public float RotateSpeed;
    public int score;

	// Use this for initialization
	void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        score = 0;
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, 0, 1) * -RotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(0, 0, 1) * RotateSpeed * Time.deltaTime);
    }

    // Update is called once per frame right before physics happen
    void FixedUpdate () {
        //Store the current vertical input in the float moveVertical.
        float move = Input.GetAxis("Vertical");

        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed * move;
    }
}
