using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	public GameObject[] chars = new GameObject[2];
	public GameObject borderParent;
	
	public float charsDistX = 0;
	public float charsDistY = 0;
	private float charsDistSum = 0;
	private float charsDistConstant = 1.5f;
	private Vector3 midpoint = new Vector3(0, 0, 0);
	private float lastSize = 1;

	public float minCamScale = 0;
	public float maxCamScale = 0;
	
	Vector3 startPos = new Vector3(0, 0, 0);
	public bool firstShake = false;
	public bool shaking = false;
	private float shakeI = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update()
	{
		trackPlayers();
		cameraShake(.05f, .45f);
	}

	void trackPlayers()		//Find midpoint between players, set camera placement equal to that point
	{
		charsDistX = Mathf.Abs(chars[0].transform.position.x - chars[1].transform.position.x);
		charsDistY = Mathf.Abs(chars[0].transform.position.y - chars[1].transform.position.y);
		charsDistSum = (charsDistX + charsDistY) / 2;
		midpoint = ((chars[0].transform.position - chars[1].transform.position) / 2) + chars[1].transform.position;
		
		this.transform.position = new Vector3(midpoint.x, midpoint.y, -10f);
	}

	void cameraShake(float dur, float mag)
	{
		 //DEBUG SHOOT
		if (Input.GetKeyDown(KeyCode.Mouse0))	{
			shaking = true;
		} 
		 
		
		if (shaking)
		{
			Debug.Log("Shake");
			
			if (firstShake)
			{
				startPos = this.transform.position;
				firstShake = false;
			}

			Vector3 tarPosition = new Vector3(Random.Range(-mag, mag), Random.Range(-mag, mag), 0);
			tarPosition += this.transform.position;
			this.transform.position += (tarPosition - this.transform.position) * .6f;

			shakeI += Time.deltaTime;

			if (shakeI >= dur)
			{
				this.transform.position = startPos;
				firstShake = true;
				shaking = false;
				shakeI = 0;
			}
		}
	}
}
