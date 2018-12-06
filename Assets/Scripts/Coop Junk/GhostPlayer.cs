using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GhostPlayer : MonoBehaviour
{

	public GameObject[] playerChars = new GameObject[2];
	public GameObject[] laserSights = new GameObject[2];

	public int targetHost = 0;
	private int otherHost = 0;
	private bool atTarget = false;
	[FormerlySerializedAs("repoSpeed")] public float throwSpeed = 0;
	private float modThrowSpeed = 0;

	private float rotaterTheta = 0;
	public float rotSpeed = 0;
	private Quaternion[] lastRotation = new Quaternion[2];

	private float chargeI = 0;
	public float chargeMax = 0;
	[FormerlySerializedAs("haveBullet")] public bool isHeld = true;
	private Rigidbody2D thisRB;
	
	public Rigidbody2D proj;
	public float projSpeed = 0;
	private bool lastFrameTrigger = false;
	
	private ParticleSystem thisPS;
	private TrailRenderer thisTR;
	public Material idleMat, chargeMat;

	public GameObject mainCam;
	private Rigidbody2D camRB;
	public GameObject mm_Player;
	private Rigidbody2D mm_PlayerRB;
	public float camSpeed = 0;
	private float mm_PlayerSpeed = 0;

	public GameObject PlayerManager, GameManager;
	private bool inSloMo = false;
	private float smI = 0;
	
	// Use this for initialization
	void Awake ()
	{
		camRB = mainCam.GetComponent<Rigidbody2D>();
		//mm_PlayerRB = mm_Player.GetComponent<Rigidbody2D>();
		thisPS = this.GetComponent<ParticleSystem>();
		thisRB = this.GetComponent<Rigidbody2D>();
		thisTR = this.GetComponent<TrailRenderer>();
		//mm_PlayerSpeed = camSpeed / 4.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		LockToHolder();
		Throw();
		Rotation();
		//CameraMove();
		Shoot();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "playerSlo" && !isHeld && thisRB.velocity.magnitude > 6)		//Enter slow motion when thrown gun is near player
		{
			GameManager.GetComponent<TimeManager>().inSloMo = true;
			GameManager.GetComponent<TimeManager>().smI = 0;
			GameManager.GetComponent<TimeManager>().sloDur = .45f;
			GameManager.GetComponent<TimeManager>().fracTime = .08f;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "playerEmpty")
		{
			PlayerDistCheck();
			PlayerManager.GetComponent<MovePlayer>().overGun = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "playerEmpty")
		{
			PlayerManager.GetComponent<MovePlayer>().overGun = false;
		}
	}
	
//********************CUSTOM FUNCTIONS*******************************************************************************

	void LockToHolder()		//Check distance to targeted character (better than physics check rn)
	{
		if (isHeld)
		{
			if (targetHost == 0)
				otherHost = 1;

			else if (targetHost == 1)
				otherHost = 0;

			this.transform.position = playerChars[targetHost].transform.position;
			laserSights[targetHost].SetActive(true);
			laserSights[otherHost].SetActive(false);
		}
		else
		{
			laserSights[targetHost].SetActive(false);
		}
	}

	void Throw()		//Throw self from current holder
	{
		if (Input.GetButton("R1_C2") && isHeld)
		{
			var thisEmission = thisPS.emission;
			thisEmission.rateOverTime = 40;
			thisPS.GetComponent<Renderer>().material = chargeMat;
			thisTR.GetComponent<Renderer>().material = chargeMat;
			
			playerChars[targetHost].transform.position +=
				new Vector3(Random.Range(-chargeI / 10, chargeI / 10), Random.Range(-chargeI / 10, chargeI / 10), 0f);
			
			chargeI += Time.deltaTime * 7;
			
			if (chargeI >= chargeMax)
			{
				chargeI = chargeMax;
			}
		}

		if (Input.GetButtonUp("R1_C2") && isHeld)
		{
			thisRB.velocity = playerChars[targetHost].transform.TransformDirection(Vector3.up * projSpeed * chargeI);

			var thisEmission = thisPS.emission;
			thisEmission.rateOverTime = 28;
			thisPS.GetComponent<Renderer>().material = idleMat;
			thisTR.GetComponent<Renderer>().material = idleMat;	
			
			chargeI = 0;
			isHeld = false;
		}
	}

	void Rotation()		//Rotate currently-controlled character
	{
		if (isHeld && (Input.GetAxis("RVertical_C2") != 0 || Input.GetAxis("RHorizontal_C2") != 0))
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

	void Shoot()	//Fire weapon from currently-controlled character
	{
		if (isHeld && !lastFrameTrigger && Input.GetAxis("R2_C2") >= .8f)
		{
			lastFrameTrigger = true;
			
			Rigidbody2D shotInstance;
			shotInstance = Instantiate(proj, playerChars[targetHost].transform.position + (playerChars[targetHost].transform.up * 12), Quaternion.identity);
			shotInstance.transform.localScale = new Vector2(1f, 1f) * chargeI;
			shotInstance.velocity = playerChars[targetHost].transform.TransformDirection(Vector3.up * projSpeed);

			mainCam.GetComponent<cameraController>().shaking = true;
			
			//Debug.Log("shoot");
		}

		if (isHeld && Input.GetAxis("R2_C2") < .5f && lastFrameTrigger)
		{
			lastFrameTrigger = false;
		}
	}

	void CameraMove()		//Move Camera & Death Plane
	{
		camRB.velocity = (new Vector2 ((camSpeed * Input.GetAxis("LHorizontal_C2")),
			(camSpeed * Input.GetAxis("LVertical_C2"))));
		
		mm_PlayerRB.velocity = (new Vector2 ((mm_PlayerSpeed * Input.GetAxis("LHorizontal_C2")),
			(mm_PlayerSpeed * Input.GetAxis("LVertical_C2"))));
	}

	public void sloMo(float dur)		//slow game time down (often called from other scripts)
	{
		if (Time.timeScale == 1)
		{
			Time.timeScale = .1f;
		}

		if (Time.timeScale == .1f)
		{
			smI += Time.unscaledDeltaTime;

			if (smI > dur)
			{
				Time.timeScale = 1f;
				inSloMo = false;
				smI = 0;
			}
		}
	}

	void PlayerDistCheck()	//Check which player is closer to this gun
	{
		float dist0ToPlayer = Vector2.Distance(this.transform.position, playerChars[0].transform.position);
		float dist1ToPlayer = Vector2.Distance(this.transform.position, playerChars[1].transform.position);

		if (dist0ToPlayer < dist1ToPlayer)
		{
			targetHost = 0;
		}
		else
		{
			targetHost = 1;
		}
	}
}
