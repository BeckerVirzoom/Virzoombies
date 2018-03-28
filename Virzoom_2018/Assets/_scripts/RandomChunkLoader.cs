using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomChunkLoader : MonoBehaviour {

    // Static object used to prevent objects from loading
    public static RandomChunkLoader CL;

    // The number of levels to save in each chunk
    public int chunkSize;

    // The index of the next scene to load
    private int toLoad;


    // The list that contains the indexes to load in the order to load them
    private List<int> SceneIndexes = new List<int>(0);


    void Awake()
    {
        // Prevents GameObject from being destroyed when a new scene is loaded

        if (CL == null)
        {
            // Allows GameObject to persist through scenes
            DontDestroyOnLoad(transform.gameObject);

            // Sets static object to this, preventing more RandomChunkLoaders from being created
            CL = this;
        }
        else
        {
            // Destroys any duplicates of this GameObject
            Destroy(this.gameObject);
        }

        // Reset list
        CreateMapIndexes();

    }


    // Use this for initialization
    void Start () {


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
        // Check if list has more than zero items
        if(SceneIndexes.Count == 0)
        {
            // Reset list
            CreateMapIndexes();

            // Make the next level to load be the hub
            toLoad = 1;
        }
        else
        {
            // Index of level to load
            toLoad = SceneIndexes[0];

            // Delete first element in Scene Index
            SceneIndexes.RemoveAt(0);
        }

    }


    // Fills the map with 5 randomly chosen level indexes
    private void CreateMapIndexes()
    {
        SceneIndexes = new List<int>(chunkSize);

        for (int i=0; i<chunkSize; i++)
        {
            // Generates a random integer between 1 and the maximum number of scenes, inclusive
            int randomScene = Random.Range(2, (SceneManager.sceneCountInBuildSettings));

            // Add random integer to Scene Index
            SceneIndexes.Insert(i, randomScene);
        }
    }


    public IEnumerator LoadLevel()
    {
        SceneManager.LoadScene(toLoad);

        yield return null;
    }
}
