using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpray : MonoBehaviour {

	public Transform sprayObject;

	public string ID = "";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if ((other.gameObject.tag == "playerProj" && ID == "Enemy") || (other.gameObject.tag == "enemyProj" && ID == "Player"))
		{
			Vector2 direction = this.transform.position - other.transform.position;
			float theta = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


			Transform sprayInstance = Instantiate (
				sprayObject,
				this.transform.position,
				Quaternion.Euler(this.transform.position.x, this.transform.position.y, theta - 90f)
			);
		}
	}
}
