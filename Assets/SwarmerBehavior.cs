using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerBehavior : MonoBehaviour {
	public  Animator animations;


	// Use this for initialization
	void Start () {

		GetComponent<AggroMimic>().OnGotUp += GotUp;
		GetComponent<AggroMimic>().OnSatDown += SatDown;
	}

	void GotUp(AggroMimic self)
	{
		animations.SetTrigger ("Seeking");
	}

	void SatDown(AggroMimic self) {
		animations.SetTrigger ("Idle");	
	}

}
