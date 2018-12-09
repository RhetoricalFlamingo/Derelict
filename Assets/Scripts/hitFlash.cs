using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitFlash : MonoBehaviour {

	public string tagID = "";
	private Color startColour;

	void Awake ()	{
		startColour = this.GetComponent<SpriteRenderer> ().color;
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if ((tagID == "Enemy" && other.gameObject.tag == "playerProj") ||
			(tagID == "Destructible") && (other.gameObject.tag == "playerProj" || other.gameObject.tag == "enemyProj"))
		{
			StartCoroutine(hitFlashFunc());
		}
	}

	public IEnumerator hitFlashFunc()
	{
		Debug.Log("hitFlash");
		this.GetComponent<SpriteRenderer>().color = new Color(.9f, .3f, .3f, .9f);
		this.transform.localScale *= 1.15f;
		yield return new WaitForSeconds(.08f);
		this.GetComponent<SpriteRenderer>().color = startColour;
		this.transform.localScale /= 1.15f;
		yield return null;

		//Debug.Log ("Colour = " + startColour);
	}
}
