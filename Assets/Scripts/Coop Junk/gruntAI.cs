﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gruntAI : MonoBehaviour
{
	private float currentHP = 0;
	public float maxHP = 0;
	public float moveSpeed = 0;
	
	public GameObject[] chars = new GameObject[2];
	private GameObject targetChar;

	public string ID = "";

	public bool active = false;
	
	// Use this for initialization
	void Awake ()
	{
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHP <= 0)
		{
			if (ID == "hasDoor")
			{
				foreach (GameObject go in this.GetComponent<doorDestroyer>().doors)
				{
					Destroy(go);
				}
			}

			Destroy(this.gameObject);
		}
		
		TargetDistanceCheck();

		if (active)
		{
			this.transform.position = Vector2.MoveTowards(this.transform.position, targetChar.transform.position,
				moveSpeed * Time.deltaTime);

			if (ID == "spinShot")
			{
				this.GetComponent<spinEnemy>().isActive = true;
			}
		}
	}
	
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "playerProj" || other.gameObject.tag == "compProj")
		{
			currentHP -= other.gameObject.GetComponent<standardBullet>().damage;
			Destroy(other.gameObject);
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

		if (dist0 < dist1 && dist0 < 150)
		{
			targetChar = chars[0];
			active = true;
		}
		else if (dist1 < 150)
		{
			targetChar = chars[1];
			active = true;
		}
		else active = false;
	}
}
