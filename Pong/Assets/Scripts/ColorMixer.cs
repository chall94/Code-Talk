using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMixer : MonoBehaviour {

	public static Color ColorMix (Color colorOne, Color colorTwo) {

		float r = 0;
		float g = 0;
		float b = 0;
		float a = 0;

		r = (colorOne.r + colorTwo.r) / 2;
		g = (colorOne.g + colorTwo.g) / 2;
		b = (colorOne.b + colorTwo.b) / 2;
		a = (colorOne.a + colorTwo.a) / 2;

		return new Color(r,g,b,a);
	}

	public static Color ColorMix (Color[] colors) {
		
		float r = 0;
		float g = 0;
		float b = 0;
		float a = 0;

		foreach (Color color in colors) {
			r += color.r;
			g += color.g;
			b += color.b;
			a += color.a;
		}

		r /= colors.Length;
		g /= colors.Length;
		b /= colors.Length;
		a /= colors.Length;

		return new Color(r,g,b,a);
	}
}
