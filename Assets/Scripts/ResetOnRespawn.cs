using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnRespawn : MonoBehaviour {

	[SerializeField]
	private EnemyID eyeDee;
	private Vector3 startPosition;
	private float startDelay;
	private AggroGroup startGroup;
	private bool notKilled = false;

	public enum EnemyID {
		TREE,
		ROCK,
		SAVE,
		PLANT
	}

	void Reset(Hero h){
		GameObject reset = null;

		switch (eyeDee) { // THIS IS NOT DRY, YOU MUST MAKE CHANGES BELOW AS WELL AS HERE
		case (EnemyID.ROCK):
			reset = Instantiate (GameManager.instance.rockEnemyFab);
			break;
		case (EnemyID.TREE):
			reset = Instantiate (GameManager.instance.treeEnemyFab);
			break;
		case (EnemyID.SAVE):
			reset = Instantiate (GameManager.instance.saveEnemyFab);
			break;
		case (EnemyID.PLANT):
			reset = Instantiate (GameManager.instance.plantEnemyFab);
			break;
		}

		if (reset.GetComponent<AggroMimic>() != null) {
			AggroMimic mimic = reset.GetComponent<AggroMimic> ();
			mimic.getUpDelay = startDelay;
			mimic.aggGroup = startGroup;
			if (startGroup != null) {
				startGroup.mimicGroup.Add (mimic);
				startGroup.RegisterTrackingMimic (mimic);
			}
		}

		if (reset != null) {
			reset.transform.position = startPosition;
			notKilled = true;
			Destroy (this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		if (GetComponent<AggroMimic>() != null) {
			startDelay = GetComponent<AggroMimic> ().getUpDelay;
			startGroup = GetComponent<AggroMimic> ().aggGroup;
		}
		GameManager.instance.OnHeroRespawn += Reset;
	}

	void OnDestroy () {
		if (!notKilled) {
			DeadGuyToRespawn me = new DeadGuyToRespawn (startPosition, eyeDee, startGroup, startDelay);
			GameManager.instance.respawnList.Add (me);
		}
		GameManager.instance.OnHeroRespawn -= Reset;
	}

	public class DeadGuyToRespawn {

		private AggroGroup groupWith;
		private float delay;
		private Vector3 spawnHere;
		private EnemyID iidd;

		public DeadGuyToRespawn (Vector3 pos, EnemyID id, AggroGroup g, float delay) {
			iidd = id;
			groupWith = g;
			spawnHere = pos;
		}

		public void Respawn(){
			GameObject reset = null;

			switch (iidd) {
			case (EnemyID.ROCK):
				reset = Instantiate (GameManager.instance.rockEnemyFab);
				break;
			case (EnemyID.TREE):
				reset = Instantiate (GameManager.instance.treeEnemyFab);
				break;
			case (EnemyID.SAVE):
				reset = Instantiate (GameManager.instance.saveEnemyFab);
				break;
			case (EnemyID.PLANT):
				reset = Instantiate (GameManager.instance.plantEnemyFab);
				break;
			}

			if (reset.GetComponent<AggroMimic>() != null) {
				AggroMimic mimic = reset.GetComponent<AggroMimic> ();
				mimic.getUpDelay = delay;
				mimic.aggGroup = groupWith;
				if (groupWith != null) {
					groupWith.mimicGroup.Add (mimic);
					groupWith.RegisterTrackingMimic (mimic);
				}
			}

			if (reset != null) {
				reset.transform.position = spawnHere;
			}
		}
	}
}
