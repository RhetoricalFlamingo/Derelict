using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour {

	public GameObject corDoor;

	private bool pressable = false, growing = true;
	public float maxScale = 0f, minScale = 0f, currentScale = 0f;
	private float startY = 0f, yTop = 5f;

	private Vector2 startScale = new Vector2(0f, 0f);

	private GameObject nearestChar;
	public GameObject char0, char1;

	private Color startColour;

	void Awake ()	{
		startScale = this.transform.localScale;
		startColour = this.GetComponent<SpriteRenderer> ().color;
		startY = corDoor.transform.position.y;
	}

	void Update () {
		if (pressable) {
			GrowShrink ();
			this.GetComponent<SpriteRenderer> ().color = new Color (.7f, 1f, .7f, .8f);
			//corDoor.SetActive (false);
			RaiseDoor();
		}
		else {
			LowerDoor ();
			this.GetComponent<SpriteRenderer> ().color = startColour;
		}
	}

	void OnTriggerStay2D (Collider2D other)	{
		if (other.gameObject.tag == "playerEmpty") {

			pressable = true;

			if (other.name == "char0Empty") {
				nearestChar = char0;
			} else if (other.name == "char1Empty") {
				nearestChar = char1;
			}
		}
	}

	void OnTriggerExit2D (Collider2D other)	{

		pressable = false;
	}


	void GrowShrink ()	{
		if (growing) {
			currentScale += (maxScale - currentScale) * Time.deltaTime;

			if (currentScale >= maxScale * .9f) {
				growing = false;
			}
		} 
		else {
			currentScale -= (currentScale - minScale) * Time.deltaTime;

			if (currentScale <= minScale * 1.1f) {
				growing = true;
			}
		}

		this.transform.localScale = startScale * currentScale;
		Debug.Log ("GrowShrink");
	}

	void RaiseDoor ()	{
		if (corDoor.transform.position.y < startY + yTop) {
			corDoor.transform.position += Vector3.up * ((yTop + startY + 1 - corDoor.transform.position.y) * Time.deltaTime * 10);
			corDoor.GetComponent<SpriteRenderer> ().color -= new Color (0f, 0f, 0f, Time.deltaTime * 12);
			corDoor.GetComponent<BoxCollider2D>().enabled = false;

			Debug.Log ("goingUp");
		}
		else {
			corDoor.GetComponent<SpriteRenderer> ().color = new Color (startColour.r, startColour.g, startColour.b, 0f);
			Debug.Log ("Completely up");
		}
	}

	void LowerDoor ()	{
		if (corDoor.transform.position.y > startY) {
			corDoor.transform.position -= Vector3.up * ((corDoor.transform.position.y - startY + 1) * Time.deltaTime * 10);
			corDoor.GetComponent<SpriteRenderer> ().color += new Color (0f, 0f, 0f, Time.deltaTime * 12);

			Debug.Log ("goingDown");
		}
		else {
			corDoor.GetComponent<BoxCollider2D>().enabled = true;
			corDoor.GetComponent<SpriteRenderer> ().color = startColour;
			Debug.Log ("completelyDown");
		}
	}
}
