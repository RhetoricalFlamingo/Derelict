using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class standardBullet : MonoBehaviour
{
	public int damage = 0;

	private void FixedUpdate()
	{
		this.GetComponent<Rigidbody2D>().velocity *= 1.04f;
	}
}
