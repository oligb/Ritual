using UnityEngine;
using System.Collections;

public class TriggerPickup : MonoBehaviour {

	// Use this for initialization
	public Texture[] ramps;
	public int currentTex=0;


	public void Pickup(){
			Debug.Log("ye-");
		currentTex++;
		if(currentTex>ramps.Length){
			currentTex=0;
		}
		Camera.main.GetComponent<CC_GradientRamp>().rampTexture=ramps[currentTex];


	}
}
