using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

	[SerializeField]
	private GameObject lights;

	private IEnumerator lastActiveIEnumerator;

	public bool isFirst = false;

	void Start() {
		GameManager.instance.CheckInSavePoint (this);
	}

	// Methods
	public void DecorateAsCurrentSave() {
		if (lastActiveIEnumerator != null) {
			StopCoroutine (lastActiveIEnumerator);
		}
		lastActiveIEnumerator = SetSaveAnimation ();
		StartCoroutine (lastActiveIEnumerator);
	}

	public void DecorateAsNotCurrentSave() {
		if (lastActiveIEnumerator != null) {
			StopCoroutine (lastActiveIEnumerator);
		}
		lastActiveIEnumerator = UnsetSaveAnimation ();
		StartCoroutine (lastActiveIEnumerator);
	}

	private IEnumerator SetSaveAnimation() {
		lights.SetActive (true);
		yield return new WaitForSeconds (1.00f);
		GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
	}

	private IEnumerator UnsetSaveAnimation() {
		yield return new WaitForSeconds (1.00f);
		lights.SetActive (false);
		GetComponent<SpriteRenderer> ().color = new Color (0.75f, 0.75f, 0.75f, 1f);
	}

	// Mono-Behavior Methods
	void Awake () {
		GetComponent<SpriteRenderer> ().color = new Color (0.75f, 0.75f, 0.75f, 1f);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<Hero> () != null) {
			GameManager.instance.SetSave (this);
		}
	}
}
