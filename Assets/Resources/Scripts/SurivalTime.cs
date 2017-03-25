using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurivalTime : MonoBehaviour {

    private float timeSurvived;
    private int maximumHealthReached;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        timeSurvived = 0;
        maximumHealthReached = 15;
    }
	
	// Update is called once per frame
	void Update () {
        timeSurvived += Time.deltaTime;
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
}
