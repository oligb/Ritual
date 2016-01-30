using UnityEngine;
using System.Collections;

public class PlayerForces : MonoBehaviour {


	public float moveForce=50f;
	public float forwardSpeed=30f;
	public float jumpForce=10f;
	public float floatForce=1f;
	public float towardsWallForce=10f;

	public float velocity;
	public float maxVelocity = 50f;

	public float raycastRadius=5f;

	public float orientToPlaneLerpSpeed =.1f;
	GameObject currentPlane;



	public Vector3 currentPlaneNormal;
	// Use this for initialization
	public int wallsLayer;
	public bool onWall=false;
	public Collider[] hitColliders;

	public Rigidbody rbody;
	GameObject secretPlayer;

	public AnimationCurve speedCurve;
	public float speedUpTime;

	public float accelTimer=0f;
	public float accelScaler=5f;

	public float lerpSlowSpeed=.1f;
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



		//rbody.AddForce(new Vector3(inputX*moveForce,0f,inputY*moveForce));


		//wall logic





		hitColliders = Physics.OverlapSphere(transform.position, raycastRadius);
		Ray ray;
		if (hitColliders.Length>=2) {
			
			onWall=true;
		}
		else{
			onWall=false;
		}


		if(onWall){

			currentPlane=hitColliders[0].gameObject;
			ray= new Ray(transform.position,currentPlane.transform.position-transform.position);
			currentPlaneNormal= GetNormalFromRay(ray);
			Debug.DrawRay(transform.position,currentPlaneNormal);

			//currentPlaneNormal=currentPlane.transform.up;
			Quaternion target = Quaternion.LookRotation(currentPlane.transform.forward,currentPlaneNormal);
			secretPlayer.transform.rotation= Quaternion.Slerp(secretPlayer.transform.rotation,target,orientToPlaneLerpSpeed);
			// rbody.AddForce(-currentPlaneNormal*towardsWallForce);

			Vector3 inputVector=new Vector3(inputX*moveForce,0f,inputY*moveForce);
			Vector3 direction =secretPlayer.transform.rotation * inputVector;
			rbody.AddForce(direction);
		}
		else{
		//	secretPlayer.transform.rotation=transform.rotation;
			rbody.AddForce(new Vector3(inputX*moveForce,0f,forwardSpeed));
		}



	


		//jumps
		if(onWall && Input.GetKeyDown("space")){
			rbody.AddForce( currentPlaneNormal.normalized *jumpForce, ForceMode.Force);
		} else if(Input.GetKey("space")){
			Vector3 currentJumpVector=currentPlaneNormal.normalized;
			rbody.AddForce(currentJumpVector*floatForce,ForceMode.Force);
		}




		//forward accell


		float maxSpeedAttained=0f;

		if(Input.GetKey(KeyCode.W)){
			accelTimer+=Time.deltaTime;
			Vector3 localVel = transform.InverseTransformDirection(rbody.velocity);
			localVel.z=accelScaler*speedCurve.Evaluate(accelTimer/speedUpTime);
			rbody.velocity=transform.TransformDirection(localVel);
			maxSpeedAttained=localVel.z;
		}

		else{
			accelTimer=0f;
			accelTimer+=Time.deltaTime;
			Vector3 localVel = transform.InverseTransformDirection(rbody.velocity);
			localVel.z=Mathf.Lerp(maxSpeedAttained,0f,lerpSlowSpeed);
		}

		/*
		if(rbody.velocity.magnitude<=maxVelocity){
			velocity=rbody.velocity.magnitude;
			rbody.AddForce(secretPlayer.transform.forward*forwardSpeed);
		}
		*/


				
			
	}


	public static Vector3 GetNormalFromRay( Ray ray ) {
		RaycastHit hit = new RaycastHit();
		if ( Physics.Raycast( ray, out hit) ) {
			Debug.Log(hit.collider.gameObject.name);
			MeshCollider meshCollider = hit.collider as MeshCollider;
			Mesh mesh = meshCollider.sharedMesh;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
			Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
			Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
			Transform hitTransform = hit.collider.transform;
			p0 = hitTransform.TransformPoint(p0);
			p1 = hitTransform.TransformPoint(p1);
			p2 = hitTransform.TransformPoint(p2);
			return Vector3.Cross( (p1 - p0), (p2 - p0) );
		}
		return Vector3.zero;
	}






	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, raycastRadius);
	}
}
