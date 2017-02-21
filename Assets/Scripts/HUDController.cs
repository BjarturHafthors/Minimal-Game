using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {
    public GameObject player;
    private float timeSurvived;

	// Use this for initialization
	void Start () {
        timeSurvived = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timeSurvived += Time.deltaTime;
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 50), "Score: " + player.GetComponent<PlayerController>().score);
        GUI.Label(new Rect(Screen.width/2-75, 10, 150, 50), "Time alive: " + timeSurvived.ToString("0.0"));
    }
}
