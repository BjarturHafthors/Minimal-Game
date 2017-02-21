using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurivalTime : MonoBehaviour {

    private float timeSurvived;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        timeSurvived = 0;
    }
	
	// Update is called once per frame
	void Update () {
        timeSurvived += Time.deltaTime;
    }

    public float getTimeSurvived()
    {
        return timeSurvived;
    }

    public void resetTime()
    {
        timeSurvived = 0;
    }
}
