using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;

/// <summary>
/// This script is being made so that from the main menu the player will be able to:
/// -start the game
/// -open settings and return from settings
/// -quit the game
/// </summary>

public class MainMenu : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject settings;
	private RandomChunkLoader loader;


    //initialization
    void Start ()
    {
		mainMenu.SetActive(true);
		settings.SetActive(false);
		loader = GameObject.Find("Loader").GetComponent<RandomChunkLoader>();

    }


    public void gameStart(bool maleChosen) //will start the game when selected
    {
        // Character Sounds
        // Put the code here, before the scene is loaded
        // Male chosen is true if the player selects male
		if (maleChosen == true) 
		{
			GameManager.playerIsMale = true;
		} 
		else 
		{
			GameManager.playerIsMale = false;
		}

        Debug.Log(maleChosen);
		SceneManager.LoadScene(1);
		mainMenu.SetActive(false);
        settings.SetActive(false);
    }

    public void openSettings() //opens the settings menu, hides the main menu
    {
		mainMenu.SetActive(false);
		settings.SetActive(true);
    }

    public void closeSettings() //reverse of the openSettings function
    {
		mainMenu.SetActive(true);
		settings.SetActive(false);
    }

    public void exitGame() //quits the game, is ignored in the editor
    {
        Application.Quit();
    }
	
}
