using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIApplyOverlayShader : MonoBehaviour {

    // Use this for initialization
    void Start () {

        // Overwrites the existing shader with the Render on Top shader.
        GetComponent<Image>().material.shader = Shader.Find("Custom/RenderOnTop");
    }
	
}
