using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticuleAimer : MonoBehaviour {

	float rotaterTheta = 0;
	Quaternion lastRotation;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("LVertical") != 0 || Input.GetAxis ("LHorizontal") != 0) {	//When placing waypoints, rotate aimer to aim waypoint and finish placement
			rotaterTheta = Mathf.Atan2 (Input.GetAxis ("LVertical"), Input.GetAxis ("LHorizontal")) * Mathf.Rad2Deg;
			//	Debug.Log (rotaterTheta);
			//	Debug.Log (Input.GetAxis ("RVertical"));
			this.gameObject.transform.rotation = Quaternion.Euler (0, 0, rotaterTheta - 90f);
			lastRotation = this.gameObject.transform.rotation;
		} else
			this.gameObject.transform.rotation = (lastRotation);
	}
}
