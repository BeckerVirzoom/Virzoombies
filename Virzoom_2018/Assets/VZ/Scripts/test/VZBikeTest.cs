//***********************************************************************
// Copyright 2014 VirZOOM
//***********************************************************************

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class VZBikeTest : MonoBehaviour
{
   TextMesh mText;

   void Start()
   {
      // Lookup text objects
      mText = GetComponent<TextMesh>();
   }

   void Update()
   {
      var controller = VZPlayer.Controller;

      mText.text = 
         "Type: " + TypeText(controller.BikeType(), controller.BikeBetaVersion()) + "\n" +
         "Connected: " + controller.IsBikeConnected() + "\n" +
         "SenderAddress: " + controller.BikeSender() + "\n" +
         "HeartRate: " + controller.HeartRate() + "\n" +
         "BatteryVolts: " + controller.BatteryVolts() + "\n" +
         "Speed: " + controller.InputSpeed + "\n" +
         "Resistance: " + controller.UncalibratedResistance() + "\n" +
         "LeftGrip: " + GripText(controller.LeftButton.Down, controller.DpadUp.Down, controller.DpadDown.Down, controller.DpadLeft.Down, controller.DpadRight.Down) + "\n" +
         "RightGrip: " + GripText(controller.RightButton.Down, controller.RightUp.Down, controller.RightDown.Down, controller.RightLeft.Down, controller.RightRight.Down);
   }

   string TypeText(int type, int version)
   {
      if (type < 0)
         return "none";
      else if (type == 0)
         return "unsupported bike";
      else if (type == 1)
         return "alpha bike";
      else if (type == 2)
      {
         if (version == 2)
            return "bike sensor";
         else
            return "beta bike";
      }
      else
         return "unknown";
   }

   string GripText(bool trigger, bool up, bool down, bool left, bool right)
   {
      string text = "";

      if (trigger)
         text += "trigger ";
      if (up)
         text += "up ";
      if (down)
         text += "down ";
      if (left)
         text += "left ";
      if (right)
         text += "right ";

      return text;
   }
}
