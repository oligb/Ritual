using UnityEngine;
using System.Collections;

public class ParentRotator : MonoBehaviour {

	// Use this for initialization

	public float pieceRadius;
	public float rotationSpeed;
	public bool clockwise =true;
	public Transform player;
	public bool rotating=false;

	public GameObject nextParent;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.childCount >0){
			rotating=true;
			transform.localScale=new Vector3(1f,2f,1f);

			player.localPosition=transform.TransformDirection(transform.forward*pieceRadius);
		}
		else{
			rotating=false;
			transform.localScale=Vector3.one;
		}

		if(Input.GetKeyDown(KeyCode.U)  && rotating ){
			nextParent.transform.LookAt(player.position);
			player.parent=nextParent.transform;
		}

		//transform.LookAt(player);

		if(rotating && transform.childCount > 0){
			if(clockwise){
				transform.Rotate(Vector3.up*rotationSpeed);
			}
			else{
				transform.Rotate(Vector3.down*rotationSpeed);
			}
		}
	
	}
}
