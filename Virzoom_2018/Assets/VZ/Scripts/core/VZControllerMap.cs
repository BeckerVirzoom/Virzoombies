//***********************************************************************
// Copyright 2014 VirZOOM
//***********************************************************************

using UnityEngine;
using System;

public class VZControllerMap
{
   public enum ControlType
   {
      Key,
      Axis
   };

   public class Control
   {
      public ControlType Type = ControlType.Key;
      public string Id = "None";
      public float Multiplier = 1;
      public float Offset = 0;
      public KeyCode Code;

      public virtual bool GetBool()
      {
         if (Type == ControlType.Key)
            return Input.GetKey(Code);
         else
            return Input.GetAxis(Id) * Multiplier + Offset > 0;
      }

      public virtual float GetFloat()
      {
         if (Type == ControlType.Key)
            return Input.GetKey(Code) ? 1 : 0;
         else
            return Input.GetAxis(Id) * Multiplier + Offset;
      }

      public void Finalize(int joystick)
      {
         Id = string.Format(Id, joystick);

         if (Type == ControlType.Key)
            Code = (KeyCode)Enum.Parse(typeof(KeyCode), Id);
      }
   }

#if VZ_GAME && !UNITY_ANDROID && (!UNITY_PS4 || UNITY_EDITOR)
   class ViveControl : Control
   {
      public enum Action 
      {
         Null,
         LeftTrigger,
         RightTrigger,
      }

      Action mAction;

      public ViveControl(Action action)
      {
         mAction = action;
      }

      public override bool GetBool()
      {
         return GetFloat() != 0 ? true : false;
      }

      public override float GetFloat()
      {
         // TouchPos.y is 0 at top and 1 at bottom
         switch (mAction)
         {
            case Action.LeftTrigger:
            {
               var deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.First);
               return (deviceIndex != -1 && SteamVR_Controller.Input(deviceIndex).GetPress(SteamVR_Controller.ButtonMask.Touchpad)) ? 1 : 0;
            }
            case Action.RightTrigger:
            {
               var deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.First);
               return (deviceIndex != -1 && SteamVR_Controller.Input(deviceIndex).GetPress(SteamVR_Controller.ButtonMask.Trigger)) ? 1 : 0;
            }
            default:
            {
               return 0;
            }
         }
      }
   }

   class TouchControl : Control
   {
      public enum Action 
      {
         Null,
         LeftTrigger,
         RightTrigger,
      }

      Action mAction;

      public TouchControl(Action action)
      {
         mAction = action;
      }

      public override bool GetBool()
      {
         return GetFloat() != 0 ? true : false;
      }

      public override float GetFloat()
      {
         switch (mAction)
         {
            case Action.LeftTrigger:
            {
               return OVRInput.Get(OVRInput.Button.One) ? 1 : 0;
            }
            case Action.RightTrigger:
            {
               return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            }
            default:
            {
               return 0;
            }
         }
      }
   }
#endif

#if VZ_GAME && UNITY_PS4 && !UNITY_EDITOR
   class MoveControl : Control
   {
      public enum Action 
      {
         Null,
         LeftTrigger,
         RightTrigger,
      }

      Action mAction;

      public MoveControl(Action action)
      {
         mAction = action;
      }

      public override bool GetBool()
      {
         return GetFloat() != 0 ? true : false;
      }

      public override float GetFloat()
      {
         switch (mAction)
         {
            case Action.LeftTrigger:
            {
               // TODO
               return 0;
            }
            case Action.RightTrigger:
            {
               // TODO
               return 0;
            }
            default:
            {
               return 0;
            }
         }
      }
   }
#endif

#if VZ_GAME && UNITY_ANDROID && !UNITY_EDITOR && !VZ_SNAPDRAGONVR
   class MobileControl : Control
   {
      public enum Action 
      {
         Forward,
         Reverse,
         LeftTrigger,
         RightTrigger,
         Null
      }

      Action mAction;

      public MobileControl(Action action)
      {
         mAction = action;
      }

      public override bool GetBool()
      {
         return GetFloat() != 0 ? true : false;
      }

      public override float GetFloat()
      {
         // TouchPos.y is 0 at top and 1 at bottom
         switch (mAction)
         {
# if !VZ_GEARVR
            case Action.Forward:
            {
               return GvrController.IsTouching ? (0.75f - GvrController.TouchPos.y) * 1.466f : 0;
            }
            case Action.Reverse:
            {
               return GvrController.IsTouching ? (GvrController.TouchPos.y - 0.75f) * 4.4f : 0;
            }
            case Action.LeftTrigger:
            {
               return GvrController.AppButton ? 1 : 0;
            }
            case Action.RightTrigger:
            {
               return GvrController.ClickButton ? 1 : 0;
            }
# else
            case Action.Forward:
            {
               return OVRInput.Get(OVRInput.RawTouch.RTouchpad) ? (OVRInput.Get(OVRInput.RawAxis2D.RTouchpad, OVRInput.Controller.RTrackedRemote).y + 0.5f) * 0.733f : 0;
            }
            case Action.Reverse:
            {
               return OVRInput.Get(OVRInput.RawTouch.RTouchpad) ?  (-0.5f - OVRInput.Get(OVRInput.RawAxis2D.RTouchpad, OVRInput.Controller.RTrackedRemote).y) * 2.2f : 0;
            }
            case Action.LeftTrigger:
            {
               return OVRInput.Get(OVRInput.RawButton.RIndexTrigger) ? 1 : 0;
            }
            case Action.RightTrigger:
            {
               return OVRInput.Get(OVRInput.RawButton.RTouchpad) ? 1 : 0;
            }
# endif
            default:
            {
               return 0;
            }
         }
      }
   }
