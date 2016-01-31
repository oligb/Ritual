using UnityEngine;
using System.Collections;

public class RotateTrails : MonoBehaviour {

	// Use this for initialization
	public GameObject target;
	public float lerpSpeed=.1f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.rotation = Quaternion.Slerp(transform.rotation,target.transform.rotation,lerpSpeed);
	}
}
