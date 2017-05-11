using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour 
{
	public void LoadByIndex(int sceneIndex)
    {
        if(sceneIndex == 1)
        {
            GameObject.Find("SurvivalTime").GetComponent<SurivalTime>().resetTime();
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
