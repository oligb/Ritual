using UnityEngine;
using System.Collections;

public class RotateTrails : MonoBehaviour {

	// Use this for initialization
	public GameObject target;
	public PlayerForces script;
	public float lerpSpeed=.1f;
	void Start () {
		script=GameObject.Find("player").GetComponent<PlayerForces>();
	}
	
	// Update is called once per frame
	void Update () {
	

		if(script.onWall){
			transform.rotation = Quaternion.Slerp(transform.rotation,target.transform.rotation,lerpSpeed);

		}
		else{
		transform.Rotate(Vector3.forward*10f);
	}
	}
}
