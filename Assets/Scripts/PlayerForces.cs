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
	public GameObject currentPlane;



	public Vector3 currentPlaneNormal;
	// Use this for initialization
	public int wallsLayer;
	public bool onWall=false;
	public bool landed=false;
	public Collider[] hitColliders;

	public Rigidbody rbody;
	GameObject secretPlayer;
	GameObject extraSecretPlayer;

	public AnimationCurve speedCurve;
	public float speedUpTime;

	public float accelTimer=0f;
	public float accelScaler=5f;

	public float lerpSlowSpeed=.1f;


	public float pitchUpTime=1f;
	public float volumeFaloffSpeed=.01f;
	public AudioSource source;
	// in my start function... set enemy layer

	void Start () {
		source=GetComponent<AudioSource>();
		secretPlayer=transform.GetChild(0).gameObject;
		extraSecretPlayer=GameObject.Find("visualPlayer");
		rbody=GetComponent<Rigidbody>();
		wallsLayer = 1 << LayerMask.NameToLayer("Walls");
	}


	void OnCollisionEnter(Collision col){
		GameObject planeInQuestion=col.gameObject;
		foreach( Collider collider in hitColliders){
			if(collider.gameObject != currentPlane && collider.gameObject !=gameObject){
				currentPlane=collider.gameObject;
				StopCoroutine("PitchUp");
				StartCoroutine("PitchDown");
				//source.pitch=1f;
				//source.time=0f;
				//source.Play();
			}

		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		float inputX=Input.GetAxis("Horizontal");
		float inputY=Input.GetAxis("Vertical");



		//rbody.AddForce(new Vector3(inputX*moveForce,0f,inputY*moveForce));


		//wall logic





		hitColliders = Physics.OverlapSphere(transform.position, raycastRadius);
		//currentPlane=hitColliders[0].gameObject;
		Ray ray;
		if (hitColliders.Length>=2) {
			
			onWall=true;
		}
		else{
			onWall=false;
		}

		Quaternion target;

		if(onWall){
			source.volume+=volumeFaloffSpeed*3f;
			if(!landed){
				source.Play();
				landed=true;
			}


			//ray= new Ray(transform.position,currentPlane.transform.position-transform.position);

			currentPlaneNormal=currentPlane.transform.up;
			target = Quaternion.LookRotation(currentPlane.transform.forward,currentPlaneNormal);

			//target=Quaternion.LookRotation(rbody.velocity.normalized,currentPlaneNormal);
			secretPlayer.transform.rotation= Quaternion.Slerp(secretPlayer.transform.rotation,target,orientToPlaneLerpSpeed);
			rbody.AddForce(-currentPlaneNormal*towardsWallForce);

			Vector3 inputVector=new Vector3(inputX*moveForce,0f,inputY*moveForce);
			Vector3 direction =secretPlayer.transform.rotation * inputVector;
			rbody.AddForce(direction);

	//		float randoStartTime=Random.Range(0f,source.clip.length-10f);

			//source.Play();
		}
		else{
			source.volume-=volumeFaloffSpeed;
			//source.Stop();

			//source.time=0f;
		//	secretPlayer.transform.rotation=transform.rotation;
		//	rbody.AddRelativeForce(new Vector3(inputX*moveForce,0f,forwardSpeed));

			secretPlayer.transform.rotation=Quaternion.LookRotation(rbody.velocity.normalized);
			Vector3 inputVector=new Vector3(inputX*moveForce,0f,0f);
			//Vector3 direction =secretPlayer.transform.rotation * inputVector;

			rbody.AddForce(inputVector);
		}



	


		//jumps
		if(onWall && Input.GetKeyDown("space")){
			
			StartCoroutine("PitchUp");
		//	Invoke("ToggleLanded",.3f);
			//source.time=0f;
			//source.Stop();
			rbody.AddForce(currentPlaneNormal*jumpForce,ForceMode.Impulse);
		}

		if(Input.GetKey("space")){
			Vector3 currentJumpVector=currentPlaneNormal;
			rbody.AddForce(currentJumpVector*floatForce);
		}


		//forward accell

		if(rbody.velocity.magnitude<=maxVelocity){

		rbody.AddRelativeForce(inputY*Vector3.forward*accelScaler);
		}
		else{
			rbody.velocity=rbody.velocity.normalized*maxVelocity;
	//		rbody.AddRelativeForce(inputY*Vector3.back*accelScaler*2f);
		}


		/*
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


				
		velocity=rbody.velocity.magnitude;


		extraSecretPlayer.transform.rotation=Quaternion.LookRotation(rbody.velocity.normalized);
	}

	IEnumerator PitchUp(){
		float startPitch=source.pitch;
		float i=0f;

		while (i<=1f){
			source.pitch=i.Remap(0f,1f,startPitch,1.5f);
			i+=Time.deltaTime/pitchUpTime;
			yield return null;
		}

	}
	IEnumerator PitchDown(){
		float startPitch=source.pitch;
		float i=0f;

		while (i<=1f){
			source.pitch=i.Remap(0f,1f,startPitch,.9f);
			i+=Time.deltaTime/pitchUpTime;
			yield return null;
		}

	}

	public void ToggleLanded(){

		landed=false;
	}

	public void SecretPlayerCollision(Collision col){
		currentPlane=col.gameObject;
			Ray ray = new Ray(transform.position,col.contacts[0].point-transform.position);
			currentPlaneNormal=GetNormalFromRay(ray).normalized;

	}



	public static Vector3 GetNormalFromRay( Ray ray ) {
		RaycastHit hit = new RaycastHit();
		if ( Physics.Raycast( ray, out hit) ) {
			Debug.Log(hit.collider.gameObject.name);
			MeshCollider meshCollider = hit.collider as MeshCollider;
			Mesh mesh = meshCollider.sharedMesh;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			Debug.Log(hit.triangleIndex);
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
