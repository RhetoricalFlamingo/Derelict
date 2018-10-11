using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayPointManager : MonoBehaviour
{

	public Transform partialWaypointPrefab;
	public Transform reticuleAimer;
	public Transform placedWaypointPrefab0;
	public Transform placedWaypointPrefab1;
	public Transform placedWaypointPrefab2;
		
	public GameObject reticule;
	public bool reticuleLocked = false;
	
	public List<GameObject> wayPoints0 = new List<GameObject>();
	public List<GameObject> wayPoints1 = new List<GameObject>();
	public List<GameObject> wayPoints2 = new List<GameObject>();

	public bool realTime = false;
	private bool lastRealTime = false;
	public GameObject turnManager;

	public int selectWaypointList = 0;
	private float lastDPadV = 0.0f;
	public int selectWaypoint = 0;
	private float lastDPadH = 0.0f;
	float circleI = 0;
	private bool circleHeld = false;
	
	// Update is called once per frame
	void Update ()
	{
		realTime = turnManager.GetComponent<turnManagerScript>().realTime;

		if (!realTime)
		{
			changeWaypointLists();
			changeWaypoints();
			placeWaypoints();
		}

		lastRealTime = realTime;
	}

	void changeWaypointLists()	//Select a set of waypoints (one set per character) to edit
	{
		if (Input.GetAxis("dVertical") <= -.75 && lastDPadV > -.75)
		{
			selectWaypointList--;
				
			if (selectWaypointList < 0)
			{
				selectWaypointList = 2;
			}
		}
			
		if (Input.GetAxis("dVertical") >= .75 && lastDPadV < .75)
		{
			selectWaypointList++;
				
			if (selectWaypointList > 2)
			{
				selectWaypointList = 0;
			}
		}
			
		lastDPadV = Input.GetAxis("dVertical");
	}
	
	void changeWaypoints()	//highlight specific waypoints within the currently-selected set of waypoints
	{
		if (Input.GetAxis("dHorizontal") <= -.75 && lastDPadH > -.75)
		{
			selectWaypoint--;
			Debug.Log(Input.GetAxis("dHorizontal"));
				
			if (selectWaypointList < 0)
			{
				if (selectWaypoint < 0)
				{
					selectWaypoint = wayPoints0.Count;
				}
			}
		}
			
		if (Input.GetAxis("dHorizontal") >= .75 && lastDPadH < .75)
		{
			selectWaypoint++;
				
			if (selectWaypoint > wayPoints0.Count)
			{
				selectWaypoint = 0;
			}
		}
			
		lastDPadH = Input.GetAxis("dHorizontal");
	}

	void placeWaypoints()	//Place and remove waypoints
	{
		if (reticuleLocked)
		{
			if (Input.GetButtonDown("Circle"))	//Cancel waypoint placement and return to reticule movement
			{
				Destroy(GameObject.FindWithTag("partialWaypoint"));
				Destroy(GameObject.FindWithTag("waypointAimer"));
				reticuleLocked = false;
			}
			if (Input.GetButtonDown("Cross"))	//Finish aiming waypoint and return to reticule movement
			{
				if (selectWaypointList == 0)
				{
					Instantiate(placedWaypointPrefab0, GameObject.FindWithTag("waypointAimer").transform.position + (transform.forward * 60), GameObject.FindWithTag("waypointAimer").transform.rotation);
					wayPoints0.Add(GameObject.FindWithTag("placedWaypoint0"));
				}
				if (selectWaypointList == 1)
				{
					Instantiate(placedWaypointPrefab1, GameObject.FindWithTag("waypointAimer").transform.position + (transform.forward * 60), GameObject.FindWithTag("waypointAimer").transform.rotation);
					wayPoints1.Add(GameObject.FindWithTag("placedWaypoint1"));
				}
				if (selectWaypointList == 2)
				{
					Instantiate(placedWaypointPrefab2, GameObject.FindWithTag("waypointAimer").transform.position + (transform.forward * 60), GameObject.FindWithTag("waypointAimer").transform.rotation);
					wayPoints2.Add(GameObject.FindWithTag("placedWaypoint2"));
				}
				
				Destroy(GameObject.FindWithTag("partialWaypoint"));
				Destroy(GameObject.FindWithTag("waypointAimer"));
				reticuleLocked = false;
			}
		}
		
		else if (!reticuleLocked)
		{
			if (Input.GetButtonDown("Cross"))	//begin placing waypoint
			{
				Instantiate(partialWaypointPrefab, reticule.transform.position, Quaternion.identity);
				Instantiate(reticuleAimer, reticule.transform.position, Quaternion.identity);
				reticuleLocked = true;
			}
			
			if (Input.GetButtonDown("Circle"))	//Remove most recent waypoint of currently selected set of waypoints
			{
				circleHeld = true;

				if (selectWaypointList == 0 && wayPoints0.Count > 0)
				{
					wayPoints0.RemoveAt(wayPoints0.Count - 1);
					GameObject[] go0 = GameObject.FindGameObjectsWithTag("placedWaypoint0");
					Destroy(go0[go0.Length - 1]);
					//Debug.Log("Delete Placed Waypoint0");
					
					if (circleI > 2)
					{
						wayPoints0.Clear();
						for (int i = 0; i < go0.Length; i++)
						{
							Destroy(go0[i]);
						}
					}
				}
				
				if (selectWaypointList == 1 && wayPoints1.Count > 0)
				{
					wayPoints1.RemoveAt(wayPoints1.Count - 1);
					GameObject[] go1 = GameObject.FindGameObjectsWithTag("placedWaypoint1");
					Destroy(go1[go1.Length - 1]);
					//Debug.Log("Delete Placed Waypoint1");
					
					if (circleI > 2)
					{
						wayPoints1.Clear();
						for (int i = 0; i < go1.Length; i++)
						{
							Destroy(go1[i]);
						}
					}
				}
				
				if (selectWaypointList == 2 && wayPoints2.Count > 0)
				{
					wayPoints2.RemoveAt(wayPoints2.Count - 1);
					GameObject[] go2 = GameObject.FindGameObjectsWithTag("placedWaypoint2");
					Destroy(go2[go2.Length - 1]);
					//Debug.Log("Delete Placed Waypoint2");
					
					if (circleI > 2)
					{
						wayPoints2.Clear();
						for (int i = 0; i < go2.Length; i++)
						{
							Destroy(go2[i]);
						}
					}
				}
			}
			
			if (circleHeld)		//If player holds circle, increment circleI
			{
				circleI += Time.deltaTime;
				
				if (circleI > .75f)	//if circleI reaches cap, delete all waypoints over current waypoint list selection
				{
					if (selectWaypointList == 0)
					{
						wayPoints0.Clear();
						
						GameObject[] deleteGO = GameObject.FindGameObjectsWithTag("placedWaypoint0");
						foreach (var gameObject in deleteGO)
						{
							Destroy(GameObject.FindWithTag("placedWaypoint0"));
						}
						
						Debug.Log("delete all waypoints0");
					}
					
					if (selectWaypointList == 1)
					{
						wayPoints1.Clear();
						
						GameObject[] deleteGO = GameObject.FindGameObjectsWithTag("placedWaypoint1");
						foreach (var gameObject in deleteGO)
						{
							Destroy(GameObject.FindWithTag("placedWaypoint1"));
						}
						
						Debug.Log("delete all waypoints1");
					}
					
					if (selectWaypointList == 2)
					{
						wayPoints2.Clear();
						
						GameObject[] deleteGO = GameObject.FindGameObjectsWithTag("placedWaypoint2");
						foreach (var gameObject in deleteGO)
						{
							Destroy(GameObject.FindWithTag("placedWaypoint2"));
						}
						
						Debug.Log("delete all waypoints2");
					}
				}
			}

			if (Input.GetButtonUp("Circle"))
			{
				circleHeld = false;
				circleI = 0;
			}
		}
	}
}
