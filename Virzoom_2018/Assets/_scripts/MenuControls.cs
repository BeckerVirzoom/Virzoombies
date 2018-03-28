using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {

    public GameObject Menu;
    public GameObject Options;
    public GameObject Controls;

	// Disable shop menu
	public GameObject Shop;

	// Edit this script so that when the menu is opened, control is suspended.
	public GameObject player;


    void Start()
    {
        
    }


    // Update is called once per frame
    void Update () {

		var controller = VZPlayer.Controller;

		if (controller.RightUp.Pressed())
        {
            if (Menu.activeSelf)
            {
                Menu.SetActive(false);
                Options.SetActive(false);
                Controls.SetActive(false);

				Shop.SetActive(false);

				// Enable parent Menu
				Menu.transform.parent.gameObject.SetActive(false);
			}
            else
            {
                Menu.SetActive(true);
                Options.SetActive(false);
                Controls.SetActive(false);

				// Disable parent Menu
				Menu.transform.parent.gameObject.SetActive(true);
			}
        }

    }
}
