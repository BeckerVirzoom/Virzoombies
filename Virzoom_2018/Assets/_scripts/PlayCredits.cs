using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCredits : MonoBehaviour
{

	public GameObject credits;

	public void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Working");
			credits.transform.parent.gameObject.SetActive(true);
			credits.GetComponent<Animator>().SetTrigger("PlayCredits");

		}

	}
}