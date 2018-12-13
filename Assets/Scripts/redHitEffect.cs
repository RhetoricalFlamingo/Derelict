using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class redHitEffect : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Image> ().color -= new Color (0f, 0f, 0f, Time.deltaTime * 1.9f);
		//When player damage sets this thing's colour opacity up, it always ticks back down
	}
}
