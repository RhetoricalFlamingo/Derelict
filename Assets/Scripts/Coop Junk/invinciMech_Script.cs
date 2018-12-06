using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class invinciMech_Script : MonoBehaviour
{
	private float currentHP = 0;
	public float maxHP = 0;
	public float moveSpeed = 0;
	public float contactDamage = 0;
	
	public GameObject[] chars = new GameObject[2];
	private GameObject targetChar;
	public GameObject ghost;

	public bool stunned = false;
	public float stunMax = 0;
	float stunI = 0;

	public bool active = false;
	
	public GameObject GameManager;
	public GameObject PlayerManager;
	
	// Use this for initialization
	void Awake ()
	{
		currentHP = maxHP;
		//ghost = GameObject.FindGameObjectWithTag("Ghost");
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHP <= 0)
		{
			Destroy(this.gameObject);
		}
		
		TargetDistanceCheck();

		if (stunned)
		{
			Stunned();
		}
		else if (active)
		{
			this.transform.position = Vector2.MoveTowards(this.transform.position, targetChar.transform.position, moveSpeed * Time.deltaTime);
		}
	}
	
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Ghost" && !ghost.GetComponent<GhostPlayer>().isHeld && ghost.GetComponent<Rigidbody2D>().velocity.magnitude > 2)
		{
			stunned = true;
			GameManager.GetComponent<TimeManager>().inSloMo = true;
			GameManager.GetComponent<TimeManager>().smI = 0;
			GameManager.GetComponent<TimeManager>().sloDur = .15f;
			GameManager.GetComponent<TimeManager>().fracTime = .06f;
			//Debug.Log("stunned");
		}
		
		if ((other.gameObject.tag == "playerProj" || other.gameObject.tag == "compProj"))
		{
			if (stunned)
			{
				currentHP -= other.gameObject.GetComponent<standardBullet>().damage;
			}
			
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
		}
	}

	private void TargetDistanceCheck()		//Checks distance and health status of players to determine who (if any) to chase
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
		else active = false;
	}

	private void Stunned()	//Invinicimechs are stunned for a time when hit by a thrown gun
	{
		this.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 255);
		
		stunI += Time.deltaTime;

		if (stunI >= stunMax)
		{
			stunI = 0;
			stunned = false;
			this.GetComponent<SpriteRenderer>().color = new Vector4(255, 0, 152, 255);
		}
		
		//ghost.GetComponent<GhostPlayer>().sloMo(.2f);
	}
}
