using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {
    

    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
        // NOTE: This code is ignored in the editor, but does work.
        Application.Quit();
    }

}
