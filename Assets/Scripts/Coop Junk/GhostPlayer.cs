using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostPlayer : MonoBehaviour
{

	public GameObject[] playerChars = new GameObject[2];

	public int targetHost = 0;
	private int otherHost = 0;
	private bool atTarget = false;
	public float repoSpeed = 0;
	private float modRepoSpeed = 0;

	private float rotaterTheta = 0;
	public float rotSpeed = 0;
	private Quaternion[] lastRotation = new Quaternion[2];

	private float chargeI = 0;
	public float chargeMax = 0;
	public Rigidbody2D proj;
	public float projSpeed = 0;
	public bool haveBullet = true;

	public GameObject mainCam;
	private Rigidbody2D camRB;
	public GameObject mm_Player;
	private Rigidbody2D mm_PlayerRB;
	public float camSpeed = 0;
	private float mm_PlayerSpeed = 0;

	public GameObject GameManager;
	private bool inSloMo = false;
	private float smI = 0;
	
	// Use this for initialization
	void Awake ()
	{
		camRB = mainCam.GetComponent<Rigidbody2D>();
		mm_PlayerRB = mm_Player.GetComponent<Rigidbody2D>();
		
		mm_PlayerSpeed = camSpeed / 4.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		distToTargetChar();
		Reposess();
		Rotation();
		//CameraMove();
		Shoot();
	}

	/*private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "playerProj")
		{
			reloading = false;
			shootI = 0;
		}
	}*/

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
		if (Input.GetButtonDown("L1_C2") && atTarget)
		{
			GameManager.GetComponent<TimeManager>().inSloMo = true;
			
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
		if (Input.GetButton("R1_C2") && haveBullet && atTarget)
		{
			chargeI += Time.deltaTime * 5;
			playerChars[targetHost].transform.position +=
				new Vector3(Random.Range(-chargeI / 5, chargeI / 5), Random.Range(-chargeI / 5, chargeI / 5), 0f);
			
			if (chargeI >= chargeMax)
			{
				chargeI = chargeMax;
			}
		}

		if (Input.GetButtonUp("R1_C2") && atTarget)
		{
			Rigidbody2D shotInstance;
			shotInstance = Instantiate(proj, playerChars[targetHost].transform.position + (playerChars[targetHost].transform.up * 10), Quaternion.identity);
			shotInstance.transform.localScale = Vector2.one * chargeI;
			shotInstance.velocity = playerChars[targetHost].transform.TransformDirection(Vector3.up * projSpeed * chargeI);

			chargeI = 0;
			haveBullet = false;
		}

		if (Input.GetButtonDown("R1_C2"))
		{
			//haveBullet = true;
		}
	}

	void CameraMove()		//Move Camera & Death Plane
	{
		camRB.velocity = (new Vector2 ((camSpeed * Input.GetAxis("LHorizontal_C2")),
			(camSpeed * Input.GetAxis("LVertical_C2"))));
		
		mm_PlayerRB.velocity = (new Vector2 ((mm_PlayerSpeed * Input.GetAxis("LHorizontal_C2")),
			(mm_PlayerSpeed * Input.GetAxis("LVertical_C2"))));
	}

	public void sloMo()
	{
		if (Time.timeScale == 1)
		{
			Time.timeScale = .1f;
		}

		if (Time.timeScale == .1f)
		{
			smI += Time.unscaledDeltaTime;

			if (smI > .2f)
			{
				Time.timeScale = 1f;
				inSloMo = false;
				smI = 0;
			}
		}
	}
}
