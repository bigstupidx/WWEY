using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {

	[SerializeField]
	private float drawSpeed;
	[SerializeField]
	private GameObject mainPanel;
	[SerializeField]
	private GameObject clickText;

	private bool isOpen = false;
	private Vector3 closed;
	private Vector3 open;

	// Methods
	public void CleanupText() {
		StartCoroutine (CloseSequence());
	}

	public void Say(string text) {
		mainPanel.GetComponentInChildren<Text> ().text = text;
	}

	private IEnumerator CreateSequence() {
		yield return new WaitForSeconds (0.25f); // This yield should be set to wait for an already created text to close.
		float progress = 0;
		float increment = drawSpeed;
		while (progress < 1) {
			mainPanel.transform.localScale = Vector3.Lerp(closed, open, progress);
			progress += increment;
			yield return new WaitForSeconds (0.01f);
		}
		isOpen = true;
	}

	private IEnumerator CloseSequence() {
		float progress = 0;
		float increment = drawSpeed;
		while (progress < 1) {
			mainPanel.transform.localScale = Vector3.Lerp(open, closed, progress);
			progress += increment;
			yield return new WaitForSeconds (0.01f);
		}
		Destroy (this.gameObject);
	}

	// Mono-Behavior Methods
	void Awake () {
		closed = new Vector3 (mainPanel.transform.localScale.x, 0, mainPanel.transform.localScale.z);
		open = new Vector3 (mainPanel.transform.localScale.x, mainPanel.transform.localScale.y, mainPanel.transform.localScale.z);
	}

	void Start () {
		mainPanel.transform.localScale = closed;
		StartCoroutine (CreateSequence ());
	}

	void Update () {
		if (isOpen) {
			if (Input.GetKeyDown(KeyCode.E)) {
				CleanupText ();
			}
		}
	}
}
