using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class doorDestroyer : MonoBehaviour {

	public List<GameObject> doors = new List<GameObject>();
	
	public GameObject tutPrompt;
	public bool dead = false;

	public void Update()
	{
		if (dead)
			tutPrompt.GetComponent<TextMesh>().text = "Ghost: \nLStick";
	}
}
