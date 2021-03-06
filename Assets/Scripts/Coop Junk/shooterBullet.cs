﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class shooterBullet : MonoBehaviour {
	
	public float speed;
	public float contactDamage = 0;
	
	private float timer;
	private float lifetime = 5;

	[Header("Instances")]
	private Transform t;
	private GameObject PlayerManager;
	public GameObject redHitEffectRect;
	
	// Use this for initialization
	void Awake () 
	{
		t = GetComponent<Transform>();
		
		timer = Time.time;
		
		PlayerManager = GameObject.FindGameObjectWithTag("playerManager");

		redHitEffectRect = GameObject.FindGameObjectWithTag ("redHitEffect");
	}
	
	// Update is called once per frame
	void Update () {
		t.Translate(Vector3.up * speed * Time.deltaTime);

		if (Time.time > timer + lifetime)
		{
			Destroy(gameObject);
		}
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (other.gameObject.name == "char0")
			{
				PlayerManager.GetComponent<MovePlayer>().currentHealth[0] -= contactDamage;
				//Debug.Log("Player0 New Health = " + PlayerManager.GetComponent<MovePlayer>().currentHealth[0]);
			}
			else if (other.gameObject.name == "char1")
			{
				PlayerManager.GetComponent<MovePlayer>().currentHealth[1] -= contactDamage;
				//Debug.Log("Player1 New Health = " + PlayerManager.GetComponent<MovePlayer>().currentHealth[1]);
			}
			redHitEffectRect.GetComponent<Image>().color = new Color (1f, 0f, 0f, .75f);

			Destroy(this.gameObject);
		}
	}
}
