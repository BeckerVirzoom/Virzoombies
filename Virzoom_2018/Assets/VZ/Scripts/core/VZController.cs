//***********************************************************************
// Copyright 2014 VirZOOM
//***********************************************************************

using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

#if UNITY_PS4 && !UNITY_EDITOR
using UnityEngine.PS4.VR;
#endif

public class VZController : MonoBehaviour
{
   //***********************************************************************
   // PUBLIC API
   //***********************************************************************

   public class Button
   {
      public bool Down { get; private set; }

      public Button()
      {
         Down = false;
      }

      public void Set(bool down)
      {
         if (down == Down)
            return;

         if (down)
         {
            Down = true;
            mDownTime = Time.time;
            mPossibleFalseUp = false;
         }
         else if (Time.time - mDownTime < 0.1f)
         {
            // Don't update Down yet
            mPossibleFalseUp = true;
         }
         else
         {
            Down = false;
            mPossibleFalseUp = false;
         }
      }

      public void Update()
      {
         // Handle possible false button up
         if (mPossibleFalseUp && Time.time - mDownTime > .2f)
         {
            mPossibleFalseUp = false;
            Down = false;
         }

         mChanged = (mLastDown != Down);
         mLastDown = Down;
      }

      public bool Pressed()
      {
         return mChanged && Down;
      }

      public bool Released()
      {
         return mChanged && !Down && mDownTime != 0;
      }

      public bool Held(float period)
      {
         return Down && mDownTime != 0 && Time.time - mDownTime > period;
      }

      public void Clear()
      {
         mDownTime = 0;
      }

      float mDownTime;
      bool mPossibleFalseUp = false;
      bool mLastDown = false;
      bool mChanged = false;
   }

   public bool TiltSteering = false;
   public Button LeftButton = new Button();
   public Button RightButton = new Button();
   public Button DpadUp = new Button();
   public Button DpadDown = new Button();
   public Button DpadLeft = new Button();
   public Button DpadRight = new Button();
   public Button RightUp = new Button();
   public Button RightDown = new Button();
   public Button RightLeft = new Button();
   public Button RightRight = new Button();
   public float InputSpeed { get; private set; } 
   public float HeadRot { get; private set; }  // positive counterclockwise
   public float HeadLean { get; private set; } // positive left
   public float HeadBend { get; private set; }
   public bool IsSteamVR { get; private set; }
   public float Distance { get; private set; }
   public Transform Head { get; private set; }

   public float BatteryVolts()
   {
      return mBikeState.BatteryVolts;
   }

   public int BikePulses()
   {
      return mBikeState.Pulses;
   }

   public float HeartRate()
   {
      float heartRate = mBikeState.HeartRate;

      if (IsBetaBike())
      {
         if (heartRate == 0)
            heartRate = 70; // assume 70 if hands off or alpha bike
         else if (heartRate > 200)
            heartRate = 200;
      }

      return heartRate;
   }

   public void OverrideHeartrate(float heartrate)
   {
      mBikeState.HeartRate = heartrate;
   }

   public bool HasBikeDongle()
   {
      return mBikeState.Type >= 0;
   }

   public float BikeReprogramProgress()
   {
      return mBikeState.ReprogramProgress;
   }

   public bool IsBikeConnected()
   {
      return mBikeState.Connected;
   }

   public bool NeedsDongleDriver()
   {
      return mBikeState.Type == (int)VZBikeType.NeedsDriver;
   }
   
   public bool IsUnsupportedBike()
   {
      return mBikeState.Type == (int)VZBikeType.Unsupported;
   }

   public bool IsAlphaBike()
   {
      return mBikeState.Type == (int)VZBikeType.Alpha;
   }

   public bool IsBetaBike()
   {
      return mBikeState.Type == (int)VZBikeType.Beta && mBikeState.BetaVersion != 2;
   }

   public bool IsBikeSensor()
   {
      return mBikeState.Type == (int)VZBikeType.Beta && mBikeState.BetaVersion == 2;
   }

   // For logging
   public int BikeType()
   {
      return mBikeState.Type;
   }

