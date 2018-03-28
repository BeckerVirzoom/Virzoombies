using UnityEngine;
using System.Collections;
using FMODUnity;

public class AudioBank : MonoBehaviour
{
	public string MusicEvent = "event:/Music";
	FMOD.Studio.EventInstance MusicEv;
	FMOD.Studio.ParameterInstance MusicTypeParam;
	private FMOD.Studio.EventInstance musicEvent;
	// Use this for initialization
	void Start()
	{
		musicEvent.start();
		MusicEv = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
		MusicEv.getParameter("Type", out MusicTypeParam);
		MusicTypeParam.setValue(0f);
		MusicEv.start();
	}
}