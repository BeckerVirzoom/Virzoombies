using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour {

    public EventSystem es;
    public GameObject selectedObject;

    private bool buttonSelected = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxisRaw("Vertical") != 0 && !buttonSelected)
        {
            es.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }

	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
