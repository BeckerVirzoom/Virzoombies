using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class DeathScript : MonoBehaviour {
	
	public int MaxHP;
	public int HP;

	// Reference to a blood splatter UI component
	public BloodSplatterUI Health;
	public HealthBar healthBar;
	public float timer;
	bool degrade;
	public Text gameover;
    public string musicEvent = "event:/Music";
    FMOD.Studio.EventInstance MusicEv;
    FMOD.Studio.ParameterInstance MusicSongParam;
    public string DMGVOEvent = "event:/DMGVO";
	FMOD.Studio.EventInstance DMGVOEv;
	FMOD.Studio.ParameterInstance DMGgenderParam;
    public string DamageEvent = "event:/Damage";
	FMOD.Studio.EventInstance DamageEv;
	FMOD.Studio.ParameterInstance DamageTypeParam;
	FMOD.Studio.ParameterInstance DamageGenderParam;
    public string moveEvent = "event:/BikeSpeed";
	FMOD.Studio.EventInstance moveEv;
	FMOD.Studio.ParameterInstance moveTypeParam;

	private bool dying;

    private bool canDamage = true;

	// Use this for initialization\
	private void Awake()
	{
		HP = MaxHP;
		degrade = false;
	}

	void Start () {

		DMGVOEv = FMODUnity.RuntimeManager.CreateInstance(DMGVOEvent);
        MusicEv = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        DamageEv = FMODUnity.RuntimeManager.CreateInstance(DamageEvent);
        MusicEv.getParameter("Song", out MusicSongParam);
        MusicSongParam.setValue(6f);
        MusicEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        MusicEv.start();
        DamageEv.getParameter("Amount", out DamageTypeParam);
        moveEv = FMODUnity.RuntimeManager.CreateInstance(moveEvent);
		moveEv.getParameter("speed", out moveTypeParam);



		// FMOD Initialization

		DamageEv = FMODUnity.RuntimeManager.CreateInstance (DamageEvent);
		DMGVOEv = FMODUnity.RuntimeManager.CreateInstance(DMGVOEvent);
		DamageEv.getParameter("Gender", out DamageGenderParam);
		DMGVOEv.getParameter("Gender", out DMGgenderParam);
	}
	

	void Update () {
		
		if (VZPlayer.Instance.Speed() < 4 && VZPlayer.Instance.Speed() != 0)
		{
		    //Debug.Log(VZPlayer.Instance.Speed() + "trigger 1");
			moveEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			moveTypeParam.setValue(0f);
			moveEv.start();
		}
		else if (VZPlayer.Instance.Speed() < 9 && VZPlayer.Instance.Speed() >= 4)
		{
			//Debug.Log(VZPlayer.Instance.Speed() + "trigger 2");
			moveEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			moveTypeParam.setValue(5f);
			moveEv.start();
		}
		else if (VZPlayer.Instance.Speed() >= 9)
		{
			//Debug.Log(VZPlayer.Instance.Speed() + "trigger 3");
			moveEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			moveTypeParam.setValue(10f);
			moveEv.start();
		}
		else if (VZPlayer.Instance.Speed() == 0)
		{
			//Debug.Log(VZPlayer.Instance.Speed() + "trigger 0");
			moveEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	public IEnumerator delay()
	{
		dying = true;

		Health.DisplayGameOver();

		yield return new WaitForSeconds(3.0f);

        MusicEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        GameManager.ResetGame();
    }

    public IEnumerator TempInvincibility()
    {
        canDamage = false;

        yield return new WaitForSeconds(2.0f);

        canDamage = true;
    }

    void OnTriggerEnter(Collider other)
	{
		if (GameManager.playerIsMale) 
		{
			DMGgenderParam.setValue (0f);
			DamageGenderParam.setValue (0f);
			Debug.Log ("Male");
		} 
		else 
		{
			DMGgenderParam.setValue (2f);
			DamageGenderParam.setValue (2f);
			Debug.Log ("Female");
		}


		if (other.gameObject.CompareTag("Obstacle"))
		{
            if (canDamage)
            {
                DMGVOEv.start();
                HP -= 1;

                // Passes the current HP to the UI component and updates it
                Health.UpdateUI(HP);
                healthBar.updateHealth(HP);

                if (HP <= 0)
                {
                    HP = 0;

                    // Pain SFX
                    // Play death sound here

                    moveEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    DamageEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    gameover.enabled = true;

                    if (!dying)
                    {
                        StartCoroutine(delay());
                    }
                }
                else
                {
                    // Pain SFX
                    // Play hurt sound here

                    if (HP == 3)
                    {
                        DamageTypeParam.setValue(.75f);
                        DamageEv.start();
                    }

                    if (HP == 2)
                    {
                        DamageEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        DamageTypeParam.setValue(.18f);
                        DamageEv.start();
                    }

                    if (HP == 1)
                    {
                        DamageEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        DamageTypeParam.setValue(.54f);
                        DamageEv.start();
                    }
                }

                degrade = true;

                other.gameObject.GetComponent<BoxCollider>().enabled = false;

                StartCoroutine(TempInvincibility());
            }
		}
		else if (other.gameObject.CompareTag("BIGobstacle"))
		{

			if (!dying)
			{
				DMGVOEv.start();
				HP = 0;

				// Passes the current HP to the UI component and updates it
				Health.UpdateUI(HP);
                healthBar.updateHealth(HP);

                moveEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				DamageEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				gameover.enabled = true;
				StartCoroutine(delay());
			}
		}
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		gameover.enabled = false;
		HP = 3;
		Health.UpdateUI(HP);
		Health.ResetGameOver();
	}
}
