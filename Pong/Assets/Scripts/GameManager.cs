using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	BreakableWall leftWall, rightWall;
	[SerializeField]
	GameObject leftPlayer, rightPlayer;
	[SerializeField]
	ColorFlash topWall, bottomWall;
	[SerializeField]
	Canvas canvas;
	SoundManager soundManager;
	[SerializeField]
	private AudioClip countdownClip;
	Text text;
	[SerializeField]
	Puck puck;

	void Start() {
		
		leftWall = GameObject.FindGameObjectWithTag("Left Wall").GetComponent<BreakableWall>();
		rightWall = GameObject.FindGameObjectWithTag ("Right Wall").GetComponent<BreakableWall>();
		topWall = GameObject.FindGameObjectWithTag("Top Wall").GetComponent<ColorFlash>();
		bottomWall = GameObject.FindGameObjectWithTag ("Bottom Wall").GetComponent<ColorFlash>();

		soundManager = GameObject.FindObjectOfType<SoundManager> ();
		canvas = GameObject.FindObjectOfType<Canvas>();

		text = canvas.GetComponentInChildren<Text> ();

		leftPlayer = Instantiate(leftPlayer, new Vector3 (-6, 0, 0), Quaternion.identity);
		rightPlayer = Instantiate(rightPlayer, new Vector3 (6, 0, 0), Quaternion.identity);

		puck = Instantiate (puck, Vector3.zero, Quaternion.identity);

		puck.Initialize ();

		StartCoroutine(puck.ResetPosition());
		StartCoroutine (Countdown ());

	}

	// pause function
	void Update() {
		if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown (KeyCode.Escape)) {
			if (Time.timeScale == 1) {
				Time.timeScale = 0;
			} else {
				Time.timeScale = 1;
			}	
		}

		if (Input.GetKeyDown (KeyCode.Space) && puck.verticalOrientation()) {
			float yDir = 0;
			if (puck.body.velocity.y > 0) {
				yDir = puck.originalSpeed / 2;
			} else {
				yDir = -puck.originalSpeed / 2;
			}
			puck.body.velocity = new Vector2 (puck.originalSpeed, Random.Range(0, yDir));
		}
	}

	void FixedUpdate() {

		if (leftWall.isBroken || rightWall.isBroken) {
			
			if (leftWall.isBroken) {
				print ("Player 2 has won the round!");
				if (rightPlayer.GetComponent<PlayerAI> ()) {
					rightPlayer.GetComponent<PlayerAI> ().speed--;
					print ("The AI's speed has decreased to " + rightPlayer.GetComponent<PlayerAI> ().speed);
				}
			}

			if (rightWall.isBroken) {
				print ("Player 1 has won the round!");
				if (rightPlayer.GetComponent<PlayerAI> ()) {
					rightPlayer.GetComponent<PlayerAI> ().speed++;
					print ("The AI's speed has increased to " + rightPlayer.GetComponent<PlayerAI> ().speed);
				}
			}
				
			StartCoroutine(leftWall.ResetHealth ());
			StartCoroutine(rightWall.ResetHealth ());
			topWall.Reset ();
			bottomWall.Reset ();
			leftPlayer.GetComponent<Paddle> ().ResetXPosition ();
			rightPlayer.GetComponent<Paddle> ().ResetXPosition ();
			print("New Round");
		}



		if (puck.isDestroyed) {
			puck.isDestroyed = false;
			StartCoroutine (puck.ResetPosition ());
			StartCoroutine (Countdown ());
		}



		if (puck.body.velocity.x > 0 && puck.initialCollision) {
			puck.ColorChange (leftPlayer.GetComponent<Renderer> ().material.color);
		} else if (puck.body.velocity.x < 0 && puck.initialCollision) {
			puck.ColorChange (rightPlayer.GetComponent<Renderer> ().material.color);
		} else {
			puck.ColorChange (Color.white);
		}
	}
		

	IEnumerator Countdown() {
		yield return new WaitForSeconds (1);

		float progressPercentage = (float)(leftWall.health + rightWall.health) / (float)(leftWall.maxHealth + rightWall.maxHealth);
		leftPlayer.GetComponent<Paddle> ().ResetSize (progressPercentage);
		rightPlayer.GetComponent<Paddle> ().ResetSize (progressPercentage);

		Image background = canvas.GetComponentInChildren<Image> ();
		background.enabled = true;
		text.enabled = true;
		for (int i = puck.timer; i > 0; i--) {
			text.text = i.ToString();
			soundManager.PlaySingle (countdownClip);
			yield return new WaitForSeconds (0.5f);
		}
		text.text = "";
		text.enabled = false;
		background.enabled = false;
	}

}
