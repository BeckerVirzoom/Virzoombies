using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRandomScene : MonoBehaviour {

    int currentScene;
    public int numberOfScenes = 2;

    private RandomChunkLoader CL;

    private void Awake()
    {
        CL = GameObject.Find("Loader").GetComponent<RandomChunkLoader>();
    }

    // Use this for initialization
    void Start () {

        currentScene = SceneManager.GetActiveScene().buildIndex;

    }
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            /*other.GetComponent<PlayerController>().enabled = false;
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;

            int newSceneIndex = currentScene;

            while (newSceneIndex == currentScene)
            {
                newSceneIndex = Random.Range(0, numberOfScenes);
                SceneManager.LoadScene(newSceneIndex);
            }*/

        }
        StartCoroutine(CL.LoadLevel());
    }
}
