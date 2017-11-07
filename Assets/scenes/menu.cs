using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {

	public void startGame () {
		SceneManager.LoadScene("level_0");
	}

	public void openTutorial () {
		SceneManager.LoadScene("tutorial");
	}

	public void openDeveloper () {
		SceneManager.LoadScene("developer");
	}

	public void exitGame() {
		Application.Quit ();
	}

	public void returnMainMenu () {
		SceneManager.LoadScene("menu");
	}
}
