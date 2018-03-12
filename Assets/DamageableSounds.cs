using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableSounds : MonoBehaviour {


	public SoundSettingRandomizer damagedSound;
	public SoundSettingRandomizer diedSound;



	public Damageable damageable;


	// Use this for initialization
	void Start () {
		if (damageable == null) {
			damageable = GetComponent<Damageable> ();
		}	

		damageable.OnDamaged += (Damageable self, int amount) => {
			if(amount > 0) {
				damagedSound.RandomizePlaySound();	
			}


		};

		damageable.OnDied += (self) => {
			diedSound.RandomizePlaySound();
		};
	}

	public void SwitchDeathToFalling() {
		diedSound.GetComponent<AudioSource> ().clip = GameManager.instance.fallingClip;
	}


}
