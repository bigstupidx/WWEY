using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilTreeBehavior : MonoBehaviour {
	public AggroMimic mimic;

	public TriggerListener attackTrigger;
	public Animator attackAnimation;
	public float cooldown = 1f;
	public Hero toAttack;

	public SoundSettingRandomizer attackSound;

	// Use this for initialization
	void Start () {
		mimic.OnGotUp += (AggroMimic self) =>  {
			GetComponent<Animator>().SetTrigger("Seeking");
		};
		mimic.OnSatDown += (AggroMimic self) => {
			GetComponent<Animator>().SetTrigger("Idle");
		};


		attackTrigger.OnTriggerEntered += (Collider2D other) => {
			Hero h = other.GetComponent<Hero>();
			if(h != null) {
				toAttack = h;
				StartCoroutine(KeepAttacking());
			}
		};
		attackTrigger.OnTriggerExited += (Collider2D other) => {
			Hero h = other.GetComponent<Hero>();
			if(h != null) {
				toAttack = null;
			}
		};
	}


	void TryAttack() {
		if (cooling || mimic.delayingGetup) {
			return;
		}
		attackSound.RandomizePlaySound ();
		attackAnimation.gameObject.SetActive (true);
		attackAnimation.GetComponent<ColliderEnableTrigger> ().OnLooped += TurnOffWhenLooped;
		StartCoroutine (DoCooldown ());
	}

	void TurnOffWhenLooped() {
		attackAnimation.gameObject.SetActive (false);
		attackAnimation.GetComponent<ColliderEnableTrigger> ().OnLooped -= TurnOffWhenLooped;

	}

	IEnumerator KeepAttacking() {
		while (toAttack != null) {
			TryAttack ();
			yield return new WaitForSeconds (.1f);
		}
	}


	bool cooling = false;
	IEnumerator DoCooldown() {
		cooling = true;
		yield return new WaitForSeconds (cooldown);	
		cooling = false;
	}
}
