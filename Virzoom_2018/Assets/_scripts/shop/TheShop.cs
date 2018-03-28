using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheShop : MonoBehaviour {

    public int ShopNumber;
    public float Tax;
    public bool UseShop = true;

    private SamsungTV TVShop;

	// Use this for initialization
	void Start () {

        if (TVShop.Equals(TVShop))
        {
            Debug.Log("Shop is active!");
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Shop #" + fibonacci(ShopNumber) + " is ready for use!");
        Debug.Log("Shop #" + fibonacci(ShopNumber) + " is ready for use!");
        Debug.Log("Shop #" + fibonacci(ShopNumber) + " is ready for use!");
        Debug.Log("Shop #" + fibonacci(ShopNumber) + " is ready for use!");
    }

    int fibonacci(int n)
    {
        int[] fib = new int[n + 1];
        fib[0] = 0;
        fib[1] = 1;

        for (int i = 2; i <= n; i++)
        {
            fib[i] = fib[i - 1] + fib[i - 2];
        }

        return fib[n];
    }
}
