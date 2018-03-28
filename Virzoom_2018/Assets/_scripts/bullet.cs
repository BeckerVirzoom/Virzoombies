using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bullet: MonoBehaviour
{
	
	private Rigidbody bulletrb;
	// Use this for initialization
	void Start()
	{

		transform.position = new Vector3(0, 0, 0);
		
		
		bulletrb = GetComponent<Rigidbody>();
		print(transform.position.z);
	}

	// Update is called once per frame
	void Update()
	{
		var controller = VZPlayer.Controller; 
		if (controller.RightButton.Pressed())
		{
			bulletrb.AddForce(controller.InputSpeed * 2, 0, 0);

		}
		
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (gameObject.tag != "Player")
		{
			transform.position = new Vector3(0, 0, 0);
		};
	}
}
