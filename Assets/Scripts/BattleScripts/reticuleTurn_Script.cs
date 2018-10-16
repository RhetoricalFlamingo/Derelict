using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticuleTurn_Script : MonoBehaviour
{

	public float maxRetSize = 0;
	public float minRetSize = 0;
	private bool retGrowing = false;
	public float growthImpulse = 0;
	
	private float impulse = 0;
	public float camImpulse = 0;
	public float camBufferX = 0;
	public float camBufferY = 0;

	public float camMin = 0;
	public float camMax = 0;
	public float zoomSpeed = 0;

	public GameObject cam;

	public GameObject waypointManager;
	public bool isLocked = false;

	// Use this for initialization
	void Awake () {
		this.transform.position = cam.transform.position + Vector3.forward * 50;
	}

	// Update is called once per frame
	void Update ()
	{
		isLocked = waypointManager.GetComponent<wayPointManager>().reticuleLocked;
		
		impulse = (cam.GetComponent<Camera> ().orthographicSize) * 2.1f;
		camBufferX = cam.GetComponent<Camera> ().orthographicSize * 1.45f;
		camBufferY = cam.GetComponent<Camera> ().orthographicSize * .7f;

		if (!isLocked)
		{
			ReticuleMover();
			DirectCameraControl ();
		}
		CameraZoom ();	
		passiveGrowth();
	}

	void ReticuleMover ()	{
		//XCheck for moving camera along with reticule
		if (this.transform.position.x < cam.transform.position.x + camBufferX && Input.GetAxis ("LHorizontal") > 0) {
			this.transform.position += new Vector3 (Input.GetAxis ("LHorizontal") * impulse * Time.deltaTime, 0, 0);
		}	else	{
			if (Input.GetAxis ("LHorizontal") > 0) {
				cam.transform.position += Vector3.right * camImpulse * Time.deltaTime;
			}
		}

		if (this.transform.position.x > cam.transform.position.x - camBufferX && Input.GetAxis ("LHorizontal") < 0) {
			this.transform.position += new Vector3 (Input.GetAxis ("LHorizontal") * impulse * Time.deltaTime, 0, 0);
		} else {
			if (Input.GetAxis ("LHorizontal") < 0) {
				cam.transform.position -= Vector3.right * camImpulse * Time.deltaTime;
			}
		}

		if (this.transform.position.y < cam.transform.position.y + camBufferY && Input.GetAxis ("LVertical") > 0) {
			this.transform.position += new Vector3 (0, Input.GetAxis ("LVertical") * impulse * Time.deltaTime, 0);
		} else {
			if (Input.GetAxis ("LVertical") > 0) {
				cam.transform.position += Vector3.up * camImpulse * Time.deltaTime;
			}
		}


		if (this.transform.position.y > cam.transform.position.y - camBufferY && Input.GetAxis ("LVertical") < 0) {
			this.transform.position += new Vector3 (0, Input.GetAxis ("LVertical") * impulse * Time.deltaTime, 0);
		} else {
			if (Input.GetAxis ("LVertical") < 0) {
				cam.transform.position -= Vector3.up * camImpulse * Time.deltaTime;
			}
		}
		//this.transform.Rotate (0, 0, Time.deltaTime * 70);
	}

	void CameraZoom ()	{
		if (cam.GetComponent<Camera> ().orthographicSize <= camMax) {
			cam.GetComponent<Camera> ().orthographicSize += (Input.GetAxis ("L2") + 1.0f) * Time.deltaTime * zoomSpeed;
		}
		if (cam.GetComponent<Camera> ().orthographicSize >= camMin) {
			cam.GetComponent<Camera> ().orthographicSize += (-Input.GetAxis ("R2") - 1.0f) * Time.deltaTime * zoomSpeed;
		}
	}

	void DirectCameraControl ()	{	//Move camera with rStick instead of reticule panning
		cam.transform.position += ((-Vector3.up * Input.GetAxis ("RVertical")) + (Vector3.right * Input.GetAxis ("RHorizontal"))) * Time.deltaTime * camImpulse;
	}

	void passiveGrowth()	//Reticule growth effect
	{
		if (this.transform.localScale.x > maxRetSize)
		{
			retGrowing = false;
		}
		if (this.transform.localScale.x < minRetSize)
		{
			retGrowing = true;
		}

		if (retGrowing)
		{
			this.transform.localScale += new Vector3(growthImpulse, growthImpulse, 0) * Time.deltaTime;
			
		}	else
		{
			this.transform.localScale -= new Vector3(growthImpulse, growthImpulse, 0) * Time.deltaTime;
			
		}
	}
}

