using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWinMessage : MonoBehaviour
{

	// Message to display when the player reaches the end of the level
	public Image win;

	public BloodSplatterUI damage;

	// Used to disable taking damage
	public DeathScript death;

	// Use so that the win state can only trigger once
	private bool won = false;


	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("EndOfLevel"))
		{
			win.gameObject.SetActive(true);

			if (!won) 
				StartCoroutine(ResetGame());
		}
	}


	public IEnumerator ResetGame()
	{
		won = true;

		win.gameObject.GetComponent<Animator>().SetTrigger("start");

		damage.ResetUI();

		death.enabled = false;

		yield return new WaitForSeconds(10.0f);

		GameManager.ResetGame();
	}

}
