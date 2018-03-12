using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour {

	[SerializeField]
	private bool winsYouTheGame;
	[SerializeField]
	private float textDrawSpeed;
	[SerializeField]
	private GameObject textCanvas;

	[SerializeField]
	private bool closeEnoughToTalk = false;
	private bool isTalking = false;
	private Vector3 closed;
	private Vector3 open;

	// Methods
	public void Talk() {
		StartCoroutine (CreateText ());
	}

	public void Shuddap() {
		StartCoroutine (CloseText ());
	}

	private IEnumerator CreateText() {
		float progress = 0;
		float increment = textDrawSpeed;
		while (progress < 1) {
			textCanvas.transform.localScale = Vector3.Lerp(closed, open, progress);
			progress += increment;
			yield return new WaitForSeconds (0.01f);
		}
		isTalking = true;
	}

	private IEnumerator CloseText() {
		float progress = 0;
		float increment = textDrawSpeed;
		while (progress < 1) {
			textCanvas.transform.localScale = Vector3.Lerp(open, closed, progress);
			progress += increment;
			yield return new WaitForSeconds (0.01f);
		}
		textCanvas.transform.localScale = closed; // won't close all the way without this.
		isTalking = false;
	}

	// Mono-Behavior Methods
	void Awake () {
		closed = new Vector3 (textCanvas.transform.localScale.x, 0, textCanvas.transform.localScale.z);
		open = new Vector3 (textCanvas.transform.localScale.x, textCanvas.transform.localScale.y, textCanvas.transform.localScale.z);
	}

	void Start () {
		textCanvas.transform.localScale = closed;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<Hero> () != null) {
			closeEnoughToTalk = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.GetComponent<Hero> () != null) {
			closeEnoughToTalk = false;
		}
	}

	void Update () {
		if (closeEnoughToTalk && Input.GetKeyDown(KeyCode.R) && !isTalking) {
			Talk ();
		}

		if (winsYouTheGame && isTalking && Input.GetKeyDown(KeyCode.Alpha8)) {
			GameManager.instance.CallVictory ();
		}

		if (isTalking && !closeEnoughToTalk) {
			Shuddap ();
		}
	}
}