   public int BikeBetaVersion()
   {
      return mBikeState.BetaVersion;
   }

   public int BikeFirmware()
   {
      return mBikeState.Firmware;
   }

   public int UncalibratedResistance()
   {
      return mBikeState.FilteredResistance;
   }

   public void ForceController()
   {
      // Act like no dongle to inhibit further UpdateBike's that could connect
      mBikeState.Type = -1;
      mBikeState.Connected = false;
   }

   public bool HasHmd()
   {
      return mHasHmd;
   }

   public void Recenter()
   {
      if (!IsHeadTracked())
      {
         Debug.LogError("Can't recenter if head isn't tracked");
         return;
      }

      // Reset hmd
      if (mHasHmd && mCamera != null)
      {
         // Don't use Unity Recenter, leave Camera uncalibrated and manually offset
         float yawOffset = -mCamera.localEulerAngles.y;

#if UNITY_PS4 && !UNITY_EDITOR
         // Don't reset rotation on ps4 if over 25 from absolute
         if (Mathf.Abs(TrackedHeadYaw()) > 25)
            yawOffset = mCameraOffset.localEulerAngles.y;
#endif

         float ang = -yawOffset * Mathf.Deg2Rad;
         float sinAng = Mathf.Sin(ang);
         float cosAng = Mathf.Cos(ang);
         float xOffset = mCamera.localPosition.z * sinAng - mCamera.localPosition.x * cosAng;
         float zOffset = -mCamera.localPosition.z * cosAng - mCamera.localPosition.x * sinAng;
         float yOffset = -mCamera.localPosition.y;

         mCameraOffset.localPosition = new Vector3(xOffset, yOffset, zOffset);
         mCameraOffset.localEulerAngles = new Vector3(0, yawOffset, 0);

         Head.localPosition = Vector3.zero;
         Head.localEulerAngles = Vector3.zero;
      }
   }

   public float TrackedHeadYaw()
   {
      // Return local yaw of mCamera, which should be relative to VR tracking system
#if VZ_GAME
      if (mCamera != null)
         return VZUtl.WrapDegrees(mCamera.localEulerAngles.y);
      else
#endif
         return 0;
   }

   public bool IsHeadTracked()
   {
      if (!mHasHmd)
         return true;

#if UNITY_PS4 && !UNITY_EDITOR
      int handle = PlayStationVR.GetHmdHandle();

      PlayStationVRTrackingStatus status;
      Tracker.GetTrackedDeviceStatus(handle, out status);
      if (status != PlayStationVRTrackingStatus.Tracking)
         return false;

      PlayStationVRTrackingQuality quality;
      Tracker.GetTrackedDevicePositionQuality(handle, out quality);
      return quality == PlayStationVRTrackingQuality.Full;

#elif VZ_GAME && !UNITY_ANDROID 
      if (IsSteamVR)
         return !SteamVR.outOfRange;
      else
         return OVRPlugin.positionTracked;
#else
      return true;
#endif
   }

   public virtual void Restart()
   {
      LeftButton.Clear();
      RightButton.Clear();
      DpadUp.Clear();
      DpadDown.Clear();
      DpadLeft.Clear();
      DpadRight.Clear();
      RightUp.Clear();
      RightDown.Clear();
      RightLeft.Clear();
      RightRight.Clear();
   }

   public GameObject TransitionCanvas()
   {
      return mTransitionCanvas;
   }

   public void ResetTransitionCanvas()
   {
      mTransitionCanvas.transform.localPosition = mTransitionCanvasPos;
      mTransitionCanvas.transform.localRotation = mTransitionCanvasRot;
      mTransitionCanvas.transform.localScale = mTransitionCanvasScale;
   }

   public string BikeSender()
   {
      if (mBikeState.Type == (int)VZBikeType.Beta)
      {
         // Beta bike
         return mBikeState.Sender();
      }
      else if (mBikeState.Type == (int)VZBikeType.Alpha)
      {
         // Alpha bike hack
#if UNITY_PS4 && !UNITY_EDITOR
         return Network.player.ipAddress; // MachineName crashes offline!
#else
         return System.Environment.MachineName;
#endif
      }
      else
      {
         return "";
      }
   }

