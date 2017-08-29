using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// I used http://freemusicarchive.org/genre/Chiptune/ to find music tracks

	public AudioSource fxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle (AudioClip clip) {

		fxSource.clip = clip;
		fxSource.Play ();
	}

	public void PlayRandom (AudioClip[] clips) {
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (0.95f, 1.05f);

		fxSource.pitch = randomPitch;
		fxSource.clip = clips [randomIndex];
		fxSource.Play ();
	}

}
