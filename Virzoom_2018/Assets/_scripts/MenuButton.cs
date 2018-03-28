using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(MainMenu);
    }

    void MainMenu()
    {
        GameObject player = GameObject.Find("VZPlayer");
        Destroy(player);
        SceneManager.LoadScene(0);
    }
}
