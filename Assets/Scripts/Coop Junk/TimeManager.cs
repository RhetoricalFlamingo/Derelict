using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public bool inSloMo = false;
	public float smI = 0;

	private float fracTime = .06f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (inSloMo)
		{
			sloMo();
		}
	}
	
	public void sloMo()
	{
		if (Time.timeScale == 1)
		{
			Time.timeScale = fracTime;
		}

		if (Time.timeScale == fracTime)
		{
			smI += Time.unscaledDeltaTime;

			if (smI > .15f)
			{
				Time.timeScale = 1f;
				inSloMo = false;
				smI = 0;
			}
		}
	}
}
