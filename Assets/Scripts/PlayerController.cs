using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 1.0f;
	public float rotationFactor = 15.0f;
	public ParticleSystem explosionPart;
	public GameObject ship;

	public float maxSpeed = 20f;
	public float respawnTime = 3f;

	public bool gameWon = false;
	public int deathCount = 0;

	private GameObject respawnPoint;
	private bool destroyed = false;
	private Rigidbody phys;
	private float deathTimer = 0f;
	private GameDriver gameDriver;

	// Use this for initialization
	void Start () {
		gameDriver = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameDriver> ();

		phys = this.GetComponent<Rigidbody> ();
		foreach(GameObject check in GameObject.FindGameObjectsWithTag("Checkpoint")){
			if (check.GetComponent<Checkpoint> ().MainGate) {
				respawnPoint = check.GetComponent<Checkpoint> ().respawnPoint;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameWon) {
			if (!destroyed) {
				if (Input.GetAxis ("Horizontal") >= 0.001 || Input.GetAxis ("Horizontal") <= -0.001) {
					this.transform.Rotate (0f, rotationFactor * Input.GetAxis ("Horizontal"), 0f);
					phys.velocity = Vector3.RotateTowards (phys.velocity, transform.forward, 360.0f, 0.0f);
				}

				if (Input.GetAxis ("Vertical") >= 0.001 || Input.GetAxis ("Vertical") <= -0.001) {
					phys.AddForce (this.transform.forward * speed * Input.GetAxis ("Vertical"), ForceMode.Impulse);
				}



				if (phys.velocity.magnitude > maxSpeed) {
					phys.velocity = phys.velocity.normalized * maxSpeed;
				}
			} else {
				deathTimer += Time.deltaTime;
				if (deathTimer >= respawnTime) {
					deathTimer = 0;
					this.transform.position = respawnPoint.transform.position;
					this.transform.rotation = respawnPoint.transform.rotation;
					ship.SetActive (true);
					phys.isKinematic = false;
					destroyed = false;
				}
			}
		
		}


	}

	public void Die(){
		deathCount++;
		phys.velocity = Vector3.zero;
		phys.isKinematic = true;

		destroyed = true;
		explosionPart.Play ();
		ship.SetActive (false);
	}

	public void Won(){
		gameWon = true;
		phys.velocity = Vector3.zero;
		phys.isKinematic = true;
	}

	public void Reset(){
		foreach (GameObject checkPoint in GameObject.FindGameObjectsWithTag("Checkpoint")) {
			if (checkPoint.GetComponent<Checkpoint> ().MainGate) {
				var resPoint = checkPoint.GetComponent<Checkpoint> ().respawnPoint.transform;
				transform.position = resPoint.position;
				transform.rotation = resPoint.rotation;
			}
		}
		phys.isKinematic = false;
		gameWon = false;
	}

	void OnCollisonEnter(Collider other){
		/*
		if (other.gameObject.tag != "Safe") {
			if((prevMag - phys.velocity.magnitude) < 1) {
				explosionPart.Play ();
				GameObject.Destroy (this);
			}
		}
		*/
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Checkpoint") {
			if (other.gameObject.GetComponent<Checkpoint> ().MainGate) {
				if (respawnPoint != other.gameObject.GetComponent<Checkpoint> ().respawnPoint) {
					gameDriver.AdvanceLap ();
				}
			}
			respawnPoint = other.gameObject.GetComponent<Checkpoint> ().respawnPoint;
		}
	}
}
