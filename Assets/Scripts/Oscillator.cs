using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
	[SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
	[SerializeField] float period = 2f;
  [SerializeField] bool fullCycle = false;
	float movementFactor;
  const float tau = Mathf.PI * 2; // about 6.28
  private Vector3 startingPos;

  // Use this for initialization
  void Start()
  {
    startingPos = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    // if( period == 0) {return;}
    if( period <= Mathf.Epsilon) {return;}
    float cycles = Time.time / period;
    float rawSinWave = Mathf.Sin(cycles * tau);

    // if should be -1 to 1 or 0 to 1
    movementFactor = fullCycle == false ? (rawSinWave / 2f + 0.5f) : rawSinWave;

    // Vector3 offset = movementFactor * movementVector;
    transform.position = startingPos + (movementFactor * movementVector);
  }
}
