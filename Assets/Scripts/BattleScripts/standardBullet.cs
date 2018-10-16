using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class standardBullet : MonoBehaviour
{
	public int damage = 0;
	
	private float lifeI = 0;
	public float lifeEnd = 0;

	private Vector3 lastVelo;

	public GameObject turnManager;
	public bool realTime = false;

	private void Update()
	{
		lifeI += Time.deltaTime;

		if (lifeI >= lifeEnd)
		{
			Destroy(this.gameObject);
			//Debug.Log("bulletDestroyed");
		}
	}

	private void FixedUpdate()
	{
		//this.GetComponent<Rigidbody2D>().velocity *= 1f;
	}
}
