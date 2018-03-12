using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettingRandomizer : MonoBehaviour {

	public float pitchVariance = .2f;
	public float volumeVariance = .2f;

	AudioSource source;

	float startPitch;
	float startVolume;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		startPitch = source.pitch;
		startVolume = source.volume;
	}

	public void RandomizePlaySound() {
//		Debug.Log ("Playing sound for " + name);
		source.pitch = startPitch +  Random.Range (-pitchVariance, pitchVariance);
		source.volume= startVolume +  Random.Range (-volumeVariance, volumeVariance);
		source.Play ();
	}

}
