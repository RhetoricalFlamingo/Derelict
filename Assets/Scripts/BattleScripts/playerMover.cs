using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMover : MonoBehaviour {

	Rigidbody2D playerRB;

	public bool isMoving = false;
	private float maxWalkSpeed = 1;
	public float walkImpulse = 0;
	public float currentWalkSpeed = 0;

	Vector3 lastPosition = new Vector3(0, 0, 0);

	float iStep = 0;
	float stepMax = 100;
	public Rigidbody2D soundWavePrefab;
	float soundAmp = 0;
	float soundSpeed = 0;

	float rotaterTheta = 0;
	Quaternion lastRotation;

	public Rigidbody2D bullet;
	public float bulletSpeed = 0;
	private float shootI = 0;
	public float shootMax = 0;

	public bool realTime = false;
	public GameObject turnManager;

//*************AWAKE***************************************************************
	//**********************************************************************************
	void Awake () {
		playerRB = this.GetComponent<Rigidbody2D>();
	}
	
//*************UPDATE***************************************************************
	//**********************************************************************************
	void Update () {
		realTime = turnManager.GetComponent<turnManagerScript> ().realTime;
		if (realTime) {
			Rotation ();
			Shoot();
			//Move ();

			//stepCount ();
		}
	}

	private void FixedUpdate()
	{
		if (realTime)
		{
			Move();
		}
	}

//*************USER_FUNCTIONS***********************************************************
//**********************************************************************************	

	void stepCount () {
		if (isMoving) {
			currentWalkSpeed = Vector3.Distance (this.transform.position, lastPosition);
			iStep += Time.deltaTime;

			stepMax = 12 / (currentWalkSpeed * 32);

			if (iStep >= stepMax) {
				Rigidbody2D soundWaveInstance;
				soundWaveInstance = Instantiate (soundWavePrefab, this.transform.position + Vector3.forward, this.transform.rotation);
				iStep = 0;
			}

			lastPosition = this.transform.position;
		} else
			currentWalkSpeed = 0;

	}

	void Move () {
		if ((Input.GetAxis ("LHorizontal") != 0 || Input.GetAxis ("LVertical") != 0)) {
			isMoving = true;
		} else {
			isMoving = false;
		  }
		
		playerRB.AddForce (new Vector2 ((walkImpulse * Input.GetAxis ("LHorizontal") / maxWalkSpeed),
										(walkImpulse * Input.GetAxis ("LVertical") / maxWalkSpeed)));
	}

	void Rotation ()	{
		if (Input.GetAxis ("RVertical") != 0 || Input.GetAxis ("RHorizontal") != 0) {
			rotaterTheta = Mathf.Atan2 (Input.GetAxis ("RVertical"), Input.GetAxis ("RHorizontal")) * -Mathf.Rad2Deg;
			//	Debug.Log (rotaterTheta);
			//	Debug.Log (Input.GetAxis ("RVertical"));
			this.gameObject.transform.rotation = Quaternion.Euler (0, 0, rotaterTheta - 90.0f);
			lastRotation = this.gameObject.transform.rotation;
		} else
			this.gameObject.transform.rotation = (lastRotation);
	}

	void Shoot()
	{
		if (Input.GetButton("R1"))
		{
			shootI += Time.deltaTime;
			if (shootI >= shootMax)
			{
				Rigidbody2D shotInstance;
				shotInstance = Instantiate(bullet, this.transform.position, Quaternion.identity);
				shotInstance.velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
				shootI = 0;
			}
		}

		if (Input.GetButtonDown("R1"))
		{
			Rigidbody2D shotInstance;
			shotInstance = Instantiate(bullet, this.transform.position, Quaternion.identity);
			shotInstance.velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
		}

		if (Input.GetButtonUp("R1"))
		{
			shootI = 0;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "enemyProj")
		{
			Destroy(this.gameObject);
		}
	}
}
