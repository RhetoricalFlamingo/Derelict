using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class progressTracker : MonoBehaviour
{

	public int farthestLevel = 1;

	public GameObject progBar;
	public GameObject progBarOrigin;
	public GameObject mainCam;
	public float distanceOnProgIncrement = 0f;
	private Vector3 targetProgBarPosition = new Vector3(0, 0, 0);

	private bool shaking = false;
	private Vector3 tarPos = new Vector3(0, 0, 0);
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		targetProgBarPosition = progBarOrigin.transform.position +
		                        (Vector3.up * distanceOnProgIncrement) * (farthestLevel - 1);
		
		if (progBar.transform.position != targetProgBarPosition)
		{
			progBar.transform.position = Vector3.MoveTowards(
				progBar.transform.position,
				targetProgBarPosition,
				Time.deltaTime * .25f);

			shaking = true;
		}
		else
		{
			mainCam.transform.position = new Vector3(0, 0, -10);
			shaking = false;
		}
		
		cameraShake(.1f);
	}
	
	void cameraShake(float mag)
	{
		if (shaking)
		{
			tarPos = new Vector3(Random.Range(-mag, mag), Random.Range(-mag, mag), 0);
			tarPos += mainCam.transform.position;
			mainCam.transform.position += (tarPos - mainCam.transform.position) * .2f;
		}
	}
}
