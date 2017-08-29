using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Paddle {

	[Tooltip("Use *VerticalWASD* or *VerticalArrow* to specify control scheme")]
	public string verticalAxisName;

	[Tooltip("Adjust the movement speed of the paddle")][Range(5, 20)]
	public float speed = 10f;

	void FixedUpdate() {
		verticalAxis = Input.GetAxis (verticalAxisName);
		body.velocity = new Vector2 (0, verticalAxis * speed);
	}

}
	