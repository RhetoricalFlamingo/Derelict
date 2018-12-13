using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gruntAI : MonoBehaviour
{
	private float currentHP = 0;
	public float maxHP = 0;
	public float moveSpeed = 0;
	public float contactDamage = 0;
	
	public GameObject[] chars = new GameObject[2];
	private GameObject targetChar;

	public string ID = "";

	public bool active = false;

	public GameObject PlayerManager;
	public Image redHitEffectRect;
	
	// Use this for initialization
	void Awake ()
	{
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHP <= 0)
		{
			if (ID == "hasDoor")
			{
				foreach (GameObject go in this.GetComponent<doorDestroyer>().doors)
				{
					Destroy(go);
				}
			}

			Destroy(this.gameObject);
		}
		
		TargetDistanceCheck();

		if (active)
		{
			this.transform.position = Vector2.MoveTowards(this.transform.position, targetChar.transform.position,
				moveSpeed * Time.deltaTime);

			if (ID == "spinShot")
			{
				this.GetComponent<spinEnemy>().isActive = true;
			}
		}
	}
	
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "playerProj" || other.gameObject.tag == "compProj")
		{
			currentHP -= other.gameObject.GetComponent<standardBullet>().damage;
			Destroy(other.gameObject);
		}
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (other.gameObject.name == "char0")
			{
				PlayerManager.GetComponent<MovePlayer>().currentHealth[0] -= contactDamage;
				Debug.Log("Player0 New Health = " + PlayerManager.GetComponent<MovePlayer>().currentHealth[0]);
			}
			else if (other.gameObject.name == "char1")
			{
				PlayerManager.GetComponent<MovePlayer>().currentHealth[1] -= contactDamage;
				Debug.Log("Player1 New Health = " + PlayerManager.GetComponent<MovePlayer>().currentHealth[1]);
			}

			redHitEffectRect.color = new Color (1f, 0f, 0f, .75f);
		}

	}

	private void TargetDistanceCheck()
	{
		float dist0 = Vector2.Distance(transform.position, chars[0].transform.position);
		float dist1 = Vector2.Distance(transform.position, chars[1].transform.position);

		if (dist0 < dist1 && dist0 < 80)
		{
			targetChar = chars[0];
			active = true;

			if (PlayerManager.GetComponent<MovePlayer>().dying[0])
			{
				targetChar = chars[1];
			}
		}
		else if (dist1 < 80)
		{
			targetChar = chars[1];
			active = true;
			
			if (PlayerManager.GetComponent<MovePlayer>().dying[1])
			{
				targetChar = chars[0];
			}
		}
		//else active = false;
	}
}
