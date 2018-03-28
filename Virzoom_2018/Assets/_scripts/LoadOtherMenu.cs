using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOtherMenu : MonoBehaviour {

    public GameObject MenuToLoad;

    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(LoadMenu);
    }

    void LoadMenu()
    {
        MenuToLoad.SetActive(true);
        transform.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
