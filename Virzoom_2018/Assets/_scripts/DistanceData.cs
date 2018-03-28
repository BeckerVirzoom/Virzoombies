using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DistanceData : ScriptableObject {


    // Distance player has traveled
    public float distance;

    // Constructor for DistanceData
    public DistanceData()
    {
        distance = 0.0f;
    }

    // Returns the current distance traveled
    public float GetDistanceTraveled()
    {
        return distance;
    }


    // Resets the current distance traveled to zero
    public void ResetDistanceTraveled()
    {
        distance = 0.0f;
    }


    // Saves the distance to a binary file
    public void Save()
    {
        SaveLoadManager.SaveDistances(this);
    }


    // Loads the distance from a binary file
    public void Load()
    {
        distance = (SaveLoadManager.LoadDistances())[0];
    }
}
