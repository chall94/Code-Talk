using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour {

	private Renderer render;
	private ScreenShake screen;
	[HideInInspector]
	public int health;
	[Tooltip("How many points are needed to win a round?")][Range(1, 20)]
	public int maxHealth = 3;
	[HideInInspector]
	public bool isBroken = false;

	void Start() {
		render = gameObject.GetComponent<Renderer> ();
		screen = GameObject.FindObjectOfType<ScreenShake> ();
		StartCoroutine(ResetHealth ());
	}

	public void damage() {
		if (health - 1 <= 0) {
			isBroken = true;
			// Debug.Log ("Wall Broken!");
		} else {
			health -= 1;
		}
		screen.time = 0.5f;
		print(gameObject.name + " damaged (" + health + "/3 health remaining)");
	}

	public void ColorMark(Color puckColor) {
		render.material.color = ColorMixer.ColorMix (render.material.color, puckColor);
	}

	public IEnumerator ResetHealth() {
		health = maxHealth;
		isBroken = false;
		print("Resetting Health of " + gameObject.name + " to 3/3");
		render.material.color = Color.white;
		yield return new WaitForSeconds (0f);
	}

}