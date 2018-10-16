using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class companionScript : MonoBehaviour
{

	public int id_Companion = 0;
	private bool realTime = false;

	public GameObject waypointManager;
	public GameObject turnManager;
	private GameObject targetWP;
	public GameObject lineObject;

	public GameObject bulletPrefab;
	public float bulletSpeed = 0;

	public float moveSpeed = 0;
	public float turnSpeedConst = 0;
	private float turnSpeed = 0;
	private float theta = 0;
	
	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		realTime = turnManager.GetComponent<turnManagerScript>().realTime;

		if (realTime)
		{
			if ((id_Companion == 0 && waypointManager.GetComponent<wayPointManager>().wayPoints0.Count > 0) ||
			    (id_Companion == 1 && waypointManager.GetComponent<wayPointManager>().wayPoints1.Count > 0))/* ||
			    (id_Companion == 2 && waypointManager.GetComponent<wayPointManager>().wayPoints2.Count > 0))*/
			{
				moveTurn();
				pathRender();
			}
		}
		else
		{
			lineObject.SetActive(false);
		}
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "playerProj")
		{
			Destroy(other.gameObject);
			
			spreadShot(-30f);
			spreadShot(-15f);
			spreadShot(0f);
			spreadShot(15f);
			spreadShot(30f);
			
			//Rigidbody2D shotInstance;
			//shotInstance = Instantiate(bullet, this.transform.position + transform.up * 15, Quaternion.identity);
			//shotInstance.velocity = transform.TransformDirection(transform.up * bulletSpeed);
		}
	}

	void moveTurn()
	{
		targetWP = GameObject.FindWithTag("placedWaypoint" + id_Companion);
		this.transform.position = Vector3.MoveTowards(this.transform.position, targetWP.transform.position, Time.deltaTime * moveSpeed);

		if (this.transform.position.x == targetWP.transform.position.x &&
		    this.transform.position.y == targetWP.transform.position.y)
		{
			Destroy(targetWP);
		}
		
		theta = Mathf.Abs(targetWP.transform.eulerAngles.z) - Mathf.Abs(this.transform.eulerAngles.z);
		turnSpeed = (Mathf.Abs(theta) * turnSpeedConst * Time.deltaTime) / (Vector3.Distance(this.transform.position, targetWP.transform.position) + 1);
		
		/*if (this.transform.eulerAngles.z < targetWP.transform.eulerAngles.z)
		{
			this.transform.eulerAngles += new Vector3(0, 0, turnSpeed * Time.deltaTime);
		}	else if (this.transform.eulerAngles.z > targetWP.transform.eulerAngles.z)
		{
			this.transform.eulerAngles += new Vector3(0, 0, -turnSpeed * Time.deltaTime);
		}
		*/
		
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetWP.transform.rotation, turnSpeed);
		
	}

	void pathRender()
	{
		lineObject.SetActive(true);
		lineObject.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
		lineObject.GetComponent<LineRenderer>().SetPosition(1, targetWP.transform.position);
	}

	void spreadShot(float angleOffset = 0f)
	{
		GameObject bullet = Instantiate<GameObject>(bulletPrefab);
		bullet.transform.position = transform.position + transform.up * 15;
 
		Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
		bulletRB.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * transform.up * 5000);
	}
}
