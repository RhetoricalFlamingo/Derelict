using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

	public bool isMoving = false;

	private Rigidbody2D playerRB;

	public float walkImpulse = 0;
	private float maxWalkSpeed = 1;
	private float rotaterTheta = 0;
	public Quaternion lastRotation;
	
	// Use this for initialization
	void Start ()
	{
		playerRB = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Move();
		Rotation();
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
		} else if (Input.GetAxis("LVertical") != 0 || Input.GetAxis("LHorizontal") != 0)
		{
			rotaterTheta = Mathf.Atan2(-Input.GetAxis("LVertical"), Input.GetAxis("LHorizontal")) * -Mathf.Rad2Deg;
			//	Debug.Log (rotaterTheta);
			//	Debug.Log (Input.GetAxis ("RVertical"));
			this.gameObject.transform.rotation = Quaternion.Euler(0, 0, rotaterTheta - 90.0f);
			lastRotation = this.gameObject.transform.rotation;
		}	
			else this.gameObject.transform.rotation = (lastRotation);
	}
}
