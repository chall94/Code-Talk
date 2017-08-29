using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : Paddle {

	[Range(1, 12)]
	public float speed = 7f;

	private float distanceRatio;
	private float lerpSpeed;
	private float resetSpeed;

	protected Puck puck;

	Vector2 paddlePosition;
	Vector2 puckPosition;

	protected override void Start() {
		puck = GameObject.FindObjectOfType<Puck> ();
		base.Start ();
	}

	void FixedUpdate() {
		
		paddlePosition = gameObject.transform.position;
		puckPosition = puck.transform.position;


		// the closer the distance between the puck and paddle, the closer to 1 the ratio gets
		// the further the distance between the puck and paddle, the closer to 0 the ratio gets

		distanceRatio = 1 / Mathf.Abs (puckPosition.x - paddlePosition.x);

		lerpSpeed = Mathf.Pow (speed, Mathf.Abs (paddlePosition.y - puck.body.position.y));
		resetSpeed = Mathf.Pow(2, Mathf.Abs(body.position.y));


		if (puckPosition.x < paddlePosition.x && puckPosition.x > -paddlePosition.x) {			// inside bounds

			if (puckPosition.y > paddlePosition.y) {
				MoveUp (lerpSpeed);
			} else if (puckPosition.y < paddlePosition.y) {
				MoveDown (lerpSpeed);
			}

		} else if (puckPosition.x > paddlePosition.x || puckPosition.x < -paddlePosition.x) {		// outside bounds
			
			if (0 > paddlePosition.y) {
				MoveUp (resetSpeed);
			} else if (0 < paddlePosition.y) {
				MoveDown (resetSpeed);
			}

		}
	}

	void MoveUp(float speed) {
		body.velocity = Vector2.Lerp (body.velocity, new Vector2 (0, 1) * speed, distanceRatio + 0.01f);
	}
	void MoveDown(float speed) {
		body.velocity = Vector2.Lerp (body.velocity, new Vector2 (0, -1) * speed, distanceRatio + 0.01f);
	}

}
