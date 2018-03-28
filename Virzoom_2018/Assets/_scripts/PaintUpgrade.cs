using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "PaintUpgrade")]

public class PaintUpgrade : PurchasableItem {

	public Material newMat;

	// When purchased, a paint upgrade attempts to find the player, and changes the material for the bike.
	public override int OnPurchase()
	{
		return 1;
	}
}
