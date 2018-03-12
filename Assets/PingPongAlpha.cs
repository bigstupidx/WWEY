using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongAlpha : MonoBehaviour {


	private SpriteRenderer myRenderer;
	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<SpriteRenderer> ();
	}


	float time;
	public float speed = 2f;
	// Update is called once per frame
	void Update () {
		float alpha = Mathf.PingPong (time, 1f); 	

		myRenderer.color = new Color (myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, alpha);
		time += Time.deltaTime * speed;
	}
}
