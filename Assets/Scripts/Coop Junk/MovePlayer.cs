﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
	private Vector2 LJOY_Write = new Vector2(0, 0);
	private Vector2 RJOY_Write = new Vector2(0, 0);

	[Header ("Movement")]
	public float[] moveImpulse = new float[2];
		public float dashSpeed = 0;
	private bool[] dashing_dash = new bool[2];
	public float dash_Dur = 0f;
	private bool[] cooling_dash = new bool[2];
	private float[] dashDelayI = new float[2];

	[Header ("Health")]
	public float[] currentHealth = new float[2];
	public float[] maxHealth = new float[2];
	public bool[] isInvincible = new bool[2];
	public bool[] dying = new bool[2];
	public float[] deathSpeed = new float[2];
	private float[] deathI = new float[2];

	[FormerlySerializedAs("ghost")] public GameObject gun;
	public bool[] overGun = new bool[2];
	
	public GameObject[] chars = new GameObject[2];
	public Rigidbody2D[] playerRBs = new Rigidbody2D[2];

	public GameObject mainCam;

	[Header ("Audio")]
	public AudioSource thisSource;
	public AudioClip dash0, dash1;
	
	public Image[] HPBars = new Image[2];
	public GameObject gameManager;
	private float dyingMinVig = 0f, dyingMaxVig = 0f, startMinVig = 0f, startMaxVig = 0f;
	
	// Use this for initialization
	void Awake () {
		for (int i = 0; i < chars.Length; i++)
		{
			playerRBs[i] = chars[i].GetComponent<Rigidbody2D>();
			currentHealth[i] = maxHealth[i];
			dying[i] = false;
			isInvincible[i] = false;
		}

		thisSource = this.GetComponent<AudioSource> ();
		startMinVig = gameManager.GetComponent<TimeManager> ().minVig;
		startMaxVig = gameManager.GetComponent<TimeManager> ().maxVig;
		dyingMinVig = startMinVig + .04f;
		dyingMaxVig = startMaxVig + .09f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		LJOY_Write = new Vector2(Input.GetAxis("LHorizontal"), Input.GetAxis("LVertical"));
		RJOY_Write = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical"));
		
		Move(0);
		Move(1);
		Dash(0);
		Dash(1);
		HealthMonitor(0);
		HealthMonitor(1);
		Revive(0);
		Revive(1);
		
		PickUpGun();
	}

