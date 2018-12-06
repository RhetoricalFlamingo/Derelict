using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;

public class ObstacleScript : MonoBehaviour
{

	private float currentHP = 0f;
	public float maxHP = 0f;

	public bool damageable = false, isGlass = false;
	private bool damaged = false;
	private bool justDamaged = false;
	public Sprite this_Damaged;

	// Use this for initialization
	void Awake ()
	{
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentHP <= 0)
		{
			Destroy(this.gameObject);
		}
		
		if (currentHP <= maxHP / 2 && !damaged)
		{
			damaged = true;
			justDamaged = true;
		}

		if (damaged)
		{
			justDamaged = false;
			this.GetComponent<SpriteRenderer>().sprite = this_Damaged;
		}
		
		/*DEBUG DAMAGE
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			currentHP--;
			StartCoroutine(hitFlash());
		}
		DEBUG DAMAGE*/
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "enemyProj")
		{
			Destroy(other.gameObject);
			if (damageable)
			{
				currentHP--;
			}
		}
		else if (other.gameObject.tag == "playerProj")
		{
			if (damageable)
			{
				currentHP--;
			}
		}
		else if (other.gameObject.tag == "Ghost")
		{
			if (isGlass)
			{
				currentHP--;
			}
		}
	}
}
