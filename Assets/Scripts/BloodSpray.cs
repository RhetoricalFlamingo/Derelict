using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpray : MonoBehaviour {

	public Transform sprayObject;

	public string ID = "";
	private bool playerSprayLimiter = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		playerSprayLimiter = false;
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (
			(other.gameObject.tag == "playerProj" && (ID == "Enemy")	//regular enemy
			|| (ID == "IMech" && this.GetComponent<invinciMech_Script>().stunned))	//stunnable enemies
			|| (other.gameObject.tag == "enemyProj" && ID == "Player" && playerSprayLimiter == false)	//player
			)
		{
			Vector2 direction = this.transform.position - other.transform.position;
			float theta = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Debug.Log ("Instantiate bloodspray at angle measure " + theta);

			Transform sprayInstance = Instantiate (
				sprayObject,
				this.transform.position,
				Quaternion.Euler(0, 0, theta + 90f)
			);

			sprayInstance.GetComponent<Rigidbody2D> ().velocity = other.GetComponent<Rigidbody2D> ().velocity;
			playerSprayLimiter = true;
		}
	}
}
