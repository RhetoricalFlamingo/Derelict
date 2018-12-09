using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour {

	public GameObject corDoor;

	private bool pressable = false, growing = true;
	public float maxScale = 0f, minScale = 0f, currentScale = 0f;
	private float startY = 0f, yTop = 2f;

	private Vector2 startScale = new Vector2(0f, 0f);

	private GameObject nearestChar;
	public GameObject char0, char1;

	private Color startColour;

	void Awake ()	{
		startScale = this.transform.localScale;
		startColour = this.GetComponent<SpriteRenderer> ().color;
		startY = corDoor.transform.position.y;
		yTop += startY;
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
			corDoor.transform.position += new Vector3 (0, ((yTop - corDoor.transform.position.y) * Time.deltaTime / 3), 0f);
			corDoor.GetComponent<SpriteRenderer> ().color -= new Color (0f, 0f, 0f, Time.deltaTime * 12);

			Debug.Log ("goingUp");
		}
		else {
			corDoor.transform.position = new Vector3 (
				corDoor.transform.position.x, 
				corDoor.transform.position.y, 
				-9f
			);
			Debug.Log ("Completely up");
		}
	}

	void LowerDoor ()	{
		if (corDoor.transform.position.y > startY) {
			corDoor.transform.position -= new Vector3 (0, ((corDoor.transform.position.y - startY) * Time.deltaTime / 3), 0f);
			corDoor.GetComponent<SpriteRenderer> ().color += new Color (0f, 0f, 0f, Time.deltaTime * 12);

			Debug.Log ("goingDown");
		}
		else {
			corDoor.transform.position = new Vector3 (
				corDoor.transform.position.x, 
				corDoor.transform.position.y, 
				0f
			);
			Debug.Log ("completelyDown");
		}
	}
}
