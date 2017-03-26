using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	[SerializeField]
	public Transform mainMenu;
	[SerializeField]
	public Transform controlsMenu;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			Mute();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Back();
		} 
	}

	public void Back() {
		if (controlsMenu.gameObject.activeInHierarchy == true) {
			controlsMenu.gameObject.SetActive (false);
			mainMenu.gameObject.SetActive (true);
		}
	}

	public void NewGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void Mute() {
		//BackgroundMusic.Instance.Mute (); 
	}

	public void Controls(bool Open) {
		if (controlsMenu.gameObject.activeInHierarchy == false) {
			mainMenu.gameObject.SetActive (false);
			controlsMenu.gameObject.SetActive (true);
		}
	}

	public void ExitGame() {

		Application.Quit();
		//UnityEditor.EditorApplication.isPlaying = false;
	}	
}
