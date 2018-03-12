using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupReceiver : MonoBehaviour {
	
	public Hero hero;

	public GameObject poisonCloudPrefab;



	public void DoPickup(Pickup pu) {
		Debug.Log ("pickup : " + pu.myType);
		switch (pu.myType) {
		case Pickup.PickupType.SWORD:
			hero.gear.sword = true;
			break;
		case Pickup.PickupType.DASHING_BOOTS:
			hero.gear.boots = true;
			break;
		case Pickup.PickupType.HEALTH:
			hero.GetComponent<Damageable> ().TakeDamage (hero.GetComponentInChildren<Damager> (), -10, 0f);
			break;
		case Pickup.PickupType.POISON_HEALTH:
			GameObject cloud = Instantiate (poisonCloudPrefab);
			cloud.transform.position = transform.position;
			Debug.Log ("HEREHRH!!");
			hero.GetComponent<Damageable> ().TakeDamage (pu.GetComponent<Damager>(), 2, 3f);
			break;
		case Pickup.PickupType.ANNKE:
			hero.gear.ankhe = true;
			break;
		}
	}
}
