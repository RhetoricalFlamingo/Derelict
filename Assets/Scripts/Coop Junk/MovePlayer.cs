using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
	public float[] moveImpulse = new float[2];
		public float dashSpeed = 0;
	private bool[] dashing_dash = new bool[2];
	private bool[] cooling_dash = new bool[2];
	private float[] dashDelayI = new float[2];

	public GameObject ghost;
	
	public GameObject[] chars = new GameObject[2];
	public Rigidbody2D[] playerRBs = new Rigidbody2D[2];
	
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
		//Swap();
		Move(0);
		Move(1);
		Dash(0);
		Dash(1);
	}

	void Move(int char_Index)	//MOVE playerCharacters
	{
		if (char_Index == 0)
		{
			playerRBs[char_Index].velocity = (new Vector2((moveImpulse[char_Index] * Input.GetAxis("LHorizontal")),
				(moveImpulse[char_Index] * Input.GetAxis("LVertical"))));
		}
		else
		{
			playerRBs[char_Index].velocity = (new Vector2 ((moveImpulse[char_Index] * Input.GetAxis("RHorizontal")),
				(moveImpulse[char_Index] * Input.GetAxis("RVertical"))));
		}
		
		if (dashing_dash[char_Index])
		{
			dashDelayI[char_Index] += Time.deltaTime;
		}
	}

	void Dash(int dash_Index)		//DASH playerCharacters
	{
		if (!dashing_dash[dash_Index] && !cooling_dash[dash_Index])	//transition from normal speed to dash
		{
			if (dash_Index == 0 && Input.GetButtonDown("L1"))
			{
				moveImpulse[0] += dashSpeed;
				dashing_dash[0] = true;
			}
			if (dash_Index == 1 && Input.GetButtonDown("R1"))
			{
				moveImpulse[1] += dashSpeed;
				dashing_dash[1] = true;
			}
		}

		if (dashing_dash[dash_Index])	//transition from dash to cooldown
		{
			dashDelayI[dash_Index] += Time.deltaTime;

			if (dashDelayI[dash_Index] > .3f)
			{
				moveImpulse[dash_Index] -= dashSpeed;
				dashDelayI[dash_Index] = 0;

				dashing_dash[dash_Index] = false;
				cooling_dash[dash_Index] = true;
			}
		}
		
		if (cooling_dash[dash_Index])	//transition from dash to cooldown
		{
			dashDelayI[dash_Index] += Time.deltaTime;

			if (dashDelayI[dash_Index] > .45f)
			{
				dashDelayI[dash_Index] = 0;

				cooling_dash[dash_Index] = false;
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
