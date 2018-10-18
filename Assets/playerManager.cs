using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerManager : MonoBehaviour {

		public GameObject[] players = new GameObject[2];
		public float[] currentHP = new float[2];
		public float[] maxHP = new float[2];
		
		public bool[] stunned = new bool[2];
		public float[] stunI = new float[2];
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < players.Length; i++)
		{
			currentHP[i] = maxHP[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < players.Length; i++)
		{
			playerDeath(i);

			if (stunned[i])
			{
				//stunCycle(i);
			}
		}
	}

	void playerDeath(int index)
	{
		if (currentHP[index] <= 0)
		{
			Debug.Log("player" + index + " died");
			SceneManager.LoadScene("coopTestScene");
		}
	}

	void stunCycle(int index)
	{
			stunI[index] += Time.deltaTime;

			if (stunI[index] > 1)
			{
				stunned[index] = false;
				stunI[index] = 0;
			}
	}

	public void takeDamage(int index, float dam)	//call from damage source
	{
		if (!stunned[index])
		{
			currentHP[index] -= dam;

			stunned[index] = true;
		}
	}
}
