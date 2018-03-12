using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilRockBehavior : MonoBehaviour {


	public Animator seekingAnimation;


	bool shouldBeAttacking = false;

	void Start () {
		GetComponent<AggroMimic>().OnGotUp += (AggroMimic self) => {
			seekingAnimation.SetTrigger("Seeking");
		};
		GetComponent<AggroMimic>().OnSatDown += (AggroMimic self) => {
			seekingAnimation.SetTrigger("Idle");
		};
	}






}
