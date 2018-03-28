//***********************************************************************
// Copyright 2014 VirZOOM
//***********************************************************************

using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

[StructLayout(LayoutKind.Sequential, Pack=8)]
public struct VZMotionInput
{
   public float DraftSpeed;
   public float DraftFactor;
   public float DownhillFactor;
   public float UphillFactor;
   public float ControllerHeadLean;
   public float LeanFudge;
   public float ApparentLean;
   public float ControllerHeadRot;
   public float SpeedFudge;
   public float MaxTurn;
   public float StoppedTurnFraction;
   public float BodyRot;
   public float Scale;
   public float LandingHardness;
   public float LandingRadius;
   public float ControllerInputSpeed;
   public float SpeedMultiplier;
   public float SpeedMultiplierSpeedSettleTime;
   public float SpeedSettleTimeWhenAccelerating;
   public float SpeedSettleTimeWhenBraking;
   public float MaxSpeed;
   public float MaxVertSpeed;
   public float ColliderSpeedMultiplier;
   public float VelocityX;
   public float VelocityY;
   public float VelocityZ;
   public float DeltaTime;
   public float HitDistance;
   public float HitNormalX;
   public float HitNormalY;
   public float HitNormalZ;
   public float PlayerX;
   public float PlayerY;
   public float PlayerZ;
   [MarshalAs(UnmanagedType.U1)]
   public bool Colliding;
   [MarshalAs(UnmanagedType.U1)]
   public bool OnMenu;
   [MarshalAs(UnmanagedType.U1)]
   public bool AllowDrift;
   [MarshalAs(UnmanagedType.U1)]
   public bool AllowYaw;
   [MarshalAs(UnmanagedType.U1)]
   public bool AllowPitch;
   [MarshalAs(UnmanagedType.U1)]
   public bool AllowRoll;
   [MarshalAs(UnmanagedType.U1)]
   public bool Reverse;
   [MarshalAs(UnmanagedType.U1)]
   public bool RaycastResult;
   [MarshalAs(UnmanagedType.U1)]
   public bool RotateAtStop;

   public void Init()
   {
      DownhillFactor = 0.5f;
      UphillFactor = 1.414f;
      ControllerHeadLean = 0;
      LeanFudge = 2;
      ApparentLean = 0.5f;
      ControllerHeadRot = 0;
      SpeedFudge = 1;
      MaxTurn = 0.5f;
      StoppedTurnFraction = 0;
      BodyRot = 0;
      Scale = 1;
      LandingHardness = 2;
      LandingRadius = 0.25f;
      ControllerInputSpeed = 0;
      SpeedMultiplier = 1;
      SpeedMultiplierSpeedSettleTime = 3;
      SpeedSettleTimeWhenAccelerating = 3;
      SpeedSettleTimeWhenBraking = 3;
      MaxSpeed = 12;
      MaxVertSpeed = 4;
      ColliderSpeedMultiplier = 1;
      VelocityX = 0;
      VelocityY = 0;
      VelocityZ = 0;
      DeltaTime = 0.016f;
      HitDistance = 0;
      HitNormalX = 0;
      HitNormalY = 1;
      HitNormalZ = 0;
      PlayerX = 0;
      PlayerY = 0;
      PlayerZ = 0;
      Colliding = true;
      OnMenu = false;
      AllowDrift = true;
      AllowYaw = true;
      AllowPitch = false;
      AllowRoll = false;
      Reverse = false;
      RaycastResult = false;
      RotateAtStop = false;
   }
}

[StructLayout(LayoutKind.Sequential, Pack=8)]
public struct VZMotionOutput
{
   public float BodyRot;
   public float RotVel;
   public float Turn;
   public float VelDot;
   public float TransverseDot;
   public float Speed;
   public float Lean;
   public float TransformEulerX;
   public float TransformEulerY;
   public float TransformEulerZ;
   public float VelocityX;
   public float VelocityY;
   public float VelocityZ;
   public float PlayerX;
   public float PlayerY;
   public float PlayerZ;
   [MarshalAs(UnmanagedType.U1)]
   public bool LandingSmoothed;
}

