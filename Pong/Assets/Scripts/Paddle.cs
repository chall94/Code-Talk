using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

	protected Rigidbody2D body;
	protected float verticalAxis;

	protected Vector3 originalPosition;

	[SerializeField][Tooltip("What is the starting height of the paddle?")][Range(4f, 8f)]
	protected float originalHeight = 6f;
	protected float currentHeight;

	[SerializeField][Tooltip("How small can the paddle get before it stops shrinking?")][Range(0.5f, 4f)]
	protected float minimumSize = 1.5f;

	[Tooltip("Every hit will reduce the paddle height by this amount:")][Range(0f, 2f)]
	public float shrinkModifierConstant = 0.2f;


	protected virtual void Start() {
		body = gameObject.GetComponent<Rigidbody2D> ();
		originalPosition = body.transform.position;
		gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x, originalHeight, gameObject.transform.localScale.z);
		currentHeight = originalHeight;
	}

	public void ResetSize(float proportion) {
		StartCoroutine (GradualReset (proportion));
	}

	private IEnumerator GradualReset(float proportion) {
		if (currentHeight < originalHeight * proportion ) {
			float heightDifference = originalHeight * proportion - currentHeight;
			while (heightDifference > 0) {
				currentHeight += shrinkModifierConstant;
				gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x, currentHeight, gameObject.transform.localScale.z);
				heightDifference -= 0.2f;
				yield return new WaitForSeconds (0.1f);
			}
		}
	}

	public void Shrink() {
		if (gameObject.transform.localScale.y >= minimumSize) {
			gameObject.transform.localScale -= new Vector3 (0, shrinkModifierConstant, 0);
			currentHeight = gameObject.transform.localScale.y;
		}
	}

	public void ResetXPosition() {
		gameObject.transform.position = originalPosition;
	}
}
