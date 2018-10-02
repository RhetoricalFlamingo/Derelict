using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gruntAI : MonoBehaviour
{

	public int maxHP = 0;
	public int currentHP = 0;
	
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
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		currentHP -= other.GetComponent<standardBullet>().damage;
		Destroy(other.gameObject);
	}
}
