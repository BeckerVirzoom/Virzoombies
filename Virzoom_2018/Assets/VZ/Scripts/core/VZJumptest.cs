using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VZJumptest : MonoBehaviour {
	/*this script is designed to allow the player to jump when on the ground and to 
	 make the force of gravity increase while the player is in the air*/


	public bool grounded; // is the bike grounded
	public float force; // multiplier for our jump
	public float gravity; // gravity value
	public float gravityFactor; // value subtracted from gravtiy
	WheelHit hit; // hit for the wheel raycast
	Rigidbody rb; // rigidbody to apply force to
	GameObject wheel; // front wheel of the bike
	VZController controller; // get the controller so we can access inputs from the bike

	// initialization
	void Start()
    {
        controller = VZPlayer.Controller; // get the controller
        rb = GetComponent<Rigidbody>(); // get the rigidbody of the bike
		wheel = GameObject.Find("Front_wheel_TextureReady:pCylinder83"); // get the front wheel of the bike
		Physics.gravity = Physics.gravity * 2;
	}

	// Update is called once per frame
	void Update()
	{
		grounded = wheel.GetComponent<WheelCollider>().GetGroundHit(out hit); // raycast to check if they bike is grounded
		if (SceneManager.GetActiveScene().name != "mainMenu")
		{
			if (grounded && (controller.RightButton.Down)) // if the player is on the ground and inputting the key for jumping, the player will jump
			{
				jump(); // call jump function
			}

			if (!grounded) // true when the bike is not grounded
			{
				gravity = Physics.gravity.y - gravityFactor; // subtract a value from the bike
				Physics.gravity = new Vector3(0, gravity, 0); // assign new gravity value
			}

			if (grounded && Physics.gravity.y != -9.81f) // true when the bike is not grounded and gravity is not default
			{
				Physics.gravity = new Vector3(0, -9.81f, 0); // reset gravity to default
			}
		}


	}

	private void jump() // function to make the player jump
	{
		rb.velocity = new Vector3(0, force, 0); // apply force to the bike
    }
}
