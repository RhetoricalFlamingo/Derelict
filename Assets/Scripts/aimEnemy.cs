﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class aimEnemy : MonoBehaviour {
	
	[Header("PLAYER TRACKING")]
	public GameObject[] chars = new GameObject[2];
	private GameObject targetChar;
	public bool active = false;
	public float rotationSpeed = 0, moveSpeed = 0;
	
	[Header("SHOT")]
	//public int shotDamage;
	public float shotSpeed, shotDelay;
	private float shotTimer;
	public int volleySize;
	public float volleyGap;

	
	[Header("INSTANCES")]
	public GameObject bulletInstance;
	private Transform t;
	public GameObject PlayerManager;
	public Image redHitEffectRect;

	[Header("STATS")]
	public float contactDamage = 0f, maxHP = 0f;
	private float currentHP = 0f;
	
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		float dist0 = Vector2.Distance(t.position, chars[0].transform.position);
		float dist1 = Vector2.Distance(t.position, chars[1].transform.position);
		
		if (dist0 < dist1 && dist0 < 150)
		{
			targetChar = chars[0];
			active = true;
			Vector2 direction = targetChar.transform.position - t.position;

			var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;		//Angle to look at Player
			//Debug.Log("angle: " + angle);

			var currentAngle = Mathf.MoveTowardsAngle (t.eulerAngles.z, angle - 90, rotationSpeed);

			//t.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward*Mathf.Sin(Time.time));
			t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, currentAngle);

			if(Time.time > shotTimer + shotDelay){

				StartCoroutine("volley");

				shotTimer = Time.time;
			}
		}
		else if (dist1 < 150)
		{
			targetChar = chars[1];
			active = true;
			Vector2 direction = targetChar.transform.position - t.position;

			var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;		//Angle to look at Player
			//Debug.Log("angle: " + angle);

			var currentAngle = Mathf.MoveTowardsAngle (t.eulerAngles.z, angle - 90, rotationSpeed);

			//t.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward*Mathf.Sin(Time.time));
			t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, currentAngle);

			if(Time.time > shotTimer + shotDelay){

				StartCoroutine("volley");

				shotTimer = Time.time;
			}
		}

		else
		{
			active = false;
		}
		
		//if player is seen
		if (active)
		{
			t.transform.position = Vector2.MoveTowards(t.position, targetChar.transform.position,
				moveSpeed * Time.deltaTime);
		}

		//if health = 0
		if (currentHP <= 0) {
			Destroy (this.gameObject);
		}
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (other.gameObject.name == "char0")
			{
				PlayerManager.GetComponent<MovePlayer>().currentHealth[0] -= contactDamage;
				Debug.Log("Player0 New Health = " + PlayerManager.GetComponent<MovePlayer>().currentHealth[0]);
			}
			else if (other.gameObject.name == "char1")
			{
				PlayerManager.GetComponent<MovePlayer>().currentHealth[1] -= contactDamage;
				Debug.Log("Player1 New Health = " + PlayerManager.GetComponent<MovePlayer>().currentHealth[1]);
			}

			redHitEffectRect.color = new Color (1f, 0f, 0f, .75f);
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

	//PRIVATE METHOD
	private IEnumerator volley(){

		int volleyCounter = 0;

		while(volleyCounter < volleySize){

			GameObject b = (GameObject)Instantiate(bulletInstance,t.position,t.rotation);
			b.GetComponent<shooterBullet>().speed = shotSpeed;
	

			volleyCounter++;

			yield return new WaitForSeconds(volleyGap);		
		}
	}
}
