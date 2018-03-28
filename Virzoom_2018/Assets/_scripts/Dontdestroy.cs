using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class GameControl : MonoBehaviour {
	public static GameControl GC;
	void Awake()
	{
		if(GC == null)
		{
			DontDestroyOnLoad(this.gameObject);
			GC = this;
		}
		else if (GC != null)
		{
			Destroy(this.gameObject);
		}

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	[Serializable]
	class PlayerPrefs
	{

	}
}
