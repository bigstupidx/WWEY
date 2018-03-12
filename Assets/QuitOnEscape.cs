using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnEscape : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public RectTransform root;

	bool shown = false;
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.OverscreenShown ()) {
			return;
		}	
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (shown) {
				root.gameObject.SetActive (false);	
			} else {
				root.gameObject.SetActive (true);
			}

			shown = !shown;

		}
	}

	public void Quit() {
		

		Application.Quit ();

	}
}
