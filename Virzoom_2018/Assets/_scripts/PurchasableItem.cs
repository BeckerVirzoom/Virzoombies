using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "PurchasableItem")]
public class PurchasableItem : ScriptableObject {

	public string itemName = "ObjectName";
	public float cost = 100.0f;
	public bool purchased = false;

	// This method is called when an item is purchased.
	// This method is made so different items can implement this same function differently.
	public virtual int OnPurchase()
	{
		return 0;
	}
}
