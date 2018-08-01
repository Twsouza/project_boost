using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;

  Rigidbody rigidBody;
  AudioSource audioSource;

  // Use this for initialization
  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    Thrust();
    Rotate();
  }

	void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Friendly":
				print("OK");
				break;
			default:
				print("DEAD!");
				break;
		}
	}

  void Thrust()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      rigidBody.AddRelativeForce(Vector3.up * mainThrust);
      if (!audioSource.isPlaying)
      {
        audioSource.Play();
      }
    }
    else
    {
      audioSource.Stop();
    }
  }

  void Rotate()
  {
		float rotationThisFrame = rcsThrust * Time.deltaTime;

		rigidBody.freezeRotation = true; // take manual control of rotation
    // its separeted because it can rotate while thrust
    // but cant rotate to left and right at same time
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      transform.Rotate(-Vector3.forward * rotationThisFrame);
    }

		rigidBody.freezeRotation = false; // resume physic control of rotation
  }
}
