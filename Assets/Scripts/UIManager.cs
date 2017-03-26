using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text health;
    private float time;
    public Text timeSurvived;
    public PlayerController player;
    public SurivalTime survival;


    void Start()
    {

    }

    void Update()
    {
        health.text = System.String.Format("{0:d}", "Health: " + player.health);
        time = survival.getTimeSurvived();

        if (Mathf.Floor(time / 60) >= 1)
        {
            timeSurvived.text = System.String.Format("{0:d}", "Time: " + Mathf.Floor(time / 60) + ":" + Mathf.Floor(time % 60).ToString("00"));
        }
        else
        {
            timeSurvived.text = System.String.Format("{0:d}", "Time: 00:" + Mathf.Floor(time % 60).ToString("00"));
        }
    }
}
