using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : EnemyController
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (game.GetComponent<GameController>().orbs.Count == 0 && health <= getInitialHealth())
        {
            rotateTowardsPlayer();

            moveTowardsPlayer();

            if (isOnScreen())
            {
                shoot();
            }
        }
        else if (!hasPickedUpOrb)
        {
            GameObject nearest = findNearestOrb();

            if (nearest == null)
            {
                // Do nothing
            }
            else
            {
                rotateTowardsNearestOrb(nearest);

                moveTowardsNearestOrb();
            }
        }
        else
        {
            rotateAwayFromPlayer();

            moveAwayFromPlayer();
        }
    }
}
