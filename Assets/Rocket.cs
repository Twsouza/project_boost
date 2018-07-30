using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		ProcessInput();
	}

	void ProcessInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			print("space pressed");
		}
		// its separeted because it can rotate while thrust
		// but cant rotate to left and right at same time
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			print("rotating left");
		}
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			print("rotating right");
		}
	}
}
