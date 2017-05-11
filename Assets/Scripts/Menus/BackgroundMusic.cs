using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour 
{
	private static BackgroundMusic instance = null;
	public static BackgroundMusic Instance 
	{
		get 
		{ 
			return instance; 
		}
	}

	AudioSource audioSource;

	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		} 
		else 
		{
			instance = this;
		}
		
		DontDestroyOnLoad(this.gameObject);
	}

	void Start() 
	{
		audioSource = GetComponent<AudioSource> ();
	}

	public void Mute() 
	{
		audioSource.mute = !audioSource.mute;
	}
}