[StructLayout(LayoutKind.Sequential, Pack=8)]
public struct VZBikeState
{
   public float Timestamp;
   public float TimeAtLastPulseMs;
   public float HeartRate;
   public float BatteryVolts;
   public float Speed;
   public float ReprogramProgress;
   public int Pulses;
   public int FilteredResistance;
   public int Type;
   public int BetaVersion;
   public int Firmware;
   [MarshalAs(UnmanagedType.U1)]
   public bool LeftTrigger;
   [MarshalAs(UnmanagedType.U1)]
   public bool DpadUp;
   [MarshalAs(UnmanagedType.U1)]
   public bool DpadDown;
   [MarshalAs(UnmanagedType.U1)]
   public bool DpadLeft;
   [MarshalAs(UnmanagedType.U1)]
   public bool DpadRight;
   [MarshalAs(UnmanagedType.U1)]
   public bool RightTrigger;
   [MarshalAs(UnmanagedType.U1)]
   public bool RightUp;
   [MarshalAs(UnmanagedType.U1)]
   public bool RightDown;
   [MarshalAs(UnmanagedType.U1)]
   public bool RightLeft;
   [MarshalAs(UnmanagedType.U1)]
   public bool RightRight;
   [MarshalAs(UnmanagedType.U1)]
   public bool Connected;
   public byte Sender0;
   public byte Sender1;
   public byte Sender2;
   public byte Sender3;
   public byte Sender4;
   public byte Sender5;

   public string Sender()
   {
      string sender;
      VZPlugin.SenderString(Sender0, Sender1, Sender2, Sender3, Sender4, Sender5, out sender);
      return sender;
   }
}

public enum VZBikeType
{
   NeedsDriver = -2,
   None = -1,
   Unsupported = 0,
   Alpha = 1,
   Beta = 2
}

public static class VZPlugin
{
#if VZ_GAME && UNITY_ANDROID && !UNITY_EDITOR
   public static void RequestPermissions()
   {
# if VZ_GEARVR || VZ_SNAPDRAGONVR
      AndroidJavaClass bleClass = new AndroidJavaClass("com.virzoom.ble.vzble.blueGigaBLE");
      bleClass.CallStatic("grantPermissions");
# else
      // Must use Google's own requester
      string[] permissionNames = { 
         "android.permission.BLUETOOTH", 
         "android.permission.BLUETOOTH_ADMIN", 
         "android.permission.ACCESS_FINE_LOCATION", 
         "android.permission.READ_EXTERNAL_STORAGE", 
         "android.permission.SET_DEBUG_APP", 
         };

      GvrPermissionsRequester permission = GvrPermissionsRequester.Instance;

      CheckPermissions(true);

      permission.RequestPermissions(permissionNames,
         (GvrPermissionsRequester.PermissionStatus[] permissionResults) =>
         {
            CheckPermissions(false);
         });
# endif
   }

   public static void CheckPermissions(bool preCheck)
   {
      AndroidJavaClass bleClass = new AndroidJavaClass("com.virzoom.ble.vzble.blueGigaBLE");
      bleClass.CallStatic("checkPermissions", preCheck);
   }
#endif

#if UNITY_PS4 && !UNITY_EDITOR
	[DllImport("vzplugin")]
	public static extern bool PS4HeadsetConnected();

	[DllImport("vzplugin")]
	static extern void PS4Print(IntPtr msg);

   public static void PS4Print(string str)
   {
      IntPtr msg = Marshal.StringToHGlobalAnsi(str);
      PS4Print(msg);
      Marshal.FreeHGlobal(msg);
   }

	[DllImport("vzplugin")]
	public static extern void PS4SetBackground(bool background);

#elif !UNITY_ANDROID && !UNITY_IOS

   [DllImport("vzplugin")]
   public static extern void PCShowConsole();

   [DllImport("vzplugin")]
   static extern IntPtr PCGraphicsDriverVersion();

