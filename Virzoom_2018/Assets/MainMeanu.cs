using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMeanu : MonoBehaviour
{
	public GameObject player;
	public GameObject spawn;
    // Use this for initialization
    void Start()
    {
		player = GameObject.Find("VZPlayer");
		spawn = GameObject.Find("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
		player.transform.position = spawn.transform.position;
		player.transform.rotation = spawn.transform.rotation;
    }
}