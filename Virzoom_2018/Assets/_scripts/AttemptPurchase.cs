using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttemptPurchase : MonoBehaviour {

	public int itemNumber;
	public GameObject confirmationMenu;
	public HandlePurchaseID pID;

	void Start()
	{
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(OpenConfirmation);
	}

	void OpenConfirmation()
	{
		pID.setPurchaseID(itemNumber);
		confirmationMenu.SetActive(true);
	}
}