   public static void PCGraphicsDriverVersion(out string version)
   {
      IntPtr buffer = PCGraphicsDriverVersion();
      version = Marshal.PtrToStringAnsi(buffer);
   }

   [DllImport("vzplugin")]
   static extern IntPtr PCOculusVersion();

   public static void PCOculusVersion(out string version)
   {
      IntPtr buffer = PCOculusVersion();
      version = Marshal.PtrToStringAnsi(buffer);
   }

#endif

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZInit")]
#else
   [DllImport("vzplugin", EntryPoint="VZInit")]
#endif
   static extern void VZInit();

   public static void Init(string dllPath = null)
   {
      // Init plugin dir
      if (dllPath != null)
      {
#if !UNITY_EDITOR
         // Player
         // add plugins path to the environment for the steam dll.
         var currentPath = Environment.GetEnvironmentVariable("PATH",
            EnvironmentVariableTarget.Process);

         if (currentPath != null && currentPath.Contains(dllPath) == false)
            Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
#endif
      }

#if UNITY_ANDROID && !UNITY_EDITOR
      // Init bluetooth class
      AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      AndroidJavaObject context = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

      AndroidJavaClass bleClass = new AndroidJavaClass("com.virzoom.ble.vzble.blueGigaBLE");
# if VZ_GEARVR || !VZ_GAME
      bool grantPermissions = true;
# else
      bool grantPermissions = false;
# endif
      bleClass.CallStatic("create", context, grantPermissions);
#endif

      VZInit();
   }

#if UNITY_IOS
   [DllImport("__Internal")]
#else
   [DllImport("vzplugin")]
#endif
   public static extern void StartCounter();

#if UNITY_IOS
   [DllImport("__Internal")]
#else
   [DllImport("vzplugin")]
#endif
   public static extern double GetCounter(bool print);

#if UNITY_IOS
   [DllImport("__Internal")]
#else
   [DllImport("vzplugin")]
#endif
   static extern void VZSetGameName(IntPtr msg);

   public static void VZSetGameName(string str)
   {
      IntPtr msg = Marshal.StringToHGlobalAnsi(str);
      VZSetGameName(msg);
      Marshal.FreeHGlobal(msg);
   }

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZResetSpeed")]
#else
   [DllImport("vzplugin", EntryPoint="VZResetSpeed")]
#endif
   public static extern void ResetSpeed(float speed);

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZResetMotion")]
#else
   [DllImport("vzplugin", EntryPoint="VZResetMotion")]
#endif
   public static extern void ResetMotion();

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZSetTurnSettleTime")]
#else
   [DllImport("vzplugin", EntryPoint="VZSetTurnSettleTime")]
#endif
   public static extern void SetTurnSettleTime(float time);

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZUpdateMotion")]
#else
   [DllImport("vzplugin", EntryPoint="VZUpdateMotion")]
#endif
   public static extern void UpdateMotion(ref VZMotionInput input, ref VZMotionOutput output);

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZConnectBike")]
#else
   [DllImport("vzplugin", EntryPoint="VZConnectBike")]
#endif
   public static extern void ConnectBike(ref VZBikeState state);

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZUpdateBike")]
#else
   [DllImport("vzplugin", EntryPoint="VZUpdateBike")]
#endif
   public static extern bool UpdateBike(ref VZBikeState state, float time);

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZCloseBike")]
#else
   [DllImport("vzplugin", EntryPoint="VZCloseBike")]
#endif
   public static extern void CloseBike();

#if UNITY_IOS
   [DllImport("__Internal", EntryPoint="VZSenderString")]
#else
   [DllImport("vzplugin", EntryPoint="VZSenderString")]
#endif
   static extern IntPtr SenderString(byte s0, byte s1, byte s2, byte s3, byte s4, byte s5);

   public static void SenderString(byte s0, byte s1, byte s2, byte s3, byte s4, byte s5, out string sender)
   {
      IntPtr buffer = SenderString(s0, s1, s2, s3, s4, s5);
      sender = Marshal.PtrToStringAnsi(buffer);
   }
}
