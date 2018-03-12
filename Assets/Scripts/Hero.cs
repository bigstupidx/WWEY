using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, HasMovement {

	// Editor-Visible Fields
	public Transform waist;
	public Transform shoulders;


	[System.Serializable]
	public class GearUnlockModule
	{
		public bool sword;
		public bool ankhe;
		public bool boots;
	}

	public SoundSettingRandomizer swingSound;
	public SoundSettingRandomizer dashSound;

	public GearUnlockModule gear = new GearUnlockModule();

	public float swingCooldown = 1f;

	public  SwordSwingEffect swordSwing;

	public Vector3 lastMoveDirection;


	[SerializeField]
	private int damage;
	[SerializeField]
	private float speed;

	private bool lockMovement;

	public void LockMovement() {
		lockMovement = true;
	}
	public void UnlockMovement() {
		lockMovement = false;
	}



	void Start() {
		CameraFollow.instance.AttachToHero (this);

		swordSwing.OnSwingStart += () => {
			LockMovement();
		};

		swordSwing.OnSwingEnd += () => {
			StartCoroutine(DelayUnlockMovement());
		};


	}

	IEnumerator DelayUnlockMovement() {
		yield return new WaitForSeconds (.1f);
		UnlockMovement ();
	}
		
	void Update() {
		CheckForMovementAndRotation ();

		if (Input.GetMouseButtonDown (0)) { //left click
			SwingSword();
		}

		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("try to dash");
			Dash ();
		}
	}

	void CheckForMovementAndRotation() {

		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");

		Vector3 intendedDirection = new Vector3 (x, y, 0f);
		if (intendedDirection.sqrMagnitude > 1f) {
			intendedDirection = intendedDirection.normalized;
		}
		lastMoveDirection = intendedDirection;

		if (lockMovement) {
//			Debug.Log ("movement is locked!");
			return;
		}

		//should be a value between 0, 1
		FeetAnimator ().speed = intendedDirection.magnitude;


		transform.Translate (intendedDirection * Time.deltaTime * speed);

		if(Mathf.Abs(x) > 0f || Mathf.Abs(y) > 0f)
			waist.localRotation = Quaternion.Euler(0, 0, Util.ZDegFromDirection(-x, y));

		shoulders.localRotation = Quaternion.Euler (0, 0, -Util.ZDegFromDirection (DirectionsToMouseInWorld()));
	}



	Vector2 DirectionsToMouseInWorld() {
		return Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
	}



	Animator FeetAnimator() {
		return waist.GetComponentInChildren<Animator> ();
	}

	Animator SwordAnimator() {
		return shoulders.GetComponentInChildren<Animator> ();
	}


	private bool dashCoolingDown = false;
	public bool isDashing = false;
	public float dashForce;
	public float dashCooldown = .5f;
	public void Dash() {
		if (!gear.boots)
			return;

		if (isDashing || dashCoolingDown)
			return;

		if (lastMoveDirection.x != 0f || lastMoveDirection.y != 0f) {
			dashSound.RandomizePlaySound ();
			isDashing = true;
			Damageable dmg = GetComponent<Damageable> ();
			dmg.TakeForce (lastMoveDirection.normalized, dashForce);
			dmg.OnKnockbackStunFinished += FinishDashAndStartCD;
		}
	}

	void SwingSword() {
		if (!gear.sword)
			return;

		if (swordCooling || lockMovement)
			return;

		swingSound.RandomizePlaySound ();
		swordSwing.Swing();
		StartCoroutine(DoSwordCooldown ());
	}

	bool swordCooling = false;
	IEnumerator DoSwordCooldown() {
		swordCooling = true;
		yield return new WaitForSeconds (swingCooldown);
		swordCooling = false;
	}

	private void FinishDashAndStartCD() {
		GetComponent<Damageable> ().OnKnockbackStunFinished -= FinishDashAndStartCD;
		StartCoroutine (DoDashingCooldown());
	}

	IEnumerator DoDashingCooldown() {
		isDashing = false;
		dashCoolingDown = true;
		yield return new WaitForSeconds (dashCooldown);
		dashCoolingDown = false;
	}
}

public class Util {
	public static float ZDegFromDirection(Vector2 vect2) {
		return ZDegFromDirection (vect2.x, vect2.y);
	}

	public static float ZDegFromDirection(float x, float y) {
		return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
	}
}
