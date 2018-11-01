﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class standardBullet : MonoBehaviour
{
	public int damage = 0;
	
	private float lifeI = 0;
	public float lifeEnd = 0;
	private bool firstColl = false;

	private Vector3 lastVelo;
	private Rigidbody2D thisRB;

	public GameObject turnManager;
	public bool realTime = false;

	private void Awake()
	{
		thisRB = this.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (firstColl)
		{
			thisRB.velocity *= .92f;
		}
		
		//this.GetComponent<Rigidbody2D>().velocity *= 56 * Time.deltaTime;
		
		/*lifeI += Time.deltaTime;

		if (lifeI >= lifeEnd)
		{
			Destroy(this.gameObject);
			//Debug.Log("bulletDestroyed");
		}*/
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		firstColl = true;
		thisRB.velocity *= .08f;
	}
}