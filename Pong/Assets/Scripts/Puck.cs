using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D body;
	private SoundManager soundManager;
	public AudioClip[] hitSounds;
	public AudioClip scoreSound;
	private SpriteRenderer sprite;
	private TrailRenderer trail;

	[Tooltip("Adjust countdown seconds")][Range(1, 10)]
	public int timer;

	[Tooltip("Set the initial speed of the puck at the start of each round")]
	public float originalSpeed;
	[Tooltip("How much should the velocity increase from each bounce off of a paddle?")]
	public float speedIncrement;
	private float newSpeed;

	[HideInInspector]
	public bool isDestroyed;
	[HideInInspector]
	public bool initialCollision;

	public bool verticalOrientation() {
		float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
		if ((angle > 80 && angle < 100) || (angle > 170 && angle < 190)) {
			return true;
		}
		return false;
	}

	public void Initialize() {
		body = gameObject.GetComponent<Rigidbody2D> ();
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		trail = gameObject.GetComponent<TrailRenderer> ();
		soundManager = GameObject.FindObjectOfType<SoundManager> ();
		isDestroyed = false;
		newSpeed = originalSpeed;
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "AI") {

			initialCollision = true;

			float newY = (transform.position.y - other.transform.position.y) / other.collider.bounds.size.y;
			float newX = transform.position.x - other.transform.position.x;

			body.velocity = new Vector2(newX, newY).normalized * newSpeed;	
			newSpeed += speedIncrement;

			if (other.gameObject.tag == "Player") {
				other.gameObject.GetComponent<PlayerController> ().Shrink ();
			} else {
				other.gameObject.GetComponent<PlayerAI> ().Shrink ();
			}

		}

		if (other.gameObject.GetComponent<ColorFlash> ()) {
			other.gameObject.GetComponent<ColorFlash> ().Flash (trail.startColor);
		}
		soundManager.PlayRandom (hitSounds);
	}

	void OnTriggerEnter2D (Collider2D other) {
		sprite.enabled = false;
		StartCoroutine(TrailKill());
		if (other.gameObject.tag == "Left Wall" || other.gameObject.tag == "Right Wall") {
			other.gameObject.GetComponent<BreakableWall> ().damage();
			other.GetComponent<BreakableWall> ().ColorMark (trail.startColor);
			isDestroyed = true;
		}
		soundManager.PlaySingle (scoreSound);
	}

	IEnumerator TrailKill() {
		// wait a little bit so the trail can finish sinking into the point of impact before resetting it
		yield return new WaitForSeconds (trail.time + 0.1f);
		trail.enabled = false;
	}

	public IEnumerator ResetPosition() {
		gameObject.GetComponent<Collider2D> ().enabled = false;
		yield return new WaitForSeconds (timer * 0.5f + 1); // one for a breather and then the countdown (1/2 second for each tick)
		gameObject.GetComponent<Collider2D> ().enabled = true;
		body.velocity = Vector2.zero;
		gameObject.transform.position = Vector2.zero;
		sprite.enabled = true;
		trail.enabled = true;
		trail.startColor = Color.white;
		newSpeed = originalSpeed;
		initialCollision = false;

		float direction = Random.Range (0, 2);
		float yDir = direction - 1;
		if (direction >= 1) {
			body.velocity = new Vector2(-originalSpeed, yDir);
		} else {
			body.velocity = new Vector2(originalSpeed, yDir);
		}
	}


	public void ColorChange(Color playerColor) {
		trail.startColor = playerColor;
	}

}
