using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSplatterUI : MonoBehaviour {

    public static int maxHealth = 3;

    public Image blood;
    public Image glow;

	public Image gameOver;

    // Will be private eventually
    public bool isGlowing = false;


    public void Start(){

        // Enable blood display
        blood.gameObject.SetActive(true);
        // Disable glow
        glow.gameObject.SetActive(false);

        // Set blood to transparent. 
        blood.color = new Color(1, 1, 1, 0f);

        //UpdateUI(1);

    }

    public void Update()
    {
        // If component should flash
        if (isGlowing)
            // Enable glow
            glow.gameObject.SetActive(true);

        else
            //Disable glow
            glow.gameObject.SetActive(false);
    }

    // Update UI should be called by the damage function whenever the player takes or heals damage
    public void UpdateUI(int currentHealth)
    {
        // Sense 0.0f is full transparency, the alpha should be subtracted from 1.0 to ensure it fades from 0 to 1.
        blood.color = new Color(1, 1, 1, (1.0f - ((float)((float)currentHealth / (float)maxHealth))));

        // If health is low, start glowing
        if (currentHealth <= 1)
            isGlowing = true;

        // Otherwise, disable the glow
        else
            isGlowing = false;
    }


	public void DisplayGameOver()
	{
		gameOver.gameObject.SetActive(true);
		gameOver.gameObject.GetComponent<Animator>().SetTrigger("start");
	}


	public void ResetGameOver()
	{
		gameOver.gameObject.SetActive(false);
	}


	public void ResetUI()
	{
		// Enable blood display
		blood.gameObject.SetActive(true);

		// Disable glow
		glow.gameObject.SetActive(false);
		isGlowing = false;

		// Set blood to transparent. 
		blood.color = new Color(1, 1, 1, 0f);
	}

}
