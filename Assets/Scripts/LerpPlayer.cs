using UnityEngine;
using System.Collections;

public class LerpPlayer : MonoBehaviour {

	// Use this for initialization
	public Transform otherPlayer;
	public float lerpSpeed=.1f;
	public bool trail=false;
	void Start () {

		if(trail){
			otherPlayer=transform.parent;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position=Vector3.Lerp(transform.position,otherPlayer.position,lerpSpeed);
	
	}
}
