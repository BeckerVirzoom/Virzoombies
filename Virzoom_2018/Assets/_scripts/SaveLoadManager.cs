using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager{

    public static void SaveDistances(DistanceData dd)
    {
        // Allows distance data to be formatted to a binary file
        BinaryFormatter bf = new BinaryFormatter();

        // Prepares output
        FileStream stream = new FileStream(Application.persistentDataPath + "/distance.sav", FileMode.Create);

        // Puts relevant information in distance save class
        DistanceSave data = new DistanceSave(dd);
        
        // Serializes data for save
        bf.Serialize(stream, data);

        // Closes file
        stream.Close();
    }


    public static float[] LoadDistances()
    {
        // Checks if it is possible to load a file
        if (File.Exists(Application.persistentDataPath + "/distance.sav"))
        {
            
            // Allows distance data to be formatted from a binary file
            BinaryFormatter bf = new BinaryFormatter();
            
            // Prepares input
            FileStream stream = new FileStream(Application.persistentDataPath + "/distance.sav", FileMode.Open);
            
            // Converts information in save file to a new distance save (requires cast)
            DistanceSave data = (DistanceSave)bf.Deserialize(stream);
            
            // Closes file
            stream.Close();
            
            // Returns the loaded information
            return data.info;
        }

        // Returns null if file cannot be loaded
        return null;
    }

	
}


// The serializable save data
[Serializable]
public class DistanceSave
{
    // Array that will hold distance traveled. Set as an array in case more information needs to be saved.
    public float[] info;

    public DistanceSave(DistanceData dd)
    {
        info = new float[1];
        info[0] = dd.distance;
    }

}
