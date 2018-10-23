﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
	private Vector2 LJOY_Write = new Vector2(0, 0);
	private Vector2 RJOY_Write = new Vector2(0, 0);
	
	public float[] moveImpulse = new float[2];
		public float dashSpeed = 0;
	private bool[] dashing_dash = new bool[2];
	private bool[] cooling_dash = new bool[2];
	private float[] dashDelayI = new float[2];

	public GameObject ghost;
	
	public GameObject[] chars = new GameObject[2];
	public Rigidbody2D[] playerRBs = new Rigidbody2D[2];

	public GameObject mainCam;
	private float xBuffer = 171.5f;
	private float yBuffer = 95f;
	
	// Use this for initialization
	void Start () {
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
	}

	void Move(int char_Index)	//MOVE playerCharacters
	{
		playerRBs[char_Index].velocity = new Vector2(0, 0);
		
		if (char_Index == 0)
		{
			if (chars[char_Index].transform.position.x < mainCam.transform.position.x - xBuffer && LJOY_Write.x < 0)	//Player0 camera boundaries, keeps player from moving away from camera once at screen edge
				LJOY_Write.x = 0;
			if (chars[char_Index].transform.position.x > mainCam.transform.position.x + xBuffer && LJOY_Write.x > 0)
				LJOY_Write.x = 0;
			if (chars[char_Index].transform.position.y < mainCam.transform.position.y - yBuffer && LJOY_Write.y < 0)
				LJOY_Write.y = 0;
			if (chars[char_Index].transform.position.y > mainCam.transform.position.y + yBuffer && LJOY_Write.y > 0)
				LJOY_Write.y = 0;
			
			
			playerRBs[char_Index].velocity += LJOY_Write * moveImpulse[char_Index];	//move player0
		}
		else if (char_Index == 1)
		{
			if (chars[char_Index].transform.position.x < mainCam.transform.position.x - xBuffer && RJOY_Write.x < 0)	//Player1 camera boundaries, keeps player from moving away from camera once at screen edge
				RJOY_Write.x = 0;
			if (chars[char_Index].transform.position.x > mainCam.transform.position.x + xBuffer && RJOY_Write.x > 0)
				RJOY_Write.x = 0;
			if (chars[char_Index].transform.position.y < mainCam.transform.position.y - yBuffer && RJOY_Write.y < 0)
				RJOY_Write.y = 0;
			if (chars[char_Index].transform.position.y > mainCam.transform.position.y + yBuffer && RJOY_Write.y > 0)
				RJOY_Write.y = 0;


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

			if (dashDelayI[char_Index] > .3f)
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

			if (dashDelayI[char_Index] > .45f)
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

			if (ghost.GetComponent<GhostPlayer>().targetHost == 0)
				ghost.GetComponent<GhostPlayer>().targetHost = 1;
			else ghost.GetComponent<GhostPlayer>().targetHost = 0;
		}
	}
}
