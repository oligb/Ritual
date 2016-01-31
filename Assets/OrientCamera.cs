using UnityEngine;
using System.Collections;

public class OrientCamera : MonoBehaviour {

	// Use this for initialization
	public Transform parent;
	public Vector3 currentPlaneDir;
	public float lerpSpeed=.1f;
	public PlayerForces playerScript;
	void Start () {
		playerScript=parent.gameObject.GetComponent<PlayerForces>();

		StartCoroutine("CamOrient");
	}
	
	// Update is called once per frame
	IEnumerator CamOrient () {

		while(true){
		lerpSpeed=playerScript.velocity.Remap(0f,playerScript.maxVelocity,.5f,0f);
			currentPlaneDir=playerScript.currentPlaneNormal;

		Quaternion target = Quaternion.LookRotation(-currentPlaneDir,Vector3.up);
		Quaternion parentForward = Quaternion.LookRotation(parent.transform.forward,Vector3.up);
		transform.rotation=Quaternion.Slerp(parentForward,target,lerpSpeed);
			yield return null;
		}

		//ta
		/*rget=Quaternion.LookRotation(rbody.velocity.normalized,currentPlaneNormal);
		secretPlayer.transform.rotation= Quaternion.Slerp(secretPlayer.transform.rotation,target,orientToPlaneLerpSpeed);

		Quaternion rot = Quaternion.LookRotation(parent.position-transform.position,Vector3.up);
		transform.rotation=Quaternion.Slerp(transform.rotation,rot,.1f);
		*/
	
	}
}
