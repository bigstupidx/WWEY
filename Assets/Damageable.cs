using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

	[SerializeField]
	private int maxHealth = 1;
	[SerializeField]
	private int health;


	public GameObject rootObject;

	public Rigidbody2D pushbackRigidbody;

	public GameObject thingWithMovement;
	private HasMovement movementToLock;

	public float HealthRatio {
		get { 
			return health / (float)maxHealth;
		}
	}


	void Awake() {
		health = maxHealth;
	}
	void Start() {
		if (thingWithMovement != null) {
			movementToLock = thingWithMovement.GetComponent<HasMovement> ();
		}
	}
	public int Health {
		get {
			return health;
		}
	}

	public delegate void DamagedAction(Damageable self, int amount);
	public event DamagedAction OnDamaged;

	public delegate void DiedAction(Damageable self);
	public event DiedAction OnDied;

	bool dying;
	public void TakeDamage(Damager source, int amount, float knockbackMag) {
		
		health -= amount;
		if (health > maxHealth) {
			health = maxHealth;
		}
		if (health < 0) {
			health = 0;
		}
		if (OnDamaged != null) {
			OnDamaged (this, amount);	
		}


		Transform opposedForceSource = source.optionalCenterOfMass != null ? source.optionalCenterOfMass : source.transform;
		Vector3 away = (transform.position - opposedForceSource.transform.position).normalized;

		TakeForce(away, knockbackMag);


		if (health <= 0 && !dying) {
			dying = true;
			if (OnDied != null) {
				OnDied (this);
			}	
		}
	}

	public void TakeForce(Vector3 away, float mag) {
		if (pushbackRigidbody != null) {
			pushbackRigidbody.AddForce (away.normalized * mag, ForceMode2D.Impulse);
		}


		if (thingWithMovement != null) {
			movementToLock.LockMovement ();
			StartCoroutine (UnlockAfterStun ());
		}
	}

	float unlockThreshold = .2f;

	IEnumerator UnlockAfterStun() {
		while (pushbackRigidbody.velocity.sqrMagnitude > unlockThreshold) {
			yield return null;
		}
		movementToLock.UnlockMovement ();
		if (OnKnockbackStunFinished != null) {
			OnKnockbackStunFinished ();
		}
	}

	public delegate void KnockbackStunFinished();
	public event KnockbackStunFinished OnKnockbackStunFinished;
}
