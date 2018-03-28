using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn_Start : MonoBehaviour {
    public GameObject Player;
	
    private void Awake()
    {
        Player = GameObject.Find("VZPlayer");
        Player.transform.position = transform.position;
		Player.transform.rotation = transform.rotation;
    } 
}
