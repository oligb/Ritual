using UnityEngine;
using System.Collections;

public class RotateATad : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("Rot",1f);
	}

	void Rot(){

		transform.Rotate(Vector3.forward*Random.Range(-50f,50f));
	}
	// Update is called once per frame
	void Update () {
	
	}
}
