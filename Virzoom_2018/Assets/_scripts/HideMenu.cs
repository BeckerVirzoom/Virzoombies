using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideMenu : MonoBehaviour {

    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(RemoveMenu);
    }

    void RemoveMenu()
    {
        transform.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
