using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float speed;
    public float RotateSpeed;
    public int health;
    private float timeOfLastShot;
    public float shootCooldown;
    public GameObject bullet;
    private float bulletSpawnOffset;
    public GameObject game;
    private bool hasShield;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    private SpriteRenderer spriteRenderer;
    private int strength;

    public GameObject brownOrb;
    public GameObject pinkOrb;
    public GameObject greenOrb;
    public GameObject yellowOrb;
    public GameObject redOrb;
    public GameObject whiteOrb;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 1;
        hasShield = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        strength = 1;
    }

    // Update is called once per frame

    void Update()
    {
        // Rotate
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, 0, 1) * -RotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(0, 0, 1) * RotateSpeed * Time.deltaTime);

        // Shoot
        if (Input.GetKey(KeyCode.Space) && timeOfLastShot + shootCooldown <= Time.time)
        {
            GameObject shotBullet = Instantiate(bullet, transform.position + transform.forward*bulletSpawnOffset, transform.rotation);
            shotBullet.GetComponent<BulletController>().setParent(gameObject);
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

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Bullet" && (other.GetComponent<BulletController> ().getParent () != gameObject || Time.time > other.gameObject.GetComponent<BulletController> ().getTimeOfSpawn () + 0.1f)) 
		{
			if (hasShield) 
			{
				hasShield = false;
				transform.Find ("Shield").GetComponent<SpriteRenderer> ().enabled = false;
			} 
			else 
			{
				spawnOrb (strength);
			}
		} 
		else if (other.tag == "ShieldProjectile") 
		{
			hasShield = true;
			transform.Find ("Shield").GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, .75f);
			transform.Find ("Shield").GetComponent<SpriteRenderer> ().enabled = true;
		} 
		else if (other.tag == "PickUp") 
		{
			if (strength == 1 && health >= 30 && health < 350)
			{
				setSprite(2);
			}
			else if (strength == 2 && health >= 350 && health < 2500)
			{
				setSprite(3);
			}
			else if (strength == 3 && health > 2500)
			{
				setSprite(4);
			}
		}
	}

    private void spawnOrb(int bulletStrength)
    {
        GameObject orb;

        if (strength == 1)
        {
            orb = pinkOrb;
        }
        else if (strength == 2)
        {
            orb = greenOrb;
        }
        else if (strength == 3)
        {
            orb = yellowOrb;
        }
        else
        {
            orb = redOrb;
        }

        if (health - orb.GetComponent<OrbController>().value < 0)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            health -= orb.GetComponent<OrbController>().value;
            GameObject spawnedOrb = Instantiate(orb, transform.position, Quaternion.identity);
            game.GetComponent<GameController>().orbs.Add(spawnedOrb);
        }   
    }

    public void setSprite(int number)
    {
        if (number == 1)
        {
            spriteRenderer.sprite = sprite1;
            strength = 1;
        }
        else if (number == 2)
        {
            spriteRenderer.sprite = sprite2;
            strength = 2;
        }
        else if (number == 3)
        {
            spriteRenderer.sprite = sprite3;
            strength = 3;
        }
        else
        {
            spriteRenderer.sprite = sprite4;
            strength = 4;
        }
    }

    public int getStrength()
    {
        return strength;
    }
}
