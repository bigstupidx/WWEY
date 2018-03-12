using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingEffect : MonoBehaviour {


	//container to manipulate position on...
	public Transform foreArm;


	public Animator swooshAnimation;

	public AnimationCurve swingRotationAnim;

	public  float minZSwordRotation = -30f;
	public float maxZSwordRotation =  100f;

	public float duration;



	// Use this for initialization
	void Start () {
//		StartCoroutine (Perform());
		DisableSword();
	}

	void EnableSword() {
//		foreArm.GetComponentInChildren<Collider2D>().enabled = true;
		foreArm.GetComponentInChildren<SpriteRenderer>().enabled = true;
	}

	void DisableSword() {
//		foreArm.GetComponentInChildren<Collider2D>().enabled = false;
		foreArm.GetComponentInChildren<SpriteRenderer>().enabled = false;
	}


	IEnumerator Perform() {
		EnableSword ();


		float directionsToEnd = maxZSwordRotation - minZSwordRotation;
		foreArm.localRotation =  Quaternion.Euler (0, 0, minZSwordRotation);
		swooshAnimation.SetTrigger ("Swoosh");

		if (OnSwingStart != null) {
			OnSwingStart ();
		}


		float time = 0f;
		while (time < duration) {
			float progress = time / duration;

			float currRatioOfComplete = swingRotationAnim.Evaluate (progress);
			float newRot = minZSwordRotation + (currRatioOfComplete * directionsToEnd);

			foreArm.localRotation =  Quaternion.Euler (0, 0, newRot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}



		DisableSword ();

		if (OnSwingEnd != null) {
			OnSwingEnd ();
		}
	}

	public void Swing() {
		StartCoroutine (Perform ());
	}

	public delegate void SwingStartAction();
	public delegate void SwingEndAction();

	public event SwingStartAction OnSwingStart;
	public event SwingStartAction OnSwingEnd;

}
