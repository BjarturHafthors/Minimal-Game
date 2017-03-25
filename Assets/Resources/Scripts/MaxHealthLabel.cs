using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxHealthLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int maxHealth = GameObject.Find("SurvivalTime").GetComponent<SurivalTime>().getMaxHealthReached();

        gameObject.GetComponent<Text>().text = "You reached a maximum health of " + maxHealth + "!";

        GameObject.Find("SurvivalTime").GetComponent<SurivalTime>().resetMaxhealth();
    }
}
