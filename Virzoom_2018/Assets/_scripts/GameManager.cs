using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class GameManager : MonoBehaviour
{


    // Static object used to prevent other GameManagers from loading
    public static GameManager GM;

    // Name of player object to reference
    public string playerName = "Player";

    // Reference to the player object
    private GameObject player;

    // Save data which holds all data relating to distance traveled
    private DistanceData distance;

    // The current loaded scene
    private int currentLoadedScene;

    // The previous loaded scene
    private int lastLoadedScene;

	// Determines the gender of the player
	public static bool playerIsMale;



    void Awake()
    {
        // Prevents GameObject from being destroyed when a new scene is loaded

        if (GM == null)
        {
            // Allows GameObject to persist through scenes
            DontDestroyOnLoad(transform.gameObject);

            // Sets static object to this, preventing more GameManagers from being created
            GM = this;

        }
        else
        {
            // Destroys any duplicates of this GameObject
            Destroy(this.gameObject);
            Debug.Log("Destroyed");
        }

        // Set the last loaded scene to 0, as the game will have just been loaded
        lastLoadedScene = 0;

        // Set the current loaded scene to the index of the current scene
        currentLoadedScene = SceneManager.GetActiveScene().buildIndex;

        // Initializes DistanceData
        distance = (DistanceData)ScriptableObject.CreateInstance("DistanceData");
    }


    // For as long as this script is enabled, detects if a scene is loaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }


    // If script is disabled, stops detecting whether or not a scene is loaded
    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }


    // Method is called whenever a scene is loaded
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the value of previous loaded scene
        lastLoadedScene = currentLoadedScene;

        // Update the value of the current loaded scene
        currentLoadedScene = SceneManager.GetActiveScene().buildIndex;

        getPlayerReference();

        // Debugging saving
        distance.distance += 10000.0f;
        Debug.Log(distance.distance);

        //countDistance();
    }


	// Destroys the player and resets the game when it is called
	public static void ResetGame()
	{
		Destroy(VZPlayer.Instance);

		GameObject player = GameObject.Find("VZPlayer");
		Destroy(player);

		VZPlayer.Reinitialize();

		SceneManager.LoadScene(0);
	}


    // Attempts to obtain reference to the player object, but only in valid scenes
    private void getPlayerReference()
    {
        // Checks if the current scene is a level, and thus, has a player in it
        if (currentLoadedScene > 1)
        {
            // Locates the player in the current scene. This is required for counting distance
            player = GameObject.Find(playerName);
        }
        else
        {
            // Releases the reference to the player. This is done to prevent the game from counting distance in certain scenes
            player = null;
        }
    }
	
	
	public float getDistance(){
		return distance.distance;
	}


	public void setDistance(float d)
	{
		distance.distance = d;
	}


	// Saves distance data to binary file
	public IEnumerator SaveDistanceData()
    {
        distance.Save();
        Debug.Log("Saved distance: " + distance.distance);
        yield return null;
    }
    
    // Loads distance data from binary file
    public IEnumerator LoadDistanceData()
    {
        distance.Load();
        Debug.Log("Loaded distance: " + distance.distance);
        yield return null;
    }
}
