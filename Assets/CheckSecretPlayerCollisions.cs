using UnityEngine;
using System.Collections;

public class CheckSecretPlayerCollisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnCollisionStay(Collision col){
		Debug.Log("yep");
		GetComponentInParent<PlayerForces>().SecretPlayerCollision(col );
	}
}
