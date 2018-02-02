using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 1.0f;
	public float rotationFactor = 15.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Vertical") >= 0.001 || Input.GetAxis ("Vertical") <= -0.001) {
			this.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * speed * Input.GetAxis ("Vertical"),ForceMode.Impulse);
		}

		if (Input.GetAxis ("Horizontal") >= 0.001 || Input.GetAxis ("Horizontal") <= -0.001) {
			this.transform.Rotate (0f, rotationFactor * Input.GetAxis ("Horizontal"), 0f);
			this.GetComponent<Rigidbody>().velocity = Vector3.RotateTowards (this.GetComponent<Rigidbody> ().velocity, transform.forward, 360.0f, 0.0f);
		}
	}
}
