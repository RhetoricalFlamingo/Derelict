using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManagerScript : MonoBehaviour {

	public bool realTime = false;
	bool lastFrameRealTime = false;

	float turnTimer = 0;
	public float turnTimerMax = 0;

	public GameObject turnMenus;

	// Use this for initialization
	void Awake () {
		turnTimer = turnTimerMax;
	}
	
	// Update is called once per frame
	void Update () {
		
		//Below is a debug line to simulate ending a turn and beginning the next action phase
		if (!realTime) {
			turnMenus.SetActive (true);

			if (Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("TouchPad"))
				realTime = true;
		}
		//***********************************************************************************

		if (realTime) {
			turnMenus.SetActive (false);

			if (!lastFrameRealTime) {
				turnTimer = turnTimerMax;
			}

			turnTimer -= Time.deltaTime;

			if (turnTimer <= 0) {
				realTime = false;
			}
		}

		lastFrameRealTime = realTime;
	}
}
