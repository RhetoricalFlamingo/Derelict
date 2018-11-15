using System.Collections;
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

	public GameObject ghost;
	public GameObject turnManager;
	public bool realTime = false;

	private void Awake()
	{
		thisRB = this.GetComponent<Rigidbody2D>();
		ghost = GameObject.FindWithTag("Ghost");
	}

	private void Update()
	{
		if (firstColl)
		{
			thisRB.drag = 7;
		}
		
		lifeI += Time.deltaTime;

		if (lifeI >= lifeEnd)
		{
			Destroy(this.gameObject);
			//Debug.Log("bulletDestroyed");
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		firstColl = true;
		thisRB.velocity *= .5f;

		if (other.gameObject.tag == "Player")
		{
			ghost.GetComponent<GhostPlayer>().isHeld = true;
			Destroy(this.gameObject);
		}
	}
}
