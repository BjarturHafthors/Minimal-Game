using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurivalTime : MonoBehaviour {

    private float timeSurvived;
    private int maximumHealthReached;
    private GameObject player;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        timeSurvived = 0;
        player = GameObject.Find("Player");
        maximumHealthReached = 15;
    }
	
	// Update is called once per frame
	void Update () {
        timeSurvived += Time.deltaTime;
        if (player != null && player.GetComponent<PlayerController>().health > maximumHealthReached)
        {
            maximumHealthReached = player.GetComponent<PlayerController>().health;
        }
    }

    public float getTimeSurvived()
    {
        return timeSurvived;
    }

    public int getMaxHealthReached()
    {
        return maximumHealthReached;
    }

    public void resetTime()
    {
        timeSurvived = 0;
    }

    public void resetMaxhealth()
    {
        maximumHealthReached = 15;
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
}
