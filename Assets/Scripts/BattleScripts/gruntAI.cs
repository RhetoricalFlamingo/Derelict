using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gruntAI : MonoBehaviour
{

    public char enemyID = ' ';

	public int maxHP = 0;
	public int currentHP = 0;
	private float moveSpeed = 0f;

	public Rigidbody2D bullet;
	float shootI = 0;
	
	public GameObject player;
	public GameObject turnManager;
	public bool realTime = false;
	
	// Use this for initialization
	void Awake ()
	{
		currentHP = maxHP;

		if (enemyID == 'c')
		{
			moveSpeed = 20f;
		}
		else if (enemyID == 's')
		{
			moveSpeed = 8f;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		realTime = turnManager.GetComponent<turnManagerScript>().realTime;
		
		if (currentHP <= 0)
		{
			Destroy(this.gameObject);
		}

		if (realTime)
		{
			if (enemyID == 'c')
			{
				chaser();
			}
			else if (enemyID == 's')
			{
				shooter();
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "playerProj" || other.tag == "compProj")
		{
			currentHP -= other.GetComponent<standardBullet>().damage;
			Destroy(other.gameObject);
		}
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

	public void shooter()
	{
		shootI += Time.deltaTime;
		this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);

		if (shootI > 1)
		{
			Rigidbody2D shotInstance;
			shotInstance = Instantiate(bullet, this.transform.position, Quaternion.identity);
			shotInstance.GetComponent<Rigidbody2D>().velocity = ((player.transform.position - this.transform.position) / 25);

			shootI = 0;
			
			Debug.Log("Fire");
		}
	}
}
