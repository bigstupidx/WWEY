using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HasMovement {
	void LockMovement();
	void UnlockMovement();
}

public class AggroMimic : MonoBehaviour, HasMovement {

	[SerializeField]
	private float speed;
	public float getUpDelay;

	public Hero heroToChase;
	private IEnumerator lastActiveIEnumerator;

	public TriggerListener aggroDomain;

	public SoundSettingRandomizer getUpSound;

	float soundPlayCooldown = 1f;
	bool coolingSound = false;
	void PlayGetUpSound() {
		if (coolingSound) {
			return;
		}
		getUpSound.RandomizePlaySound ();
		StartCoroutine (CooldownGetUpSound ());
	}

	IEnumerator CooldownGetUpSound() {
		coolingSound = true;
		yield return new WaitForSeconds (soundPlayCooldown);
		coolingSound = false;
	}

	void Awake () {
		if (aggGroup != null) {
			aggGroup.mimicGroup.Add (this);
		}
	}

	void Start() {
		aggroDomain.OnTriggerEntered += ThingEntered;
		aggroDomain.OnTriggerExited += ThingExited;
	}

	[SerializeField]
	bool movementLocked;
	public void LockMovement() {
		movementLocked = true;
	}
	public void UnlockMovement() {
		movementLocked = false;
	}


	// Methods
	private void GetUp() {
		PlayGetUpSound ();
		if (OnGotUp != null) {
			OnGotUp (this);
		}
	}

	private void SitDown() {
		if (OnSatDown != null) {
			OnSatDown (this);
		}
	}

	// Mono-Behavior Methods
	void ThingEntered (Collider2D other) {
//		Debug.Log ("ENTER");
		if (other.GetComponent<Hero>() != null) {
			
			heroToChase = other.GetComponent<Hero> ();
			StartCoroutine (DelayGetUp ());
		}
	}

	public bool delayingGetup = false;
	IEnumerator DelayGetUp() {
		delayingGetup = true;
		Debug.Log ("DELAYING: " + delayingGetup + CurrentlyChasing());
		yield return new WaitForSeconds (getUpDelay);
		if (heroToChase != null)
			GetUp ();
		delayingGetup = false;
	}

	void ThingExited (Collider2D other) {
//		Debug.Log ("EXIT");
		if (other.GetComponent<Hero>() != null) {
			SitDown ();
			heroToChase = null;
		}
	}

	public AggroGroup aggGroup;
	Hero HeroToChaseConsideringGroup() {
		if (aggGroup != null) {
			Hero groupone = aggGroup.HeroToChase ();
			if (groupone != null)
				return groupone;

			return heroToChase;
		} else {
			return heroToChase;
		}
	}

	void Update () {
		if (CurrentlyChasing()) {
//			Debug.Log ("DOING: " + delayingGetup + CurrentlyChasing());
			Hero toChase = HeroToChaseConsideringGroup ();
			transform.position = Vector3.MoveTowards (transform.position, toChase.transform.position, speed * Time.deltaTime);
			transform.localRotation = Quaternion.Euler (0, 0, -Util.ZDegFromDirection (toChase.transform.position - transform.position)  + 180f);
		}
	}

	public bool CurrentlyChasing() {
		return HeroToChaseConsideringGroup() != null && !movementLocked && !delayingGetup;
	}

	public delegate void GotUpAction(AggroMimic self);
	public event GotUpAction OnGotUp;
	public delegate void SitDownAction(AggroMimic self);
	public event SitDownAction OnSatDown;
}
