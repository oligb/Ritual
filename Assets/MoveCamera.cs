using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	// Use this for initialization
	public float lerpMoveSpeed=.1f;
	public float lerpLookSpeed=.1f;
	public Transform target;
	public Transform player;
	Vector3 playerLerpPos;
	void Start () {
		player=GameObject.Find("player").transform;
		target=GameObject.Find("CamTarget").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.position=Vector3.Lerp(transform.position,target.position,lerpMoveSpeed);
		Vector3 lerpedTarget=Vector3.Lerp(playerLerpPos,player.position,lerpLookSpeed);
		transform.LookAt(lerpedTarget);

		playerLerpPos=player.position;

	}
}