   public void TryConnectBike()
   {
      VZPlugin.ConnectBike(ref mBikeState);
   }

   public Transform Neck()
   {
      return mNeck;
   }

   public void AttachPlayer(VZPlayer player)
   {
      // Unparent old camera
      if (mCamera != null)
         mCamera.SetParent(null, false);

      // Set new camera and parent ourselves to player
      if (player != null)
      {
         mCamera = player.Camera;
         transform.SetParent(player.transform, false);
      }
      else
      {
         mCamera = null;
         transform.SetParent(null, false);
      }

      // Parent new camera
      if (mCamera != null)
         mCamera.SetParent(mCameraOffset, false);
   }

   public string ControllerName()
   {
      if (!IsBikeConnected() || IsBikeSensor())
         return mController.Name;
      else
         return "Bike";
   }

   // Subclass should provide calibration
   public virtual float CalibratedResistance()
   {
      return 3; // Assume playing on 3 if not calibrated
   }

   public Transform CameraOffset()
   {
      return mCameraOffset;
   }

   public void PickController(string description)
   {
      TextAsset text = Resources.Load("VZControllerMap") as TextAsset;
      using (var stream = new StringReader(text.text))
      {
         var serializer = new XmlSerializer(typeof(VZControllerMap));
         var controllerMap = serializer.Deserialize(stream) as VZControllerMap;
         mController = controllerMap.PickController(description);
      }
   }

   //***********************************************************************
   // PRIVATE API
   //***********************************************************************

   const float kSpeedThreshHi = 0.7f;
   const float kSpeedThreshLo = 0.3f;
   const float kSpeedHoldPeriod = 0.321f;
   const float kSpeedXboneFactor = 23.5f/25f; // xbone controller trigger is a little harder to press
   const float kCountsPerRev = 2f; // compared to 8 for bike magnets
   const float kMetersPerRev = 3 * 2.154f; // with 3:1 wheel gearing and 27" wheels
   const float kControllerMaxSpeed = 10.0f;
   const float kHeadDead = 0.02f;
   const float kHeadWidth = 0.1f;

   protected Transform mCameraOffset;
   protected Transform mCamera;
   protected VZControllerMap.Controller mController;

   Vector3 mTransitionCanvasPos;
   Quaternion mTransitionCanvasRot;
   Vector3 mTransitionCanvasScale;
   bool mIsInBackgroundExecution = false;
   VZBikeState mBikeState = new VZBikeState();
   float mControllerPitch = 0;
   float mControllerYaw = 0;
   float mControllerLean = 0;
   float mControllerBend = 0;
   float mControllerPitchVel = 0;
   float mControllerYawVel = 0;
   float mControllerLeanVel = 0;
   float mControllerBendVel = 0;
   float mControllerSpeed = 0;
   bool mHasHmd;
   GameObject mTransitionCanvas;
   Transform mNeck;

   void Log(string msg)
   {
#if VZ_GAME
      VZUtl.Log(msg);
#else
      Debug.Log(msg);
#endif
   }

#if UNITY_PS4 && !UNITY_EDITOR
   int mPS4RenderScale = 2;

   void PSVRCompleted(DialogStatus status, DialogResult result)
   {
      Log("PSVR Completed " + status + " " + result);

      if (result != DialogResult.OK)
      {
         HmdSetupDialog.OpenAsync(0, PSVRCompleted); 
         return;
      }

      StartCoroutine(SetupPSVR());
   }

   IEnumerator SetupPSVR()
   {
      PlayStationVRSettings.reprojectionSyncType = PlayStationVRReprojectionSyncType.ReprojectionSyncVsync;
      PlayStationVRSettings.reprojectionFrameDeltaType = PlayStationVRReprojectionFrameDeltaType.UnityCameraAndHeadRotation;
      PlayStationVRSettings.minOutputColor = new Color(0.006f, 0.006f, 0.006f);

      UnityEngine.VR.VRSettings.LoadDeviceByName("PlayStationVR");

      yield return null; // HACK in Unity example to wait a frame

      UnityEngine.VR.VRSettings.showDeviceView = true;

      // Delay setting scale
      UnityEngine.VR.VRSettings.renderScale = 1.4f;

      Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
      UnityEngine.VR.VRSettings.enabled = true;

      mHasHmd = true;
      mPS4RenderScale = 2;
   }
#endif

