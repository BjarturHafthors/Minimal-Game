using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private SpriteRenderer spriteRenderer;
    private int difficulty;
    private bool engineOverheated;
    private int engineHeat;
    public int maxEngineHeat;
    private float timeOfLastEngineCool;
    public float engineCooldown;
    public Slider engineHeatSlider;

    // Use this for initialization
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 1;
        hasShield = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        difficulty = 1;
        engineOverheated = false;
        engineHeat = 0;
        engineHeatSlider.value = 0;
        engineHeatSlider.maxValue = maxEngineHeat;
    }

    // Update is called once per frame

    void Update()
    {
        // Rotate
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            transform.Rotate(new Vector3(0, 0, 1) * -RotateSpeed * Time.deltaTime);
        else if (!(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            transform.Rotate(new Vector3(0, 0, 1) * RotateSpeed * Time.deltaTime);

        // Shoot & EngineHeat
        if (!engineOverheated && Input.GetKey(KeyCode.Space) && timeOfLastShot + shootCooldown <= Time.time)
        {
            GameObject shotBullet = Instantiate(bullet, transform.position + transform.forward*bulletSpawnOffset, transform.rotation);
            shotBullet.GetComponent<BulletController>().setParent(gameObject);
            shotBullet.GetComponent<BulletController>().strength = difficulty;
            timeOfLastShot = Time.time;

            engineHeat += 1;
            engineHeatSlider.value = engineHeat;

            if (engineHeat >= maxEngineHeat)
            {
                engineOverheated = true;
                engineHeat += 5;
            }
        } 
        else if (engineHeat > 0 && timeOfLastEngineCool + engineCooldown <= Time.time && timeOfLastShot + shootCooldown <= Time.time)
        {
            timeOfLastEngineCool = Time.time;
            if (engineOverheated && engineHeat < maxEngineHeat)
            {
                engineOverheated = false;
            }
            else
            {
                engineHeat--;
                engineHeatSlider.value = engineHeat;
            }
        }
        else if (engineHeat < 0)
        {
            engineHeat = 0;
            engineHeatSlider.value = engineHeat;
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
				spawnOrb(other.GetComponent<BulletController> ().strength);
			}
		} 
		else if (other.tag == "ShieldProjectile") 
		{
			hasShield = true;
			transform.Find ("Shield").GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, .75f);
			transform.Find ("Shield").GetComponent<SpriteRenderer> ().enabled = true;
		} 
	}

    public void spawnOrb(int strength)
    {
        string path =  "Prefabs/";

        if (strength == 1)
        {
            path += "PinkOrb";
        }
        else if (strength == 2)
        {
            path += "GreenOrb";
        }
        else if (strength == 3)
        {
            path += "YellowOrb";
        }
        else
        {
            path += "RedOrb";
        }

        GameObject spawnedOrb = Instantiate(Resources.Load<GameObject> (path), transform.position, Quaternion.identity);

        if (health - spawnedOrb.GetComponent<OrbController>().value <= 0)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            health -= spawnedOrb.GetComponent<OrbController>().value;
            game.GetComponent<GameController>().orbs.Add(spawnedOrb);
        }   
    }

    public int getDifficulty()
    {
        return difficulty;
    }

    public void setDifficulty(int difficulty)
    {
        if (difficulty == 2)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/PlayerBlue");
            this.difficulty = 2;
        }
        else if (difficulty == 3)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/PlayerGreen");
            this.difficulty = 3;
        }
        else if (difficulty == 4)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/PlayerRed");
            this.difficulty = 4;
        }
    }
}
