using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnMenuSelector : MonoBehaviour {

	public List<Transform> actionsTransform = new List<Transform>();

	public GameObject selector;
	public int selPosition = 0;

	public bool realTime = false;
	private bool lastRT = false;
	public GameObject turnManager;

	// Use this for initialization
	void Awake () {
		addAllOptions ();
	}
	
	// Update is called once per frame
	void Update () {
		realTime = turnManager.GetComponent<turnManagerScript> ().realTime;
		if (!realTime) {

			selector.transform.position = actionsTransform [selPosition].position;
			if (Input.GetButtonDown ("L1")) {
				selPosition--;
				if (selPosition < 0) {
					selPosition = actionsTransform.Count - 1;
				}
			}
			if (Input.GetButtonDown ("R1")) {
				selPosition++;
				if (selPosition > actionsTransform.Count - 1) {
					selPosition = 0;
				}
			}
		}

		lastRT = realTime;
	}

	void addAllOptions ()	{
		GameObject[] go = GameObject.FindGameObjectsWithTag ("turnOption");
		foreach (GameObject turnOption in go) {
			actionsTransform.Add (turnOption.transform);
		}
	}
}
