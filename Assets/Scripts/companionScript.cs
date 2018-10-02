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
			    (id_Companion == 1 && waypointManager.GetComponent<wayPointManager>().wayPoints1.Count > 0) ||
			    (id_Companion == 2 && waypointManager.GetComponent<wayPointManager>().wayPoints2.Count > 0))
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
}
