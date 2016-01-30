using UnityEngine;
using System.Collections;

public class LerpPlayer : MonoBehaviour {

	// Use this for initialization
	public Transform otherPlayer;
	public float lerpSpeed;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position=Vector3.Lerp(transform.position,otherPlayer.position,.1f);
	
	}
}