//**********CUSTOM FUNCTIONS******************************************************************************************

	void Move(int char_Index)	//MOVE playerCharacters
	{
		playerRBs[char_Index].velocity = new Vector2(0, 0);
		
		if (char_Index == 0)
		{
			playerRBs[char_Index].velocity += LJOY_Write * moveImpulse[char_Index];	//move player0
		}
		else if (char_Index == 1)
		{
			playerRBs[char_Index].velocity += RJOY_Write * moveImpulse[char_Index];	//move player1
		}
		
		if (dashing_dash[char_Index])
		{
			dashDelayI[char_Index] += Time.deltaTime;
		}
	}

	void Dash(int char_Index)		//DASH playerCharacters
	{
		if (!dashing_dash[char_Index] && !cooling_dash[char_Index])	//transition from normal speed to dash
		{
			if (char_Index == 0 && Input.GetButtonDown("L1"))
			{
				thisSource.PlayOneShot (dash0);

				moveImpulse[0] += dashSpeed;
				dashing_dash[0] = true;
				chars[0].GetComponent<SpriteRenderer>().color -=Color.black * .3f;
				chars[0].GetComponent<TrailRenderer>().emitting = true;
				isInvincible[0] = true;
				mainCam.GetComponent<cameraController>().shaking = true;
			}
			if (char_Index == 1 && Input.GetButtonDown("R1"))
			{
				thisSource.PlayOneShot (dash1);

				moveImpulse[1] += dashSpeed;
				dashing_dash[1] = true;
				chars[1].GetComponent<SpriteRenderer>().color -= Color.black * .3f;
				chars[1].GetComponent<TrailRenderer>().emitting = true;
				isInvincible[1] = true;
				mainCam.GetComponent<cameraController>().shaking = true;
			}
		}

		if (dashing_dash[char_Index])	//transition from dash to cooldown
		{
			dashDelayI[char_Index] += Time.deltaTime;

			if (dashDelayI[char_Index] > dash_Dur)
			{
				moveImpulse[char_Index] -= dashSpeed;
				dashDelayI[char_Index] = 0;

				dashing_dash[char_Index] = false;
				cooling_dash[char_Index] = true;
				
				chars[char_Index].GetComponent<SpriteRenderer>().color += Color.black * .3f;
				chars[char_Index].GetComponent<TrailRenderer>().emitting = false;
				isInvincible[char_Index] = false;
			}
		}
		
		if (cooling_dash[char_Index])	//transition from dash to cooldown
		{
			dashDelayI[char_Index] += Time.deltaTime;

			if (dashDelayI[char_Index] > .38f)
			{
				dashDelayI[char_Index] = 0;

				cooling_dash[char_Index] = false;
			}
		}
	}

	public void PickUpGun()		//When a playerCharacter is colliding with the GUN, button input picks it up
	{
		if (overGun[0] && (dashing_dash[0] || cooling_dash[0]))
		{
			if (!gun.GetComponent<GhostPlayer> ().isHeld) {
				gun.GetComponent<GhostPlayer> ().fireDelay = gun.GetComponent<GhostPlayer> ().startFireDelay;
				gun.GetComponent<AudioSource> ().pitch = 1f;
			}
			gun.GetComponent<GhostPlayer>().isHeld = true;
			gun.GetComponent<GhostPlayer> ().targetHost = 0;
		}
		if (overGun[1] && (dashing_dash[1] || cooling_dash[1]))
		{
			if (!gun.GetComponent<GhostPlayer> ().isHeld) {
				gun.GetComponent<GhostPlayer> ().fireDelay = gun.GetComponent<GhostPlayer> ().startFireDelay;
				gun.GetComponent<AudioSource> ().pitch = 1f;
			}
			gun.GetComponent<GhostPlayer>().isHeld = true;
			gun.GetComponent<GhostPlayer> ().targetHost = 1;
		}
	}

	void HealthMonitor(int char_Index)		//Monitor player's healthbars and enter downed state
	{
		HPBars[char_Index].fillAmount = currentHealth[char_Index] / maxHealth[char_Index];
		
		if (currentHealth [char_Index] <= 0) {
			dying [char_Index] = true;

			gameManager.GetComponent<TimeManager> ().minVig = dyingMinVig;
			gameManager.GetComponent<TimeManager> ().maxVig = dyingMaxVig;

			if (dying [char_Index]) {
				Debug.Log ("Player" + char_Index + "is Dying");
				moveImpulse [char_Index] = deathSpeed [char_Index];
				deathI [char_Index] += Time.deltaTime;

				if (deathI [char_Index] >= 10) {
					SceneManager.LoadScene ("coopTestScene");
				}
			}
		}
		else if (!dying[0] && !dying[1]) {
			gameManager.GetComponent<TimeManager> ().minVig = startMinVig;
			gameManager.GetComponent<TimeManager> ().maxVig = startMaxVig;
		}
	}

	void Revive(int char_Index)		//Allow one moving character to revive the other with an input in proximity
	{
		if (dying [char_Index] && (Input.GetButton ("L3") || Input.GetButton ("R3")) && Vector2.Distance (chars [0].transform.position, chars [1].transform.position) < 12) {
			

			dying [char_Index] = false;
			deathI [char_Index] = 0;
			moveImpulse [char_Index] = 60f;
			currentHealth [char_Index] = maxHealth [char_Index];
			
			Debug.Log ("Res");
		}
	}
}
