using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            print(res.width + "x" + res.height);
        }*/
        Screen.SetResolution(1920, 1080, true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
