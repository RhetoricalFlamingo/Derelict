using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MovePlayer : MonoBehaviour
{
	private Vector2 LJOY_Write = new Vector2(0, 0);
	private Vector2 RJOY_Write = new Vector2(0, 0);
	
	public float[] moveImpulse = new float[2];
		public float dashSpeed = 0;
	private bool[] dashing_dash = new bool[2];
	private bool[] cooling_dash = new bool[2];
	private float[] dashDelayI = new float[2];

	[FormerlySerializedAs("ghost")] public GameObject gun;
	public bool overGun = false;
	
	public GameObject[] chars = new GameObject[2];
	public Rigidbody2D[] playerRBs = new Rigidbody2D[2];

	public GameObject mainCam;
	
	// Use this for initialization
	void Awake () {
		for (int i = 0; i < chars.Length; i++)
		{
			playerRBs[i] = chars[i].GetComponent<Rigidbody2D>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		LJOY_Write = new Vector2(Input.GetAxis("LHorizontal"), Input.GetAxis("LVertical"));
		RJOY_Write = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical"));
		
		//Swap();
		Move(0);
		Move(1);
		Dash(0);
		Dash(1);
		
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
				moveImpulse[0] += dashSpeed;
				dashing_dash[0] = true;
			}
			if (char_Index == 1 && Input.GetButtonDown("R1"))
			{
				moveImpulse[1] += dashSpeed;
				dashing_dash[1] = true;
			}
		}

		if (dashing_dash[char_Index])	//transition from dash to cooldown
		{
			dashDelayI[char_Index] += Time.deltaTime;

			if (dashDelayI[char_Index] > .25f)
			{
				moveImpulse[char_Index] -= dashSpeed;
				dashDelayI[char_Index] = 0;

				dashing_dash[char_Index] = false;
				cooling_dash[char_Index] = true;
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

	void Swap()		//IKARUGA Colour Swap
	{
		if (Input.GetButtonDown("R1") || Input.GetButtonDown("L1"))
		{
			Vector2 char0Pos = chars[0].transform.position;
			chars[0].transform.position = chars[1].transform.position;
			chars[1].transform.position = char0Pos;

			if (gun.GetComponent<GhostPlayer>().targetHost == 0)
				gun.GetComponent<GhostPlayer>().targetHost = 1;
			else gun.GetComponent<GhostPlayer>().targetHost = 0;
		}
	}

	public void PickUpGun()		//When a playerCharacter is colliding with the GUN, button input picks it up
	{
		if (overGun && Input.GetButton("Cross"))
		{
			gun.GetComponent<GhostPlayer>().isHeld = true;
		}
	}
}
