using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	public SoundSettingRandomizer pickUpSound;

	public enum PickupType {
		SWORD,
		DASHING_BOOTS,
		POISON_HEALTH,
		HEALTH,
		ANNKE
	}

	void PlayPickupSound() {
		pickUpSound.RandomizePlaySound ();
	}

	public PickupType myType;
	void OnTriggerEnter2D(Collider2D other) {
		PickupReceiver h = other.GetComponent<PickupReceiver> ();
		if (h != null) {
			h.DoPickup (this);
			if (pickUpSound != null) {
				PlayPickupSound ();
			}
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			gameObject.GetComponent<Collider2D> ().enabled = false;
			Destroy (gameObject, 2f);
		}
	}

}
