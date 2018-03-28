using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableDisableInput : MonoBehaviour {

	public VirZoomInputModule menuControls;

	void Start()
	{
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(InputFix);
	}

	void InputFix()
	{
		//switches control schemes to allow for better menu navigation
		menuControls.enabled = (!menuControls.enabled);
	}
}
