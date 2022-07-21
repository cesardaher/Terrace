using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
	public string newGameScene;
	public string titleScreenScene = "TitleScreen";

	public void NewGame()
	{
		SceneManager.LoadScene(newGameScene);
		TimeManager.newGame = true;
	}

	public void ContinueGame()
	{
		SceneManager.LoadScene(newGameScene);
		TimeManager.newGame = false;
	}

	public void ReturnTitle()
	{
		SceneManager.LoadScene(titleScreenScene);
		//if birds appeared
		if (StoryManager.instance.birdsAppeared)
		{
			//save
			SceneProperties.instance.GetScene();
			SceneProperties.instance.SaveScene();
		}
	}

	public void SaveGame()
    {
		//if birds appeared
		if (StoryManager.instance.birdsAppeared)
		{
			//save
			SceneProperties.instance.GetScene();
			SceneProperties.instance.SaveScene();
		}

	}

	public void QuitGame()
	{
		Debug.Log("button pressed");
		Scene currentScene = SceneManager.GetActiveScene();
		//if in title menu
		
		if (currentScene.name != titleScreenScene)
		{
			SaveGame();
		}

		Application.Quit();
	}

	public void SimpleQuit() //while serialization system is not ready
    {
		Application.Quit();
	}

	public void SimpleReturnTitle() //while serialization system is not ready
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(titleScreenScene);
	}
}
