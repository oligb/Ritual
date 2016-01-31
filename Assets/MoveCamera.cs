using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	// Use this for initialization
	public float lerpMoveSpeed=.1f;
	public float lerpLookSpeed=.1f;
	public Transform target;
	public Transform player;
	public PlayerForces script;
	Vector3 playerLerpPos;
	public float extraFixVelocityBuffer=15f;

	void Start () {
		player=GameObject.Find("player").transform;
		target=GameObject.Find("CamTarget").transform;
		script=player.gameObject.GetComponent<PlayerForces>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		lerpMoveSpeed=script.velocity.Remap(0f,script.maxVelocity+extraFixVelocityBuffer,.3f,.05f);
		transform.position=Vector3.Lerp(transform.position,target.position,lerpMoveSpeed);
		Vector3 lerpedTarget=Vector3.Lerp(playerLerpPos,player.position,lerpLookSpeed);
		transform.LookAt(lerpedTarget);


		playerLerpPos=player.position;



	}
}
