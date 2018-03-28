using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPHGauge : MonoBehaviour {

    // The object that is to be rotated
    public GameObject arrow;

    // The maximum rotation of the arrow
    public float maxRotation;
    // The minimum rotation of the arrow
    public float minRotation;

    // The maximum speed the player can travel
    private float maxSpeed;
    // The current speed the player is traveling at
    private float currentSpeed;

	// Use this for initialization
	void Start () {

        maxSpeed = VZPlayer.Instance.MaxSpeed;

        arrow.transform.eulerAngles = new Vector3(arrow.transform.eulerAngles.x, arrow.transform.eulerAngles.y, minRotation);

    }
	
	// Update is called once per frame
	void Update () {

        // The new rotation value
        float newRotate = 0.0f;

        // Get the current speed the player is traveling
        currentSpeed = VZPlayer.Instance.Speed();

        // The player's speed converted to a percent
        float percentSpeed = (currentSpeed / maxSpeed);

        // Calculate the rotation based off of the player's speed percentage
        // maxRot + minRot times the speed gives a value between 0 and | minRot + maxRot |
        // This value is decreased by minRot to keep it in the range of minRot and maxRot
        newRotate = ((percentSpeed * (Mathf.Abs(maxRotation) + Mathf.Abs(minRotation))) - Mathf.Abs(minRotation)) * -1;

        // Set the arrow's rotation
        arrow.transform.eulerAngles = new Vector3(arrow.transform.eulerAngles.x, arrow.transform.eulerAngles.y, newRotate);

    }
}
