using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class sounds : MonoBehaviour {

	public string DamageEvent = "event:/Damage";
	FMOD.Studio.EventInstance DamageEv;
	FMOD.Studio.ParameterInstance DamageTypeParam;


	void Start () {
		DamageEv = FMODUnity.RuntimeManager.CreateInstance(DamageEvent);
		DamageEv.getParameter("Amount", out DamageTypeParam);
	}

	/*void OnTriggerEnter(Collider theCollision)

	void Update () {
		VZPlayer.Instance.Speed() >= 
	}*/
}
