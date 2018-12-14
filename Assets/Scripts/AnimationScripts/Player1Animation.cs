using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Animation : MonoBehaviour {
	
	private Animator _anim;
	private SpriteRenderer _spriteRenderer;

	public MovePlayer _movement;
	
	// Use this for initialization
	void Start ()
	{
		_anim = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_movement = GetComponent<MovePlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetAxis("LHorizontal") > 0.2) //if player is walking right
		{
			_anim.SetBool("walking", true);
			_anim.SetBool("looking_side", true);
			_anim.SetBool("looking_front", false);
			_anim.SetBool("looking_back", false);
			_spriteRenderer.flipX = false;
		} 
		else if (Input.GetAxis("LHorizontal") < -0.2)	//if player is walking left
		{
			_anim.SetBool("walking", true);
			_anim.SetBool("looking_side", true);
			_anim.SetBool("looking_front", false);
			_anim.SetBool("looking_back", false);
			_spriteRenderer.flipX = true;
			
		}
		
		
		if (Input.GetAxis("LVertical") > 0.2) //if player is walking down
		{
			_anim.SetBool("walking", true);
			_anim.SetBool("looking_back", true);
			_anim.SetBool("looking_side", false);
			_anim.SetBool("looking_front", false);
		}
		else if (Input.GetAxis("LVertical") < -0.2)
		{
			_anim.SetBool("walking", true);
			_anim.SetBool("looking_back", false);
			_anim.SetBool("looking_side", false);
			_anim.SetBool("looking_front", true);
		}



	}
}
