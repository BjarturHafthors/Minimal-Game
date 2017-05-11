using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeOpaque : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .2f);
    }
}
