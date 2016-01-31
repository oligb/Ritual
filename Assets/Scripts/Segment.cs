using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Segment : MonoBehaviour {
    
    public float baseLength;
    public int numberOfPiecesToChoose = 2;
    public List<GameObject> leftPieces;
    public List<GameObject> rightPieces; 
    public List<GameObject> roofPieces;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void AssembleSegment( float distance ) {
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
}
