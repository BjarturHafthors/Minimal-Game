using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLabelText : MonoBehaviour {
    private float time;

	// Use this for initialization
	void Start () {
        time = GameObject.Find("SurvivalTime").GetComponent<SurivalTime>().getTimeSurvived();

        if (Mathf.Floor(time % 60) != 1)
        {
            if (Mathf.Floor(time / 60) > 1)
            {
                gameObject.GetComponent<Text>().text = "You survived for " + Mathf.Floor(time / 60) + " minutes and " + Mathf.Floor(time % 60) + " seconds!";
            }
            else if (Mathf.Floor(time / 60) == 1)
            {
                gameObject.GetComponent<Text>().text = "You survived for " + Mathf.Floor(time / 60) + " minute and " + Mathf.Floor(time % 60) + " seconds!";
            }
            else
            {
                gameObject.GetComponent<Text>().text = "You survived for " + Mathf.Floor(time % 60) + " seconds!";
            }
        }
        else
        {
            if (Mathf.Floor(time / 60) > 1)
            {
                gameObject.GetComponent<Text>().text = "You survived for " + Mathf.Floor(time / 60) + " minutes and " + Mathf.Floor(time % 60) + " second!";
            }
            else if (Mathf.Floor(time / 60) == 1)
            {
                gameObject.GetComponent<Text>().text = "You survived for " + Mathf.Floor(time / 60) + " minute and " + Mathf.Floor(time % 60) + " second!";
            }
            else
            {
                gameObject.GetComponent<Text>().text = "You survived for " + Mathf.Floor(time % 60) + " second!";
            }
        }

        GameObject.Find("SurvivalTime").GetComponent<SurivalTime>().resetTime();
    }
}
