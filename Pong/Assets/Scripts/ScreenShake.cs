using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

	public Transform cameraTransform;
	private Vector3 initialPos;
	[HideInInspector]
	public float time;
	[Tooltip("How hard do you want the camera to shake?")][Range(0, 10)]
	public float intensity;
	[Tooltip("How quickly should the camera cooldown?")][Range(0, 10)]
	public float decayRate;

	void Start() {
		initialPos = cameraTransform.localPosition;
	}

	void Update() {
		if (time > 0) {
			cameraTransform.localPosition = initialPos + Random.insideUnitSphere * intensity;

			time -= Time.deltaTime * decayRate;
		}
		else {
			time = 0f;
			cameraTransform.localPosition = initialPos;
		}
	}

}