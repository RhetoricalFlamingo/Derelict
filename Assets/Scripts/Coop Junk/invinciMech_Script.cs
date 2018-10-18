﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class invinciMech_Script : MonoBehaviour
{
	private float currentHP = 0;
	public float maxHP = 0;
	public float moveSpeed = 0;
	
	public GameObject[] chars = new GameObject[2];
	private GameObject targetChar;

	public bool stunned = false;
	public float stunMax = 0;
	float stunI = 0;
	
	// Use this for initialization
	void Awake ()
	{
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHP <= 0)
		{
			Destroy(this.gameObject);
		}
		
		TargetDistanceCheck();

		if (stunned)
		{
			Stunned();
		}
		else
		{
			this.transform.position = Vector2.MoveTowards(this.transform.position, targetChar.transform.position, moveSpeed * Time.deltaTime);
		}
	}
	
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "playerProj" || other.gameObject.tag == "compProj")
		{
			currentHP -= other.GetComponent<standardBullet>().damage;
			Destroy(other.gameObject);
		}
		
		if (other.gameObject.tag == "Ghost")
		{
			stunned = true;
			Debug.Log("stunned");
		}
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Destroy(other.gameObject);
			Debug.Log("playerDead");

			SceneManager.LoadScene("coopTestScene");
		}
	}

	private void TargetDistanceCheck()
	{
		float dist0 = Vector2.Distance(transform.position, chars[0].transform.position);
		float dist1 = Vector2.Distance(transform.position, chars[1].transform.position);

		if (dist0 < dist1)
		{
			targetChar = chars[0];
		}
		else targetChar = chars[1];
	}

	private void Stunned()
	{
		this.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 255);
		
		stunI += Time.deltaTime;

		if (stunI >= stunMax)
		{
			stunI = 0;
			stunned = false;
			this.GetComponent<SpriteRenderer>().color = new Vector4(255, 0, 152, 255);
		}
	}
}