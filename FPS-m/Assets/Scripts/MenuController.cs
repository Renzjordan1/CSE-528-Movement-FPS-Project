using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	// Load the game
	public void LoadGame()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("FPS_Renz 1");
	}

	// Load the main menu
	public void LoadMainMenu()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("Main Menu");
	}

	// Exit the application
	public void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
	}
}
