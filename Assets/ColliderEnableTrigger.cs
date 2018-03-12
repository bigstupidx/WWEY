using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEnableTrigger : MonoBehaviour {


	public Collider2D optionalColliderToDisable;

	void Start() {
		DisableCollider ();
	}

	public void EnableCollider() {
		if (optionalColliderToDisable != null) {
			optionalColliderToDisable.enabled = true;
		} else {
			GetComponent<Collider2D> ().enabled = true;
		} 

	}

	public void DisableCollider() {
		if (optionalColliderToDisable != null) {
			optionalColliderToDisable.enabled = false;
		} else {
			GetComponent<Collider2D> ().enabled = false;
		}
	}

	public void Loop() {
		if (OnLooped != null) {
			OnLooped ();
		}
	} 

	public delegate void LoopedAction();
	public event LoopedAction OnLooped;
}