   protected virtual void Awake()
   {
		
#if !UNITY_ANDROID || VZ_SNAPDRAGONVR
      TiltSteering = false;
#else
      TiltSteering = true;
#endif

      Distance = 0;

      // Neck/camera/head setup
      mNeck = transform.Find("Neck");
      mCameraOffset = mNeck.Find("CameraOffset");
      mCamera = null;
      Head = mNeck.Find("Head");

      // Setup transition canvas
      mTransitionCanvas = mNeck.Find("Head/TransitionCanvas").gameObject;
      mTransitionCanvas.SetActive(true);

      mTransitionCanvasPos = mTransitionCanvas.transform.localPosition;
      mTransitionCanvasRot = mTransitionCanvas.transform.localRotation;
      mTransitionCanvasScale = mTransitionCanvas.transform.localScale;

#if UNITY_ANDROID && !UNITY_EDITOR
      // PC/PSVR need to have text in front to be seen on external monitor
      // with android assume we're centered already
      mTransitionCanvas.transform.SetParent(mNeck, false);
#endif

      // SteamVR
      IsSteamVR = (UnityEngine.VR.VRSettings.loadedDeviceName == "OpenVR" && UnityEngine.VR.VRDevice.isPresent);

      // Init plugin
      VZPlugin.Init(Application.dataPath + Path.DirectorySeparatorChar + "Plugins");

      mIsInBackgroundExecution = false;

#if VZ_GAME
      // Add OVRManager for hand controls
# if UNITY_ANDROID && !UNITY_EDITOR && VZ_GEARVR
      GameObject go = new GameObject();
      go.name = "OVRManager";
      go.AddComponent<OVRManager>();
      go.transform.SetParent(transform);
      OVRManager.cpuLevel = 3;
      OVRManager.gpuLevel = 3;
# endif

# if !UNITY_ANDROID && !UNITY_PS4
      if (!IsSteamVR)
      {
         GameObject go = new GameObject();
         go.name = "OVRManager";
         go.AddComponent<OVRManager>();
         go.transform.SetParent(transform);
      }
# endif
#endif
   }

   protected virtual void Start()
   {
      InputSpeed = 0;
      HeadRot = 0;
      HeadLean = 0;
      HeadBend = 0;

#if !UNITY_PS4 && VZ_GAME
      // Redirect Fmod to Rift headphones
      if (GameObject.Find("FMOD_StudioSystem") != null)
      {
         FMOD.System sys = FMODUnity.RuntimeManager.LowlevelSystem;
//         Log(sys);

         int num;
         sys.getNumDrivers(out num);
//         Log(num);

         for (int i = 0; i < num; ++i)
         {
            int namelen = 100;
            StringBuilder name = new StringBuilder(namelen);
            Guid guid;
            int systemrate;
            FMOD.SPEAKERMODE speakermode;
            int speakermodechannels;

            sys.getDriverInfo(i, name, namelen, out guid, out systemrate, out speakermode, out speakermodechannels);

//            Log(i + " " + name + " " + namelen + " " + guid + " " + systemrate + " " + speakermode + " " + speakermodechannels);

            if (name.ToString() == "Headphones (Rift Audio)")
            {
               sys.setDriver(i);
               Log("Redirecting Fmod audio to Rift headphones");
               break;
            }
         }
      }
#endif

      // Setup hmd
#if VZ_GAME && !UNITY_ANDROID
      if (VZReplay.Playback())
      {
         mHasHmd = false;
         UnityEngine.VR.VRSettings.enabled = false;
      }
      else
#endif
      {
#if UNITY_PS4 && !UNITY_EDITOR
         mHasHmd = false;

         Log("PSVR initially enabled " + UnityEngine.VR.VRSettings.enabled);

         if (UnityEngine.VR.VRSettings.enabled)
            StartCoroutine(SetupPSVR());
         else
            HmdSetupDialog.OpenAsync(0, PSVRCompleted); 
#elif UNITY_ANDROID && VZ_SNAPDRAGONVR
# if UNITY_EDITOR
         mHasHmd = false;
# else
         mHasHmd = true;
# endif
#else
         mHasHmd = UnityEngine.VR.VRDevice.isPresent;
         UnityEngine.VR.VRSettings.enabled = mHasHmd;
#endif
      }

      // Try serial communication
#if VZ_GAME && !UNITY_ANDROID
      if (VZReplay.Playback())
      {
         mBikeState.Type = (int)VZBikeType.None;
      }
      else
#endif
      {
         TryConnectBike();
      }

      // Pick controller map
      PickController("");

      // Log
      String hmd = "None";
      if (mHasHmd)
      {
         if (IsSteamVR)
            hmd = "SteamVR";
         else
         {
#if UNITY_PS4 && !UNITY_EDITOR
            hmd = "PSVR";
#elif !UNITY_EDITOR && !VZ_GEARVR
            hmd = "Daydream";
#else
            hmd = "Oculus";
#endif
         }
      }
      Log("Bike:" + mBikeState.Type + " Hmd:" + hmd + " Controller:" + mController.Name);
   }

