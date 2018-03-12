using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMimic : MonoBehaviour {

	[SerializeField]
	private GameObject evil;
	[SerializeField]
	private GameObject mouth;

	private IEnumerator Eat () {
		evil.SetActive (true);
		yield return new WaitForSeconds (0.66f);
		GetComponent<AudioSource> ().Play ();
		mouth.SetActive (true);
		yield return new WaitForSeconds (0.05f);
		 
		mouth.GetComponent<Collider2D> ().enabled = true;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<Hero>() != null) {
			StartCoroutine (Eat());
		}
	}
}
