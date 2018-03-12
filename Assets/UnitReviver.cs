using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitReviver : MonoBehaviour {
	
	[SerializeField]
	private GameObject toReviveFab;
	[SerializeField]
	private Vector3 startLocation;
	private bool available = false;

	void Revive () {
		GameObject man = Instantiate (toReviveFab);
		man.transform.position = startLocation;
		Destroy (this.gameObject);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<Hero> () != null) {
			if (other.gameObject.GetComponent<Hero>().gear.ankhe) {
				available = true;
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.GetComponent<Hero> () != null) {
			available = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (available && Input.GetKeyDown(KeyCode.R)) {
			Revive ();
		}
	}
}
