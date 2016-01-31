using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProcPathGenerator : MonoBehaviour {
    
    [HeaderAttribute("Setup")]
    public List<GameObject> curveSegmentPrefab;
    public float pointDistance;
    public float pointRadius;
    public int segmentsPerCurve;
    public Vector2 maxAngle;
    public Vector2 minAngle;
    [HeaderAttribute("Starting Generation")]
    
    public int leadingCurves;
    
    [SerializeField]
    private List<Vector3> points = new List<Vector3>();
    private bool makingNewCurves;
    private bool isMakingCurve;
    [SerializeField]
    private Vector2 currentAngling;
    [SerializeField]
    private Transform helperObject;
    
    void OnDrawGizmos() {
        for( int i = 0; i < points.Count; i++ ) {
            Gizmos.DrawWireSphere( points[i], 1);
            if( i > 0) {
                Gizmos.DrawLine( points[ i - 1], points[ i ]);
            }
        }
    }

	// Use this for initialization
	void Start () {
        makingNewCurves = true;
        
        Vector3 firstPoint = transform.position + transform.rotation * Vector3.forward * -pointDistance;
        points.Add( firstPoint );
        
        points.Add( transform.position );
        
        StartCoroutine( MaintainLeadingCurves() );
	}
    
    IEnumerator MaintainLeadingCurves() {
        while( makingNewCurves ) {
            if( points.Count - 1 < leadingCurves ) {
                
                BuildNewSegment();
                GenerateNextPoint();
                yield return null;
            } else {
                yield return null;
            } 
        }
    }
    
    void GenerateNextPoint() {
        
        // Choose point from last direction
        Vector3 prevDifference = points[points.Count - 1] - points[points.Count - 2];
        Vector3 newPoint = prevDifference.normalized * pointDistance + points[points.Count - 1];
        
        // Choose direciton
        Vector3 offset = Random.onUnitSphere;
        currentAngling.x += offset.x;
        currentAngling.y += offset.y;
        
        // Only correct direciton if direction correction is permitted
        if( maxAngle.x != minAngle.x ) {
            if( currentAngling.x > maxAngle.x ) {
                currentAngling.x = maxAngle.x;
            } else if( currentAngling.x < minAngle.x ) {
                currentAngling.x = minAngle.x;
            }
        }
        if( maxAngle.y != minAngle.y ) {
            if( currentAngling.y > maxAngle.y ) {
                currentAngling.y = maxAngle.y;
            } else if( currentAngling.y < minAngle.y ) {
                currentAngling.y = minAngle.y;
            }
        }
        
        offset *= pointRadius;
        offset.z = 0; 
        
        // Implement direction
        helperObject.position = points[points.Count - 1];
        helperObject.LookAt(newPoint, transform.up);
        helperObject.position = newPoint;
        helperObject.Translate( offset, Space.Self);
        points.Add( helperObject.position );
    }
    
    void BuildNewSegment() {
        
        // Choose angle
        Vector3 angle = points[points.Count - 1] - points[points.Count - 2];
        
        // Choose prefab
        int randomPrefab = Random.Range(0, curveSegmentPrefab.Count );
        GameObject newSegment = Instantiate(
          curveSegmentPrefab[randomPrefab],
          points[points.Count - 1],
          Quaternion.LookRotation(angle.normalized, transform.up)  
        ) as GameObject;
        newSegment.BroadcastMessage("AssembleSegment", pointDistance);
        
        // delay between making curves
    }
    
    
    
}
