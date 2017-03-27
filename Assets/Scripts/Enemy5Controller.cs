using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5Controller : EnemyController
{
    public GameObject shieldProjectile;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        bool canShield = false;

        if (!isOnScreen())
        {
            rotateTowardsPlayer();

            moveTowardsPlayer();
        }
        else if (game.GetComponent<GameController>().enemies.Count > 1)
        {
            GameObject nearest = findNearestEnemy(gameObject);

            if (nearest == null || !nearest.GetComponent<EnemyController>().isOnScreen())
            {
                // Do nothing
            }
            else
            {
                rotateTowardsNearestEnemy(nearest);

                moveTowardsNearestEnemy();

                if (isOnScreen())
                {
                    shootShieldProjectile();
                }

                canShield = true;
            }
        }

        if (!canShield)
        {
            rotateTowardsPlayer();

            moveTowardsPlayer();

            if (isOnScreen())
            {
                shoot();
            }
        }
    }

    protected void shootShieldProjectile()
    {
        if (timeOfLastShot + shootCooldown <= Time.time)
        {
            GameObject shotBullet = Instantiate(shieldProjectile, transform.position + transform.forward * bulletSpawnOffset, transform.rotation);
            shotBullet.GetComponent<BulletController>().setParent(gameObject);
            timeOfLastShot = Time.time;
        }
    }
}
