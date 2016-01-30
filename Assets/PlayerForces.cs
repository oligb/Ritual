using UnityEngine;
using System.Collections;

public class PlayerForces : MonoBehaviour {


	public float moveForce=50f;
	public float forwardSpeed=30f;
	public float jumpForce=10f;
	public float towardsWallForce=10f;


	public float raycastRadius=5f;
	public GameObject currentPlane;

	public Vector3 currentPlaneNormal;
	// Use this for initialization
	public int wallsLayer;
	public bool onWall=false;
	public Collider[] hitColliders;

	public Rigidbody rbody;
	GameObject secretPlayer;

	// in my start function... set enemy layer

	void Start () {
		secretPlayer=transform.GetChild(0).gameObject;
		rbody=GetComponent<Rigidbody>();
		wallsLayer = 1 << LayerMask.NameToLayer("Walls");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float inputX=Input.GetAxis("Horizontal");
		float inputY=Input.GetAxis("Vertical");

		rbody.AddForce(new Vector3(inputX*moveForce,inputY*moveForce,forwardSpeed));

		hitColliders = Physics.OverlapSphere(transform.position, raycastRadius);
		if (hitColliders.Length>=2) {
			onWall=true;
		}
		else{
			onWall=false;
		}
		if(onWall){
			currentPlaneNormal=hitColliders[0].transform.forward;
			secretPlayer.transform.rotation = Quaternion.LookRotation(currentPlaneNormal);
			rbody.AddForce(-currentPlaneNormal*towardsWallForce);
		}

		if(Input.GetKeyDown("space")){
			rbody.AddForce(currentPlaneNormal*jumpForce,ForceMode.Impulse);
		}
				
			
	}
}
