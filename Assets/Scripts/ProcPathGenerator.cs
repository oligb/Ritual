using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProcPathGenerator : MonoBehaviour {
    
    [HeaderAttribute("Setup")]
    public GameObject curveSegmentPrefab;
    public float pointDistance;
    public float pointRadius;
    public int segmentsPerCurve;
    public float radiusFromCurve;
    public BezierControlPointMode preferedMode;
    [HeaderAttribute("Starting Generation")]
    
    public int leadingCurves;
    
    [SerializeField]
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> evaluatedPoints = new List<Vector3>();
    private bool makingNewCurves;
    private bool isMakingCurve;
    
    void OnDrawGizmos() {
        for( int i = 0; i < evaluatedPoints.Count; i++ ) {
            Gizmos.DrawWireSphere( evaluatedPoints[i], 1);
            if( i > 0) {
                Gizmos.DrawLine( evaluatedPoints[ i - 1], evaluatedPoints[ i ]);
            }
        }
    }

	// Use this for initialization
	void Start () {
        makingNewCurves = true;
        points.Add( transform.position );
        Vector3 secondPoint = transform.position;
        secondPoint.z += pointDistance;
        points.Add( secondPoint );
        // evaluatedPoints.Add( transform.position );
        for( int i = 0; i < curves; i++) {
            // Debug.Log("Starting curve #" + i);
            GenerateNewCurveSegment();
            PlaceSegments();
        }
        GetComponent<LineRenderer>().SetPositions( evaluatedPoints.ToArray() );
	}
    
    IEnumerator MaintainLeadingCurves() {
        while( makingNewCurves ) {
            if( points.Count - 1 < leadingCurves ) {
                StartCoroutine( BuildNewSegment() );
                GenerateNextPoint();
                if( isMakingCurve ) {
                    yield return null;
            } 
        }
                }
    }
    
    void GenerateNextPoint() {
        Vector3 prevDifference = points[points.Count - 1] - points[points.Count - 2];
        Vector3 newPoint = prevDifference.normalized * pointDistance + points[points.Count - 1];
    }
    
    IEnumerator BuildNewSegment() {
        
    }
    
    public void GenerateNewCurveSegment() {
        Vector3 velocity = transform.forward;;
        if( points.Count != 0 ) {
            velocity = GetVelocity( 0 );
        }
            
        for( int i = 0; i < 4; i++ ) {
            if( points.Count == 0) {
                Vector3 firstPoint = Random.onUnitSphere * radius;
                points.Add( firstPoint );
            } else {
                Vector3 newPoint = points[ points.Count - 1] + Random.insideUnitSphere * radius;
                
                // newPoint = Vector3.RotateTowards() (newPoint, velocity);
                
                points.Add( newPoint );
            }
        }
        modes.Add( preferedMode );
        EnforceMode(points.Count - 4 );
    }
    
    public void PlaceSegments() {
        for( int i = 1; i <= segmentsPerCurve; i++ ) {
            float t =  ( i * 1.0f ) / segmentsPerCurve;
            Vector3 center = GetPoint( t );
            evaluatedPoints.Add( center );
            
            Vector3 velocity = GetVelocity( t );
            // Debug.Log( "Time " + t + ", center " + center + ", velocity" + velocity );
            
            GameObject newSegment = Instantiate( curveSegmentPrefab, center, Quaternion.LookRotation( velocity.normalized, transform.up )  ) as GameObject;
            Vector3 segmentPosition = new Vector3( radiusFromCurve, 0, 0);
            segmentPosition.z = 0;
            newSegment.transform.Translate( segmentPosition, Space.Self );
            
            Vector3 newEuler = newSegment.transform.eulerAngles;
            newEuler.x = velocity.x;
            newEuler.y = velocity.y;
            
            Vector3 displacement = newSegment.transform.position - center;
            newEuler.z = displacement.normalized.z;
            newSegment.transform.eulerAngles = newEuler;
            
            // newSegment.transform.LookAt( center );
            // newSegment.transform.Rotate(90, 0, 90, Space.Self);
            // newSegment.transform.rotation *= Quaternion.Euler( newSegment.transform.forward * 90f);
        }
    }
    
    
}
