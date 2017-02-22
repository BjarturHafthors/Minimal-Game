using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : EnemyController
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
        }
        else if (health <= getInitialHealth())
        {
            GameObject nearest = findNearestOrb();

            rotateTowardsNearestOrb(nearest);

            moveTowardsNearestOrb();
        }
        else
        {
            rotateAwayFromPlayer();

            moveAwayFromPlayer();

            speed += 0.2f;
        }
    }
}
