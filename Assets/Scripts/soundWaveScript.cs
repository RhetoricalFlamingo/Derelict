using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundWaveScript : MonoBehaviour {

	public float waveVeloc = 0;
	float velocityScaler = 1;
	float lastX = 0;

	public GameObject source;

	float colourGrad = 0;

	public GameObject turnManager;
	private bool realTime = true;

	// Use this for initialization
	void Awake () {
		turnManager = GameObject.FindWithTag ("TM");
		source = GameObject.FindWithTag ("Player");
		waveVeloc = source.GetComponent<playerMover> ().currentWalkSpeed * 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		realTime = turnManager.GetComponent<turnManagerScript> ().realTime;
		if (realTime) {
			this.transform.localScale += new Vector3 (waveVeloc * velocityScaler * Time.deltaTime, waveVeloc * velocityScaler * Time.deltaTime, 0);

			if (this.transform.localScale.x <= lastX) {	
				Destroy (this.gameObject);
			}

			velocityScaler -= .85f * Time.deltaTime;

			this.GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, .9f - colourGrad);
			colourGrad += Time.deltaTime / 1.6f;

			lastX = this.transform.localScale.x;
		}
	}
}
