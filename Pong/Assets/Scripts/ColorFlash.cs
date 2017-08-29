using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour {

	private Renderer render;
	private Color originalColor = Color.white;
	private Color puckColor = Color.white;

	[HideInInspector]
	public bool impacted;
	private bool cooldown;
	[SerializeField]
	private float flashTime = 0.1f;
	private float limit = 0;
	private float controlTime = 0f;

	void Start() {
		render = gameObject.GetComponent<Renderer> ();
		originalColor = render.material.color;
		limit = flashTime / 2;
	}

	void Update() {

		if (controlTime > limit - 0.05f && impacted) {
			impacted = false;
			cooldown = true;
			controlTime = 0f;
			render.material.color = puckColor;
		}

		if (controlTime < limit && impacted) {
			render.material.color = Color.Lerp (render.material.color, puckColor, controlTime);
			controlTime += Time.deltaTime * 2;
		}

		if (controlTime > limit - 0.05f && cooldown) {
			cooldown = false;
			controlTime = 0f;
			render.material.color = originalColor;
		}

		if (controlTime < limit && cooldown) {
			render.material.color = Color.Lerp (render.material.color, originalColor, controlTime);
			controlTime += Time.deltaTime;
		}

	}

	public void Flash (Color hitColor) {
		puckColor = hitColor;
		impacted = true;
		controlTime = 0f;
	}

	public void Reset() {
		render.material.color = Color.white;
		Debug.Log ("Resetting Top Wall and Bottom Wall colors");
	}
}
