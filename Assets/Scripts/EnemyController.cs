using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed;
    public float RotateSpeed;
    public GameObject player;
    private bool hasStolenOrb;
    private Rigidbody2D rb2d;
    private float timeOfLastShot;
    public float shootCooldown;
    public GameObject bullet;
    private float bulletSpawnOffset;
    public float leastPossibleDistanceToPlayer;
    private float health;
    public GameObject orb;

    // Use this for initialization
    void Start () {
        hasStolenOrb = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 1;
        health = 5;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = player.transform.position - transform.position;
        if (Mathf.Abs(direction.x) > leastPossibleDistanceToPlayer || Mathf.Abs(direction.y) > leastPossibleDistanceToPlayer)
        {
            // Move
            rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed;
        }

        // Rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90 ;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Shoot
        if (timeOfLastShot + shootCooldown <= Time.time)
        {
            Instantiate(bullet, transform.position + transform.forward * bulletSpawnOffset, transform.rotation);
            timeOfLastShot = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<BulletController>() is BulletController && Time.time > other.gameObject.GetComponent<BulletController>().getTimeOfSpawn() + 0.1f)
        {
            if (health - 5 <= 0)
            {
                Instantiate(orb, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                health -= 5;
            }
        }
    }
}
