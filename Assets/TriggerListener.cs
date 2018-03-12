using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerListener : MonoBehaviour {
	

	void OnTriggerEnter2D(Collider2D other) {
		if (OnTriggerEntered != null) {
			OnTriggerEntered (other);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (OnTriggerExited != null) {
			OnTriggerExited (other);
		}
	}

	public delegate void  TiggerEnteredAction(Collider2D other);
	public event TiggerEnteredAction OnTriggerEntered;
	public delegate void  TiggerExitedAction(Collider2D other);
	public event TiggerExitedAction OnTriggerExited;




}
