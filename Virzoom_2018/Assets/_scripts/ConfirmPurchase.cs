using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPurchase : MonoBehaviour {

	public PowerUpList pul;
	
	public GameObject confirmationMenu;
	public GameObject shopMenu;

	private int PurchaseID;


	void Start()
	{
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Purchase);

	}

	void Purchase()
	{
		// Update PurchaseID to that of currently selected button
		PurchaseID = transform.gameObject.GetComponent<HandlePurchaseID>().getPurchaseID();

		// Attempt to purchase item
		pul.PurchaseItem(PurchaseID);

		// Close confirmation and open shop menu
		confirmationMenu.SetActive(false);
		shopMenu.SetActive(true);

		// Reenable input for menu
		shopMenu.GetComponent<VirZoomInputModule>().enabled = true;
	}
}
