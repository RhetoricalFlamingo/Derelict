using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class TimeManager : MonoBehaviour
{
	[Header("Slow Motion")]
	public bool inSloMo = false;
	public float smI = 0;

	public float fracTime = .06f, sloDur = 0;

	[Header("Timer")]
	private float timeElapsed = 0f;
	public float timeInSeconds = 0f, timeInMS = 0f, timeInNS = 0f;

	public Text timerSeconds, timerMS, timerNS;

	[Header("Vignetting")]
	public PostProcessingProfile mainPPP;
	private VignetteModel.Settings vigSettings;
	public float minVig = .43f, maxVig = .5f, currentVig = 0;
	public float vigSpeed = 5f;
	private bool vigIncreasing = false;
	
	// Use this for initialization
	void Start () {
		vigSettings = mainPPP.vignette.settings;

		currentVig = minVig;
	}
	
	// Update is called once per frame
	void Update () {
		if (inSloMo)
		{
			sloMo(sloDur);
		}

		timer ();
		heartBeatVignette ();
	}
	
	public void sloMo(float dur)	//Activate slow motion
	{
		if (Time.timeScale == 1)
		{
			Time.timeScale = fracTime;
		}

		if (Time.timeScale < 1)		//increment towards ending slowmotion
		{
			smI += Time.unscaledDeltaTime;

			if (smI > dur)
			{
				Time.timeScale = 1f;
				inSloMo = false;
				smI = 0;
			}
		}
	}

	public void timer()	{
		timeElapsed += Time.deltaTime;

		timeInSeconds = (int)timeElapsed;

		timeInMS = (timeElapsed - timeInSeconds) * 100;	timeInMS = (int)timeInMS;

		timeInNS = (timeElapsed - (timeInSeconds + timeInMS / 100)) * 10000;	timeInNS = (int)timeInNS;

		timerSeconds.text = "" + (99 - timeInSeconds);
		timerMS.text = ":" + (99 - timeInMS);
		timerNS.text = ":" + (99 - timeInNS);
	}

	public void heartBeatVignette ()	{
		if (!vigIncreasing) {
			currentVig -= (currentVig - minVig) * Time.deltaTime * vigSpeed;
			if (currentVig <= minVig + .007f) {
				vigIncreasing = true;
			}
		} 
		else {
			currentVig += (maxVig - currentVig) * Time.deltaTime * vigSpeed;
			if (currentVig >= maxVig - .007f) {
				vigIncreasing = false;
			}
		}

		vigSettings.intensity = currentVig;
		mainPPP.vignette.settings = vigSettings;
	}
}
