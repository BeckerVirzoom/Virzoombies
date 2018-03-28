using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotdown : MonoBehaviour {

	GameObject Player;
	float distance;

	public string ShootEvent = "event:/Shoot";
	FMOD.Studio.EventInstance ShootEv;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		ShootEv = FMODUnity.RuntimeManager.CreateInstance (ShootEvent);
	}



	//Trigger Box that allows Zombie to be killed. Press button while in Trigger to kill zombie.
    void OnTriggerStay(Collider other)
    {
		distance = Vector3.Distance(other.transform.position, gameObject.transform.position);
		

        if (VZPlayer.Controller.RightDown.Pressed() && distance < 30 && other.tag == "Player")
        {
            // Character Sounds
            // Play Zombie shot success sound here
			ShootEv.start();
            transform.GetComponentInParent<Fat_ZombieAI>().isDead = true;
        }
    }
}
