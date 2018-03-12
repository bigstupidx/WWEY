using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHeartOnDeath : MonoBehaviour {


	public static int sinceLastDrop = 0;

	public GameObject healthPrefab;
	public GameObject poisonPrefab;

	public Damageable damageable;

	public float chanceIncre = .1f;
	public float baseToBeat = .95f;

	// Use this for initialization
	void Start () {
		damageable.OnDied += (Damageable self) => {
			TrySpawnHeart();
		};
	}

	void TrySpawnHeart() {
		if (RollSuccess ()) {
			//23% to get poison
			if (Random.Range (0f, 1f) > .77f) {
				StartCoroutine (DelayDrop (poisonPrefab, transform.position));
			} else {
				StartCoroutine (DelayDrop (healthPrefab, transform.position));
			}
		}		
	}

	IEnumerator DelayDrop(GameObject drop, Vector3 pos) {
		
		yield return new WaitForSeconds (.5f);

		GameObject heart = Instantiate (drop);
		heart.transform.position = pos;
	}

	bool RollSuccess() {
		float toBeat = baseToBeat - (chanceIncre * sinceLastDrop);
		sinceLastDrop++;
		if (Random.Range(0f, 1f) > toBeat) {
			sinceLastDrop = 0;
			return true;
		}

		return false;
	}

}
