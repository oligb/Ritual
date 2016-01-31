using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Segment : MonoBehaviour {
    
    public float baseLength;
    public int numberOfPiecesToChoose = 2;
    [HideInInspector]
    public ProcPathGenerator parentPath;
    [HideInInspector]
    public int segmentNumber;
    public List<GameObject> leftPieces;
    public List<GameObject> rightPieces; 
    public List<GameObject> roofPieces;
    public List<GameObject> coins;

	// Use this for initialization
	void Start () {
        List<int> pieces = new List<int>();
        for( int i = 0; i < numberOfPiecesToChoose; i++ ) {
            int index = Random.Range( 0, leftPieces.Count + rightPieces.Count - 1 );
            while( pieces.Contains(index) ) {
                index = Random.Range( 0, leftPieces.Count + rightPieces.Count - 1 );
            }
            pieces.Add( index );
        }
        foreach( int index in pieces ) {
            if( index < leftPieces.Count ) {
                leftPieces[index].SetActive(true);
            } else {
                rightPieces[ index - leftPieces.Count ].SetActive(true);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerEnter( Collider other ) {
        if( other.gameObject.CompareTag( "Player" ) ) {
            parentPath.PlayerEnteredSegment(segmentNumber);
        }
    }
    
    public void DisableSegment() {
        foreach( GameObject obj in leftPieces ) {
            obj.SetActive(false);
        }
        foreach( GameObject obj in rightPieces ) {
            obj.SetActive(false);
        }
        foreach( GameObject obj in roofPieces ) {
            obj.SetActive(false);
        }
        foreach( GameObject obj in coins ) {
            obj.SetActive(false);
        }
    }
   
}
