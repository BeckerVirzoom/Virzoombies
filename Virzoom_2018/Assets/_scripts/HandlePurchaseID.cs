using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePurchaseID : MonoBehaviour
{

	int purchaseID = -1;

	public void setPurchaseID(int i)
	{
		purchaseID = i - 1;
	}

	public int getPurchaseID()
	{
		return purchaseID;
	}
}
