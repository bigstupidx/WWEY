﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToStart : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1)){
			GameManager.instance.SpawnHero ();
		}

	}
}