#endif

   public class Controller
   {
      public string Name;
      public string Description;
      public string Icons;
      public Control LeftUp;
      public Control LeftLeft;
      public Control LeftRight;
      public Control LeftDown;
      public Control RightUp;
      public Control RightLeft;
      public Control RightRight;
      public Control RightDown;
      public Control LeftTrigger;
      public Control RightTrigger;
      public Control LeanLeft;
      public Control LeanRight;
      public Control LeanForward;
      public Control LeanBack;
      public Control LookLeft;
      public Control LookRight;
      public Control LookUp;
      public Control LookDown;
      public Control Forward;
      public Control Reverse;
   }

   public Controller[] Controllers;

   Controller FindController(string description)
   {
      foreach (var controller in Controllers)
      {
         if (controller.Description == description)
            return controller;
      }
      
      return null;
   }

   public Controller PickController(string description)
   {
      string[] joysticks = Input.GetJoystickNames();
      int joyNum = 0;
      Controller controller = null;

#if VZ_GAME && !UNITY_ANDROID && (!UNITY_PS4 || UNITY_EDITOR)
      if (description == "Vive")
      {
         controller = new Controller();
         controller.Name = "Vive";
         controller.Description = "Vive";
         controller.Icons = "buttons_vive";
         controller.LeftUp = new ViveControl(ViveControl.Action.Null);
         controller.LeftLeft = new ViveControl(ViveControl.Action.Null);
         controller.LeftRight = new ViveControl(ViveControl.Action.Null);
         controller.LeftDown = new ViveControl(ViveControl.Action.Null);
         controller.RightUp = new ViveControl(ViveControl.Action.Null);
         controller.RightLeft = new ViveControl(ViveControl.Action.Null);
         controller.RightRight = new ViveControl(ViveControl.Action.Null);
         controller.RightDown = new ViveControl(ViveControl.Action.Null);
         controller.LookLeft = new ViveControl(ViveControl.Action.Null);
         controller.LookRight = new ViveControl(ViveControl.Action.Null);
         controller.LookUp = new ViveControl(ViveControl.Action.Null);
         controller.LookDown = new ViveControl(ViveControl.Action.Null);
         controller.LeanLeft = new ViveControl(ViveControl.Action.Null);
         controller.LeanRight = new ViveControl(ViveControl.Action.Null);
         controller.LeanForward = new ViveControl(ViveControl.Action.Null);
         controller.LeanBack = new ViveControl(ViveControl.Action.Null);
         controller.LeftTrigger = new ViveControl(ViveControl.Action.LeftTrigger);
         controller.RightTrigger = new ViveControl(ViveControl.Action.RightTrigger);
         controller.Forward = new ViveControl(ViveControl.Action.Null);
         controller.Reverse = new ViveControl(ViveControl.Action.Null);
      }
      else if (description == "Touch")
      {
         controller = new Controller();
         controller.Name = "Touch";
         controller.Description = "Touch";
         controller.Icons = "buttons_RiftTouch";
         controller.LeftUp = new TouchControl(TouchControl.Action.Null);
         controller.LeftLeft = new TouchControl(TouchControl.Action.Null);
         controller.LeftRight = new TouchControl(TouchControl.Action.Null);
         controller.LeftDown = new TouchControl(TouchControl.Action.Null);
         controller.RightUp = new TouchControl(TouchControl.Action.Null);
         controller.RightLeft = new TouchControl(TouchControl.Action.Null);
         controller.RightRight = new TouchControl(TouchControl.Action.Null);
         controller.RightDown = new TouchControl(TouchControl.Action.Null);
         controller.LookLeft = new TouchControl(TouchControl.Action.Null);
         controller.LookRight = new TouchControl(TouchControl.Action.Null);
         controller.LookUp = new TouchControl(TouchControl.Action.Null);
         controller.LookDown = new TouchControl(TouchControl.Action.Null);
         controller.LeanLeft = new TouchControl(TouchControl.Action.Null);
         controller.LeanRight = new TouchControl(TouchControl.Action.Null);
         controller.LeanForward = new TouchControl(TouchControl.Action.Null);
         controller.LeanBack = new TouchControl(TouchControl.Action.Null);
         controller.LeftTrigger = new TouchControl(TouchControl.Action.LeftTrigger);
         controller.RightTrigger = new TouchControl(TouchControl.Action.RightTrigger);
         controller.Forward = new TouchControl(TouchControl.Action.Null);
         controller.Reverse = new TouchControl(TouchControl.Action.Null);
      }
#endif

#if VZ_GAME && UNITY_PS4 && !UNITY_EDITOR
      if (description == "Move")
      {
         controller = new Controller();
         controller.Name = "Move";
         controller.Description = "Move";
         controller.Icons = "buttons_vive";
         controller.LeftUp = new MoveControl(MoveControl.Action.Null);
         controller.LeftLeft = new MoveControl(MoveControl.Action.Null);
         controller.LeftRight = new MoveControl(MoveControl.Action.Null);
         controller.LeftDown = new MoveControl(MoveControl.Action.Null);
         controller.RightUp = new MoveControl(MoveControl.Action.Null);
         controller.RightLeft = new MoveControl(MoveControl.Action.Null);
         controller.RightRight = new MoveControl(MoveControl.Action.Null);
         controller.RightDown = new MoveControl(MoveControl.Action.Null);
         controller.LookLeft = new MoveControl(MoveControl.Action.Null);
         controller.LookRight = new MoveControl(MoveControl.Action.Null);
         controller.LookUp = new MoveControl(MoveControl.Action.Null);
         controller.LookDown = new MoveControl(MoveControl.Action.Null);
         controller.LeanLeft = new MoveControl(MoveControl.Action.Null);
         controller.LeanRight = new MoveControl(MoveControl.Action.Null);
         controller.LeanForward = new MoveControl(MoveControl.Action.Null);
         controller.LeanBack = new MoveControl(MoveControl.Action.Null);
         controller.LeftTrigger = new MoveControl(MoveControl.Action.LeftTrigger);
         controller.RightTrigger = new MoveControl(MoveControl.Action.RightTrigger);
         controller.Forward = new MoveControl(MoveControl.Action.Null);
         controller.Reverse = new MoveControl(MoveControl.Action.Null);
      }
#endif

#if VZ_GAME && UNITY_ANDROID && !UNITY_EDITOR && !VZ_SNAPDRAGONVR
      {
         controller = new Controller();
# if !VZ_GEARVR
         controller.Name = "Gvr";
         controller.Description = "Gvr";
         controller.Icons = "buttons_Daydream";
# else
         controller.Name = "GearVR";
         controller.Description = "GearVR";
         controller.Icons = "buttons_GearVR";
# endif
         controller.LeftUp = new MobileControl(MobileControl.Action.Null);
         controller.LeftLeft = new MobileControl(MobileControl.Action.Null);
         controller.LeftRight = new MobileControl(MobileControl.Action.Null);
         controller.LeftDown = new MobileControl(MobileControl.Action.Null);
         controller.RightUp = new MobileControl(MobileControl.Action.Null);
         controller.RightLeft = new MobileControl(MobileControl.Action.Null);
         controller.RightRight = new MobileControl(MobileControl.Action.Null);
         controller.RightDown = new MobileControl(MobileControl.Action.Null);
         controller.LookLeft = new MobileControl(MobileControl.Action.Null);
         controller.LookRight = new MobileControl(MobileControl.Action.Null);
         controller.LookUp = new MobileControl(MobileControl.Action.Null);
         controller.LookDown = new MobileControl(MobileControl.Action.Null);
         controller.LeanLeft = new MobileControl(MobileControl.Action.Null);
         controller.LeanRight = new MobileControl(MobileControl.Action.Null);
         controller.LeanForward = new MobileControl(MobileControl.Action.Null);
         controller.LeanBack = new MobileControl(MobileControl.Action.Null);
         controller.LeftTrigger = new MobileControl(MobileControl.Action.LeftTrigger);
         controller.RightTrigger = new MobileControl(MobileControl.Action.RightTrigger);
         controller.Forward = new MobileControl(MobileControl.Action.Forward);
         controller.Reverse = new MobileControl(MobileControl.Action.Reverse);
      }
#endif


      foreach (var desc in joysticks)
      {
         if (controller != null)
            break;

         joyNum++;

#if UNITY_PS4 && !UNITY_EDITOR
         if (desc != "")
            controller = FindController("DS4");
         else
#endif
            controller = FindController(desc);
      }

      if (controller == null)
         controller = FindController("Keyboard");

      // Set joystick index
      controller.LeftUp.Finalize(joyNum);
      controller.LeftLeft.Finalize(joyNum);
      controller.LeftRight.Finalize(joyNum);
      controller.LeftDown.Finalize(joyNum);
      controller.RightUp.Finalize(joyNum);
      controller.RightLeft.Finalize(joyNum);
      controller.RightRight.Finalize(joyNum);
      controller.RightDown.Finalize(joyNum);
      controller.LeftTrigger.Finalize(joyNum);
      controller.RightTrigger.Finalize(joyNum);
      controller.LeanLeft.Finalize(joyNum);
      controller.LeanRight.Finalize(joyNum);
      controller.LeanForward.Finalize(joyNum);
      controller.LeanBack.Finalize(joyNum);
      controller.LookLeft.Finalize(joyNum);
      controller.LookRight.Finalize(joyNum);
      controller.LookUp.Finalize(joyNum);
      controller.LookDown.Finalize(joyNum);
      controller.Forward.Finalize(joyNum);
      controller.Reverse.Finalize(joyNum);

      return controller;
   }
}
