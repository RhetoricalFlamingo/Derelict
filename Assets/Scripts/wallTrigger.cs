using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallTrigger : MonoBehaviour
{

	public GameObject top_wall;
	public GameObject top_wall_2;
	public GameObject right_wall;
	public GameObject left_wall;
	// Use this for initialization
	void Start () {
		
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "top_trigger")
		{
			Destroy(top_wall);
			Destroy(top_wall_2);
		}
		
		if (col.gameObject.name == "right_trigger")
		{
			Destroy(right_wall);
		}
		
		if (col.gameObject.name == "left_trigger")
		{
			Destroy(left_wall);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
