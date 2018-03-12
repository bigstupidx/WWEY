using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGhostDone : MonoBehaviour {

	public Transform toFollow;

	public bool dontFadeAway = false;

	//gets called from the animation event in the ghost animation
	public void DestroySelf() {
//		Debug.Log ("HERE!");
		GetComponent<Animator> ().speed = 0f;
//		Destroy (gameObject);
		StartCoroutine(FadeAway());
	}

	IEnumerator FadeAway() {
		if (dontFadeAway) {
			yield return null;
			Destroy (gameObject);
		} else {
			SpriteRenderer rend = GetComponent<SpriteRenderer> ();
			float runningA = 1f;
			float startSc = transform.localScale.x;
			while (runningA > 0f) {
				rend.color = new Color (rend.color.r, rend.color.g, rend.color.b, runningA);
				transform.localScale = new Vector3 (startSc + (1f - runningA), startSc + (1f - runningA), 1f);
				runningA -= Time.deltaTime * 2f;
				yield return new WaitForEndOfFrame ();
			}
			Destroy (gameObject);
		}

	}

	void Update() {
		if (toFollow != null) {
			transform.position = toFollow.position;
		}
	}
}