   protected virtual void OnDestroy()
   {
#if !UNITY_ANDROID || UNITY_EDITOR
      VZPlugin.CloseBike();
#endif
   }

   float ControllerSpeed()
   { 
      float axis = MakeAxis(mController.Reverse, mController.Forward, 1);
      return axis * kControllerMaxSpeed;
   }

   // Turn opposing control inputs into single axis
#if UNITY_ANDROID
   float MakeAxis(float neg, float pos, float multiplier)
   {
      float axis = 0;

      if (pos > neg)
         axis = pos;
      else
         axis = -neg;

      return axis * multiplier;
   }
#endif

   float MakeAxis(VZControllerMap.Control negative, VZControllerMap.Control positive, float multiplier)
   {
      float pos = positive.GetFloat();
      float neg = negative.GetFloat();
      float axis = 0;

      if (pos > neg)
         axis = pos;
      else
         axis = -neg;

      return axis * multiplier;
   }

   protected virtual void Update()
   {
#if UNITY_PS4 && !UNITY_EDITOR
      if (UnityEngine.PS4.Utility.isInBackgroundExecution != mIsInBackgroundExecution)
      {
         mIsInBackgroundExecution = !mIsInBackgroundExecution;
         VZPlugin.PS4SetBackground(mIsInBackgroundExecution);
         mPS4RenderScale = 2; // to fix scale coming back from dash
      }

      // HACK still not fixed in Unity 5.4
      if (mPS4RenderScale != 0)
      {
         mPS4RenderScale--;
         UnityEngine.VR.VRSettings.renderScale = 1.4f;
      }
#endif

#if !(UNITY_ANDROID && !UNITY_EDITOR && VZ_GEARVR)
      // Quit on ESCAPE if not GearVR
      if (Input.GetKey("escape"))
      {
# if UNITY_ANDROID && !UNITY_EDITOR && !VZ_GEARVR && !VZ_SNAPDRAGONVR
         UnityEngine.VR.VRSettings.enabled = false;
# endif
         Application.Quit();
      }
#endif

#if VZ_GAME && !UNITY_ANDROID
      // Get replay record
      if (VZReplay.Playback())
      {
         VZReplay.Record record = VZReplay.Instance.GetRecord();

         if (record != null)
         {
            InputSpeed = record.inputSpeed;

            LeftButton.Set(record.leftButton);
            RightButton.Set(record.rightButton);
            DpadUp.Set(record.dpadUp);
            DpadDown.Set(record.dpadDown);
            DpadLeft.Set(record.dpadLeft);
            DpadRight.Set(record.dpadRight);
            RightUp.Set(record.rightUp);
            RightDown.Set(record.rightDown);
            RightLeft.Set(record.rightLeft);
            RightRight.Set(record.rightRight);

            if (mCamera != null)
            {
               mCamera.localRotation = record.headRotation;
               mCamera.localPosition = record.headPosition;
            }
         }
      }
      else
#endif
      {
         // Update speed and triggers
         if (mBikeState.Type > 0)
         {
            VZPlugin.UpdateBike(ref mBikeState, Time.time);

            InputSpeed = mBikeState.Speed;

            if (!mBikeState.Connected || IsBikeSensor())
            {
               // Get from controller if vz sensor or not connected
               LeftButton.Set(mController.LeftTrigger.GetBool());
               RightButton.Set(mController.RightTrigger.GetBool());
            }
            else
            {
               LeftButton.Set(mBikeState.LeftTrigger);
               RightButton.Set(mBikeState.RightTrigger);
            }
         }
         else
         {
            // Read from joypad/keyboard if no serial
            InputSpeed = ControllerSpeed();
            LeftButton.Set(mController.LeftTrigger.GetBool());
            RightButton.Set(mController.RightTrigger.GetBool());
         }

         // Update face buttons
#if !UNITY_PS4 || UNITY_EDITOR
         if (mBikeState.Type == (int)VZBikeType.Beta)
         {
            // TODO set "right" for vzsensor for one frame after tap on touchpad
            DpadUp.Set(mBikeState.DpadUp);
            DpadDown.Set(mBikeState.DpadDown);
            DpadLeft.Set(mBikeState.DpadLeft);
            DpadRight.Set(mBikeState.DpadRight);
            RightUp.Set(mBikeState.RightUp);
            RightDown.Set(mBikeState.RightDown);
            RightLeft.Set(mBikeState.RightLeft);
            RightRight.Set(mBikeState.RightRight);
         }
         else
#endif
         {
            RightLeft.Set(mController.RightLeft.GetBool());
            RightUp.Set(mController.RightUp.GetBool());
            RightDown.Set(mController.RightDown.GetBool());
            RightRight.Set(mController.RightRight.GetBool());
            DpadRight.Set(mController.LeftRight.GetBool());
            DpadUp.Set(mController.LeftUp.GetBool());
            DpadDown.Set(mController.LeftDown.GetBool());
            DpadLeft.Set(mController.LeftLeft.GetBool());
         }

         // Update camera controls
         if (mCamera != null)
         {
            // Rotate head without hmd 
            if (!mHasHmd)
            {
               float yaw = MakeAxis(mController.LookLeft, mController.LookRight, Mathf.Rad2Deg);
               float pitch = MakeAxis(mController.LookUp, mController.LookDown, Mathf.Rad2Deg * 2);

               mControllerYaw = Mathf.SmoothDamp(mControllerYaw, yaw, ref mControllerYawVel, 1);
               mControllerPitch = Mathf.SmoothDamp(mControllerPitch, pitch, ref mControllerPitchVel, 1);
               mCamera.localEulerAngles = new Vector3(mControllerPitch, mControllerYaw, 0);
            }

            // Translate head without hmd 
            if (!mHasHmd)
            {
               float lean = MakeAxis(mController.LeanLeft, mController.LeanRight, 0.25f);
               float bend = MakeAxis(mController.LeanBack, mController.LeanForward, 0.25f);

               mControllerLean = Mathf.SmoothDamp(mControllerLean, lean, ref mControllerLeanVel, 0.5f);
               mControllerBend = Mathf.SmoothDamp(mControllerBend, bend, ref mControllerBendVel, 0.5f);

               if (TiltSteering)
               {
                  Vector3 angs = mCamera.localEulerAngles;
                  angs.z = -mControllerLean * Mathf.Rad2Deg;
                  mCamera.localEulerAngles = angs;
               }
               else
               {
                  mCamera.localPosition = new Vector3(mControllerLean, 0, mControllerBend);
               }
            }
         }
      }

      // Update Head from Camera and CameraOffset
      if (mCamera != null)
      {
         float ang = -mCameraOffset.localEulerAngles.y * Mathf.Deg2Rad;
         float sinAng = Mathf.Sin(ang);
         float cosAng = Mathf.Cos(ang);
         float x = -mCamera.localPosition.z * sinAng + mCamera.localPosition.x * cosAng + mCameraOffset.localPosition.x;
         float z = mCamera.localPosition.z * cosAng + mCamera.localPosition.x * sinAng + mCameraOffset.localPosition.z;
         float y = mCamera.localPosition.y + mCameraOffset.localPosition.y;

         Head.localPosition = new Vector3(x, y, z);

         Vector3 localAngles = mCamera.localEulerAngles;
         localAngles.y += mCameraOffset.localEulerAngles.y;
         Head.localEulerAngles = localAngles;
      }

      // Update button data
      LeftButton.Update();
      RightButton.Update();
      DpadUp.Update();
      DpadDown.Update();
      DpadLeft.Update();
      DpadRight.Update();
      RightUp.Update();
      RightDown.Update();
      RightLeft.Update();
      RightRight.Update();

      // Get head rot from look direction
      Vector3 localForward = mNeck.InverseTransformDirection(Head.forward);
      HeadRot = -Mathf.Atan2(localForward.x, localForward.z);

      if (HeadRot > Mathf.PI)
         HeadRot -= Mathf.PI * 2;

      if (HeadRot < -Mathf.PI)
         HeadRot += Mathf.PI * 2;

      // Get head lean & bend
      if (TiltSteering)
      {
         float roll = Head.localEulerAngles.z;
         if (roll > 180)
            roll -= 360;
         if (roll > 20)
            roll = 20;
         else if (roll < -20)
            roll = -20;
         HeadLean = roll / 1.5f * Mathf.Deg2Rad;
      }
      else
      {
         HeadLean = -Head.localPosition.x;
      }

      HeadBend = Head.localPosition.z;

      // Subtract rot from lean/bend if hmd (controller doesn't simulate head width)
      if (mHasHmd)
      {
         // Adjust lean
         if (Mathf.Abs(HeadLean) < kHeadDead)
         {
            HeadLean = 0;
         }
         else if (HeadLean > 0)
         {
            if (!TiltSteering)
            {
               if (HeadRot > Mathf.PI / 2.0f)
                  HeadLean -= kHeadWidth;
               else 
                  HeadLean -= kHeadWidth * Mathf.Sin(HeadRot); 
            }

            HeadLean -= kHeadDead;

            if (HeadLean < 0)
               HeadLean = 0;
         }
         else
         {
            if (!TiltSteering)
            {
               if (HeadRot < -Mathf.PI / 2.0f)
                  HeadLean += kHeadWidth;
               else 
                  HeadLean -= kHeadWidth * Mathf.Sin(HeadRot); 
            }

            HeadLean += kHeadDead;

            if (HeadLean > 0)
               HeadLean = 0;
         }

         // Adjust bend
         if (!TiltSteering)
         {
            float headPitch = Head.localEulerAngles.x * Mathf.Deg2Rad;

            if (headPitch < -Mathf.PI)
               headPitch += Mathf.PI * 2;

            HeadBend += kHeadWidth * (1 - Mathf.Cos(headPitch)); 
         }
      }

      // Update distance

      // Adjust "real" speed by resistance factor, which isn't yet incorporated into InputSpeed because all
      // current games don't want it there
      //
      // InputSpeed was determined in 15th speed with 27" wheels on a Kinetic trainer, which experimentally correlates to resistance 5 on beta bike
      float realSpeed = InputSpeed * ResistanceTorque(CalibratedResistance()) / ResistanceTorque(5);

      Distance += Mathf.Abs(realSpeed) * Time.deltaTime;
   }

   float ResistanceTorque(float setting)
   {
      // "Torque" has been experimentally determined to be 0.534 + 0.2372(r), where r is resistance dial setting.
      //
      // The experiment place a weight on pedal of a bike at different resistance settings and measured time to accelerate through an arc
      return 0.535f + 0.2372f * setting;
   }
}

