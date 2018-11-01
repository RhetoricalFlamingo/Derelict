using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchScript : MonoBehaviour
{

	public GameObject GameManager;
	public bool flipped = false;

	public string ID = "";
	public List<GameObject> enemiesToSpawn = new List<GameObject>();
	
	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Ghost")
		{
			flipped = true;
			this.GetComponent<SpriteRenderer>().color = Color.white;
			GameManager.GetComponent<TimeManager>().inSloMo = true;
			GameManager.GetComponent<TimeManager>().smI = 0;
			
			if (ID == "enemySpawner")
			{
				foreach (GameObject go in enemiesToSpawn)
				{
					go.SetActive(true);
				}
			}
		}
	}
}
