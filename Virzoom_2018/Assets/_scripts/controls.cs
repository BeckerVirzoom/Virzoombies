using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour {
	// need an input manager for the inputs of the bike
	// cant use get on the button binding
	// Must use a custom enumerator to get the proper buttons of the bike
	// Use this for initialization
	VZController controller;
	void Awake ()
	{
		controller = VZPlayer.Controller;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
