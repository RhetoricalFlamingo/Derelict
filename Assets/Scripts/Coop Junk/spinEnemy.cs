using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinEnemy : MonoBehaviour {
	
	[Header("GENERAL")]
	public float rotationSpeed;
	public bool isActive = false;
	
	[Header("SHOT")]
	//public int shotDamage;
	public float shotSpeed;
	public float shotDelay;
	private float shotTimer;
	
	[Header("INSTANCES")]
	public GameObject bulletInstance;
	private Transform t;
	
	// Use this for initialization
	void Start () 
	{
		t = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActive)	{
			
			t.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

			if (Time.time > shotTimer + shotDelay)
			{
				GameObject b = (GameObject) Instantiate(bulletInstance, t.position, t.rotation);
				//b.GetComponent<shooterBullet>().setOwner(this);
				b.GetComponent<shooterBullet>().speed = shotSpeed;


				shotTimer = Time.time;
			}
		}
	}
}
