using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingTrigger : MonoBehaviour {

	void Start() {
		GetComponent<Collider2D> ().enabled = false;
	}

	public void SwordSwingEnd() {
		Debug.Log ("Sword swung!");
		GetComponent<Collider2D> ().enabled = false;
	}

	public void SwordSwingSwoosh() {

		GetComponent<Collider2D> ().enabled = true;
		Debug.Log ("should collide!");
	}
}
