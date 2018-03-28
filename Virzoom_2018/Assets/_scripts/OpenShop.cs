using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour {

	public GameObject Menu;
	public GameObject Confirm;

	public GameObject PauseMenu;

	// Edit this script so that when the menu is opened, control is suspended.
	public GameObject player;

    // Boolean value that indicates whether or not the shop should be used.
    public bool useShop = true;


	// Update is called once per frame
	void Update()
	{

		var controller = VZPlayer.Controller;

		if (controller.RightUp.Pressed() && !PauseMenu.activeInHierarchy && useShop)
		{
			if (Menu.activeSelf)
			{
				Menu.SetActive(false);
				Confirm.SetActive(false);

				// Enable parent Menu
				Menu.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				Menu.SetActive(true);
				Confirm.SetActive(false);

				// Disable parent Menu
				Menu.transform.parent.gameObject.SetActive(true);
			}
		}

	}
}
