using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	/// <summary>
	/// WIP script for controlling the healthbar of the player
	/// </summary>
	public int health; // health of the player
	public GameObject barObject; // the healthbar object we will manipulate
	public Animator healthAnimator; // animator of the mask object that we use to make the healthbar blink
	public Sprite [] healthBar_textures; // array of textures whoch hold all the different health bar states

	// Use this for initialization
	void Start ()
	{

		barObject = this.gameObject;
		//barObject.GetComponent<Image>().fillAmount = 1;
		healthAnimator = GetComponent<Animator>();
		health = 3;
		healthAnimator.SetInteger("Health", health);
		barObject.GetComponent<Image>().sprite = healthBar_textures[health];
	}

	public void updateHealth (int currentHealth)
	{
		healthAnimator.SetInteger("Health", currentHealth);
		//barObject.GetComponent<Image>().fillAmount = health / 3;
		barObject.GetComponent<Image>().sprite = healthBar_textures[currentHealth];
	}
	/*
	public void healthUp()
	{
		if (health != 3)
		{
			health += 1;
			updateHealth();
		}
	}

	public void healthDown()
	{
		if (health != 0)
		{
			health -= 1;
			updateHealth();
		}
	}
	*/
}
