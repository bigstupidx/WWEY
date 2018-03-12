using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour {

	[SerializeField]
	[TextArea(5, 5)]
	private string text;
	private bool triggered;
	private bool ontop;
	private GameObject spawnedText;

	void Reset (Hero notUsed) { // Not used.
		triggered = false;
	}

	IEnumerator DestroyRoutine() {
		yield return new WaitForSeconds(0.66f);
		if (GameManager.instance.CurrentText != null && !ontop) {
			GameManager.instance.CurrentText.CleanupText ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<Hero>() != null && !triggered) {
			GameManager.instance.SpawnText (text);
			//triggered = true;
			ontop = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<Hero> () != null) {
			StartCoroutine (DestroyRoutine());
			ontop = false;
		}
	}

	void Start () {
		//GameManager.instance.OnHeroRespawn += Reset;
	}

	void OnDestroy() {
		//GameManager.instance.OnHeroRespawn -= Reset;
	}
}
