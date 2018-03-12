using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour {
	public Damageable damageable;


	public bool destroysOnDeath = true;
	public float destroyDelay = .8f;
	// Use this for initialization
	void Start () {
		damageable.OnDied += (Damageable self) => {
			if(deathParticle != null) {
				GameObject p = Instantiate(deathParticle);
				p.transform.position = transform.position;
				p.transform.localScale = self.rootObject.transform.localScale;

				DeathGhostDone eff = p.GetComponent<DeathGhostDone>();
				if(eff != null) {
					eff.toFollow = transform;
				}
			}


			StartCoroutine(DelayDestroy());


		};
	}

	IEnumerator DelayDestroy() {
		yield return new WaitForSeconds (destroyDelay);

		Destroy(gameObject);
		if (toSpawnOnDestroy != null) {
			GameObject go = Instantiate (toSpawnOnDestroy);
			go.transform.position = transform.position;
			if (toSpawnOnDestroy.GetComponent<UnitReviver>() == null) {
				GameManager.instance.propDestroyList.Add (go);
			}
		}
	}


	public GameObject deathParticle;

	public GameObject toSpawnOnDestroy;

}
