using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Controller : EnemyController {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (game.GetComponent<GameController>().orbs.Count == 0)
        {
            moveTowardsPlayer();

            rotateTowardsPlayer();

            if (isOnScreen())
            {
                shoot();
            }
        }
        else
        {
            GameObject nearest = findNearestOrb();

            rotateTowardsNearestOrb(nearest);

            moveTowardsNearestOrb(nearest);
        }
    }
}
