using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour {



	public float lerpSpeed=.1f;

	public AnimationCurve curve;
	public float scale=10f;
	GameObject otherTrailsObj;

	public Transform target;
	void Start () {

		otherTrailsObj=GameObject.Find("otherTrails");
	}




	void OnTriggerEnter( Collider col){
		if(col.tag=="Player"){
	Destroy(GetComponent<SphereCollider>());
	target= otherTrailsObj.GetComponent<AssignPickup>().Assign();
		otherTrailsObj.GetComponentInParent<TriggerPickup>().Pickup();

	StartCoroutine("Collected");
		}
}


// Update is called once per frame
void FixedUpdate () {
	if(target !=null){
		transform.position=Vector3.Lerp(transform.position,target.position,lerpSpeed);
	}
}


IEnumerator Collected(){

	Vector3 startSize=transform.localScale;
	float i=0f;
	while(i<=1.1f){
			transform.localScale=curve.Evaluate(i)*scale*Vector3.one;
		i+=Time.deltaTime/1f;
		yield return null;
	}
}
}


