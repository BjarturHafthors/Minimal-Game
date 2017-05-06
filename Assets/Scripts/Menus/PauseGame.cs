using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour {
	[SerializeField]
	public Transform canvas;
	private Transform player;
	[SerializeField]
	public Transform pauseMenu;
	[SerializeField]
	public Transform controlsMenu;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			PauseResume();
		} else if (Input.GetKeyDown (KeyCode.R)) {
			 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void PauseResume() 
	{
		if (canvas.gameObject.activeInHierarchy == false) 
		{
			 Cursor.visible = true;

			if (pauseMenu.gameObject.activeInHierarchy == false) 
			{
				ExitControls();
			}

			canvas.gameObject.SetActive (true);
			Time.timeScale = 0;
			player.GetComponent<PlayerController> ().enabled = false;
		} 
		else 
		{
			canvas.gameObject.SetActive (false);
			Time.timeScale = 1;
			player.GetComponent<PlayerController> ().enabled = true;
			 Cursor.visible = false;
		}
	}

	public void ExitControls()
	{
		pauseMenu.gameObject.SetActive (true);
		controlsMenu.gameObject.SetActive (false);
	}

	public void Quit() {
		SceneManager.LoadScene (0);
		//Application.Quit();
		//UnityEditor.EditorApplication.isPlaying = false;
	}

	public void Mute() {
		BackgroundMusic.Instance.Mute (); 
	}

	public void Controls(bool Open) {
		if (Open) {
			controlsMenu.gameObject.SetActive (true);
			pauseMenu.gameObject.SetActive (false);
		} else {
			controlsMenu.gameObject.SetActive (false);
			pauseMenu.gameObject.SetActive (true);
		}
	}
}
