using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ravine : MonoBehaviour {

	IEnumerator Fall (Damageable damagable) {
		float speed = 1f;
		GameObject rootObjectFaller = damagable.rootObject;

		rootObjectFaller.GetComponentInChildren<DeathEffect> ().deathParticle = null;
		rootObjectFaller.GetComponentInChildren<DeathEffect> ().toSpawnOnDestroy = null;
		rootObjectFaller.GetComponentInChildren<DamageableSounds> ().SwitchDeathToFalling ();
		rootObjectFaller.GetComponentInChildren<HasMovement> ().LockMovement ();

		while (rootObjectFaller.transform.localScale.magnitude > .2f && (rootObjectFaller.transform.localScale.x > 0f)) {
			rootObjectFaller.transform.localScale = new Vector3 (
				rootObjectFaller.transform.localScale.x - Time.deltaTime * speed, 
				rootObjectFaller.transform.localScale.y - Time.deltaTime * speed, 
				rootObjectFaller.transform.localScale.z - Time.deltaTime * speed
			);
			yield return new WaitForEndOfFrame ();
		}


		damagable.TakeDamage (GetComponent<Damager> (), 10000, 0f);


		rootObjectFaller.GetComponentInChildren<HasMovement> ().LockMovement ();

	}

	[SerializeField]
	private Hero player;

	void OnTriggerEnter2D (Collider2D other) {
		Damageable dmg = other.gameObject.GetComponent<Damageable> ();
		if (dmg != null) {
			Hero h = dmg.GetComponent<Hero> ();
			if (h != null) {
				player = h;
			} else {
				StartCoroutine(Fall (dmg));
			}
		}

	}

	void OnTriggerExit2D(Collider2D other) {
		Damageable dmg = other.gameObject.GetComponent<Damageable> ();
		if (dmg != null) {
			Hero h = dmg.GetComponent<Hero> ();
			if (h != null) {
				player = null;
			}
//			damageable = 
		}

	}

	public void Update() {
		if(player != null && !player.isDashing) {
			StartCoroutine( Fall (player.GetComponent<Damageable> ()));
		}

	}
}
