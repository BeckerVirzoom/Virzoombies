using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpList : MonoBehaviour
{

	public List<PurchasableItem> items;
	public SkinnedMeshRenderer pMesh;
	private GameManager gm;

	void Start()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		for(int i=0; i<items.Count; i++)
		{
			items[i].purchased = false;
		}
	}

	public void PurchaseItem(int i)
	{
		// Check if i is a valid index

		if(i < items.Count && i >= 0)
		{
			// Check if item has not been purchased
			if (items[i].purchased == false)
			{
				if (gm.getDistance() >= items[i].cost)
				{
					items[i].purchased = true;

					// Subtract points from GameManager
					gm.setDistance(gm.getDistance() - items[i].cost);

					// Figure out which type of Purchasable Item it is

					// If purchased item is a PaintUpgrade
					if(items[i].OnPurchase() == 1)
					{
						Material newMat = ((PaintUpgrade)items[i]).newMat;

						pMesh.material = newMat;
					}

					Debug.Log(gm.getDistance());
				}
				else
				{
					Debug.Log("Insufficient Funds");
				}
			}
			else
			{
				Debug.Log("Already purchased");
			}
		}
	}
}

