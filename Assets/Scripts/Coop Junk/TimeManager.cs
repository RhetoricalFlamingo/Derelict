using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public bool inSloMo = false;
	public float smI = 0;

	public float fracTime = .06f;
	public float sloDur = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (inSloMo)
		{
			sloMo(sloDur);
		}
	}
	
	public void sloMo(float dur)	//Activate slow motion
	{
		if (Time.timeScale == 1)
		{
			Time.timeScale = fracTime;
		}

		if (Time.timeScale == fracTime)		//increment towards ending slowmotion
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
}
