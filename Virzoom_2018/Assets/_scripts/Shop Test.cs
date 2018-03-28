using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTest : MonoBehaviour {
	int points = 1000;
	VZController controller;
	// Use this for initialization
	void Start () {
		controller = VZPlayer.Controller;

	}
	
	// Update is called once per frame
	void Update () {
		if ((controller.RightLeft.Down))
		{

		}
	}

}
