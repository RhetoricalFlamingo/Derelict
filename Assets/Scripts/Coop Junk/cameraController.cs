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
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update()
	{
		charsDistX = Mathf.Abs(chars[0].transform.position.x - chars[1].transform.position.x);
		charsDistY = Mathf.Abs(chars[0].transform.position.y - chars[1].transform.position.y);
		charsDistSum = (charsDistX + charsDistY) / 2;
		midpoint = ((chars[0].transform.position - chars[1].transform.position) / 2) + chars[1].transform.position;

		
		/*if ((charsDistX > minCamScale && charsDistX < maxCamScale) || (charsDistY > minCamScale * .2f && charsDistY < maxCamScale * .2f))
		{
			this.GetComponent<Camera>().orthographicSize = charsDistSum / charsDistConstant;
			
			lastSize = charsDistSum / charsDistConstant;
			borderParent.transform.localScale = new Vector2(lastSize / 100, lastSize / 100);
		}
		else
		{
			this.GetComponent<Camera>().orthographicSize = lastSize;
			borderParent.transform.localScale = new Vector2(lastSize / 100, lastSize / 100);
		}*/
		
		this.transform.position = new Vector3(midpoint.x, midpoint.y, -10f);
	}
}
