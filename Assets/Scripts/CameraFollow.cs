using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	// Editor-Visible Fields
	public static CameraFollow instance;

	// Fields
	private Hero hero;

	// Methods
	public void AttachToHero (Hero h) {
		hero = h;
	}

	// Mono-Behavior Methods
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
	}

	void Update () {
		if (hero != null) {
			transform.position = new Vector3 (hero.transform.position.x, hero.transform.position.y, transform.position.z);
		}
	}
}
