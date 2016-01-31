using UnityEngine;
using System.Collections;

public class AssignPickup : MonoBehaviour {

	// Use this for initialization
	int numActive=-1;
	public TrailRenderer[] renderers;
	void Start () {
		renderers=GetComponentsInChildren<TrailRenderer>();
	}

	public Transform  Assign(){
		numActive++;
		return renderers[numActive].gameObject.transform;


	}
	// Update is called once per frame
	void Update () {
	
	}
}
