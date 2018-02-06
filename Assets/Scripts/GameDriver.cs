using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDriver : MonoBehaviour {

	public Text timeText;
	public Text lapText;

	public Text finalTimeText;
	public Text deathText;

	public GameObject HUD;
	public GameObject WinScreen;
	public GameObject Stats;

	private PlayerController player;
	private float timeElasped = 0f;
	private int laps = 1;
	private bool Won = false;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		HUD.SetActive (true);
		WinScreen.SetActive (false);
		Stats.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!Won) {
			timeElasped += Time.deltaTime;
			lapText.text = laps + "/3";
			timeText.text = string.Format ("{0:00}:", timeElasped / 60) + string.Format ("{0:00}", timeElasped % 60);
		} 
	}

	public void AdvanceLap(){
		if (laps == 3) {
			GameWon ();
		} else {
			laps += 1;
		}
	}

	public void GameWon(){
		player.Won ();
		HUD.SetActive (false);
		WinScreen.SetActive(true);
		finalTimeText.text = string.Format ("{0:00}:", timeElasped / 60) + string.Format ("{0:00}", timeElasped % 60);
		deathText.text = player.deathCount.ToString();
		Stats.SetActive (true);
	}

	public void ResetGame(){
		timeElasped = 0f;
		laps = 1;
		player.deathCount = 0;
		WinScreen.SetActive(false);
		Stats.SetActive (false);
		HUD.SetActive (true);
		player.Reset();
		Won = false;
	}
}
