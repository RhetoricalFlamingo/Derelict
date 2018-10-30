using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shooterBullet : MonoBehaviour {
	
	public float speed;
	
	private float timer;
	private float lifetime = 5;

	private Transform t;
	
	// Use this for initialization
	void Start () 
	{
		t = GetComponent<Transform>();
		
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		t.Translate(Vector3.up * speed * Time.deltaTime);

		if (Time.time > timer + lifetime)
		{
			Destroy(gameObject);
		}
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Destroy(other.gameObject);
			Debug.Log("playerDead");

			SceneManager.LoadScene("coopTestScene");
		}
	}
}
