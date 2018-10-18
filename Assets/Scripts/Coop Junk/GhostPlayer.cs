using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{

	public GameObject[] playerChars = new GameObject[2];

	public int targetHost = 0;
	private int otherHost = 0;
	private bool atTarget = false;
	public float repoSpeed = 0;
	private float modRepoSpeed = 0;

	private float rotaterTheta = 0;
	private Quaternion[] lastRotation = new Quaternion[2];

	private float shootI = 0;
	public float shootMax = 0;
	public Rigidbody2D proj;
	public float projSpeed = 0;

	public GameObject mainCam;
	private Rigidbody2D camRB;
	public float camSpeed = 0;
	
	// Use this for initialization
	void Start ()
	{
		camRB = mainCam.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		distToTargetChar();
		Reposess();
		Rotation();
		CameraMove();

		if (atTarget)
		{
			Shoot();
		}
	}

	void distToTargetChar()
	{
		if (Vector2.Distance(transform.position, playerChars[targetHost].transform.position) < 8f)
		{
			atTarget = true;
			Debug.Log("atTarget");
		}
		else atTarget = false;

		if (targetHost == 0)
			otherHost = 1;
		
		if (targetHost == 1)
			otherHost = 0;
	}	//Check distance to targeted character (better than physics check rn)

	void Reposess()		//Change current character to control
	{
		if (Input.GetButtonDown("L1_C2"))
		{
			if (targetHost == 0)
				targetHost = 1;
			else targetHost = 0;

			modRepoSpeed = repoSpeed;
		}
		
		this.transform.position = Vector3.MoveTowards(transform.position, playerChars[targetHost].transform.position, modRepoSpeed * Time.deltaTime);

		if (modRepoSpeed < 1000)
		{
			modRepoSpeed += 350 * Time.deltaTime;
		}
	}

	void Rotation()		//Rotate currently-controlled character
	{
		if (atTarget && (Input.GetAxis("RVertical_C2") != 0 || Input.GetAxis("RHorizontal_C2") != 0))
		{
			rotaterTheta = Mathf.Atan2(Input.GetAxis("RVertical_C2"), Input.GetAxis("RHorizontal_C2")) * -Mathf.Rad2Deg;
			playerChars[targetHost].transform.rotation = Quaternion.Euler(0, 0, rotaterTheta - 90.0f);
			lastRotation[0] = playerChars[0].transform.rotation;
			lastRotation[1] = playerChars[1].transform.rotation;
			Debug.Log("atTargetRotation");
		}
		else
		{
			playerChars[otherHost].transform.rotation = lastRotation[otherHost];
		}
	}

	void Shoot()	//Fire weapon of currently-controlled character
	{
		if (Input.GetButton("R1_C2"))
		{
			shootI += Time.deltaTime;
			if (shootI >= shootMax)
			{
				Rigidbody2D shotInstance;
				shotInstance = Instantiate(proj, playerChars[targetHost].transform.position, Quaternion.identity);
				shotInstance.velocity = playerChars[targetHost].transform.TransformDirection(Vector3.up * projSpeed);
				shootI = 0;
			}
		}

		if (Input.GetButtonDown("R1_C2"))
		{
			Rigidbody2D shotInstance;
			shotInstance = Instantiate(proj, playerChars[targetHost].transform.position, Quaternion.identity);
			shotInstance.velocity = playerChars[targetHost].transform.TransformDirection(Vector3.up * projSpeed);
		}

		if (Input.GetButtonUp("R1_C2"))
		{
			shootI = 0;
		}
	}

	void CameraMove()		//Move Camera & Death Plane
	{
		camRB.velocity = (new Vector2 ((camSpeed * Input.GetAxis("LHorizontal_C2")),
			(camSpeed * Input.GetAxis("LVertical_C2"))));
	}
}
