using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gruntAI : MonoBehaviour
{

    public char enemyID = ' ';

	public int maxHP = 0;
	public int currentHP = 0;
	private float moveSpeed = 20f;

	public GameObject player;
	public GameObject turnManager;
	public bool realTime = false;
	
	// Use this for initialization
	void Awake ()
	{
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update ()
	{
		realTime = turnManager.GetComponent<turnManagerScript>().realTime;
		
		if (currentHP <= 0)
		{
			Destroy(this.gameObject);
		}

		if (realTime && enemyID == 'c')
		{
			chaser();
		}
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		currentHP -= other.GetComponent<standardBullet>().damage;
		Destroy(other.gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (enemyID == 'c' && other.gameObject.tag == "Player")
		{
			Destroy(other.gameObject);
			Debug.Log("playerDead");
		}
	}

	public void chaser()
	{
		this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
		//Debug.Log("chasePlayer");
	}
}
