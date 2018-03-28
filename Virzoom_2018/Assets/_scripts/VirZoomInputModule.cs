using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;

public class VirZoomInputModule : MonoBehaviour {

	public EventSystem eventSystem;
	public List<Button> buttons;

	private int currentSelected;
	public string UIEvent = "event:/UI";
	FMOD.Studio.EventInstance UIEv;
	FMOD.Studio.ParameterInstance UITypeParam;
	// Use this for initialization
	void Start () {
		UIEv = FMODUnity.RuntimeManager.CreateInstance(UIEvent);
		UIEv.getParameter("Type", out UITypeParam);
		currentSelected = 0;

	}
	
	// Update is called once per frame
	void Update () {

		// Gets reference to controller
		var controller = VZPlayer.Controller;

		// If either of the down buttons are pressed
		if (controller.DpadDown.Pressed() || controller.RightDown.Pressed())
		{
			// Add to currently selected index
			currentSelected++;

			// Menu SFX
			// Play the scroll sound here, BEFORE the button is highlighted
			UITypeParam.setValue(3f);
			UIEv.start();
		}
		// Otherwise, check if either of the up buttons are pressed
		else if (controller.DpadUp.Pressed() || controller.RightUp.Pressed())
		{
			// Subtract from currently selected index
			currentSelected--;

			// Menu SFX
			// Play the scroll sound here, BEFORE the button is highlighted
			UITypeParam.setValue(3f);
			UIEv.start();
		}

		// If index is too large, set it to the first element in the list
		if(currentSelected > buttons.Count-1)
		{
			currentSelected = 0;
		}
		// If index is too small, set it to the last element in the list
		else if(currentSelected < 0)
		{
			currentSelected = buttons.Count - 1;
		}

		
		// Make the selected element highlighted in the event system
		eventSystem.SetSelectedGameObject(buttons[currentSelected].gameObject);

		// Check if confirm button is pressed
		if (controller.DpadRight.Pressed() || controller.RightRight.Pressed())
        {
            // Menu SFX
            // Play the confirm sound here, BEFORE the button is clicked
			UITypeParam.setValue(0f);
			UIEv.start();

            // Click the current selected button
            buttons[currentSelected].onClick.Invoke();
		}

	}

	// When enabled, default the selection to the first button
	private void OnEnable()
	{
		currentSelected = 0;
		eventSystem.SetSelectedGameObject(buttons[currentSelected].gameObject);
	}
}
