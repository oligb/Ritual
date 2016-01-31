using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProcPathGenerator : MonoBehaviour {
    [HeaderAttribute("Player Systems")]
    
    public float distanceLeftPath;
    public float pathRegenTimeout = 10f;
    public ProcPathGenerator pathGeneratorPrefab;
    public GameObject newPathFloorPrefab;
    public Vector3 distanceForNewPath;
    public int leadingCurves;
    public int trailingCurves;
    public PlayerForces player;
    
    [HeaderAttribute("Setup")]
    public List<GameObject> curveSegmentPrefab;
    public float pointDistance;
    public float pointRadius;
    public int segmentsPerCurve;
    public Vector2 maxAngle;
    public Vector2 minAngle;
    
    
    
    
    [SerializeField]
    private List<Vector3> points = new List<Vector3>();
    private bool activePath;
    private bool isMakingCurve;
    [HeaderAttribute("Status")]
    [SerializeField]
    private Vector2 currentAngling;
    [SerializeField]
    private Transform helperObject;
    private int currentPlayerSegment;
    private List<Segment> segments;
    
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
        segments = new List<Segment>();
        activePath = false;
        
        Vector3 firstPoint = transform.position + transform.rotation * Vector3.forward * -pointDistance;
        points.Add( firstPoint );
        
        points.Add( transform.position );
        
        
        StartCoroutine( MaintainLeadingCurves() );
        StartCoroutine( PathRegenCheck() );
	}
    
    void Update() {
        
    }
    
    IEnumerator MaintainLeadingCurves() {
        activePath = true;
        while( activePath ) {
            if( points.Count - 1 < currentPlayerSegment + leadingCurves ) {
                Debug.Log("Making segment " + (segments.Count - 1) );
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
        helperObject.LookAt(newPoint, Vector3.up);
        helperObject.position = newPoint;
        helperObject.Translate( offset, Space.Self);
        points.Add( helperObject.position );
    }
    
    void BuildNewSegment() {
        
        // Choose angle
        Vector3 angle = points[points.Count - 1] - points[points.Count - 2];
        
        // Choose prefab
        int randomPrefab = Random.Range(0, curveSegmentPrefab.Count );
        GameObject newSegmentGO = Instantiate(
          curveSegmentPrefab[randomPrefab],
          points[points.Count - 1],
          Quaternion.LookRotation(angle.normalized, Vector3.up)  
        ) as GameObject;
        Segment newSegment = newSegmentGO.GetComponent<Segment>();
        segments.Add( newSegment );
        
        newSegment.parentPath = this;
        newSegment.segmentNumber = segments.Count - 1;
        segments.Add( newSegment );
        // newSegment.BroadcastMessage("AssembleSegment", pointDistance);
        
        // delay between making curves
    }
    
    public void PlayerEnteredSegment(int segment) {
        currentPlayerSegment = segment;
        if( currentPlayerSegment >= trailingCurves ) {
            Debug.Log( "Player at segment " + segment + ", disabled segment " + segment );
            segments[segment - trailingCurves].DisableSegment();
        } else {
            Debug.Log( "Player at segment " + segment);
        }
    }
    
    IEnumerator PathRegenCheck() {
        yield return new WaitForSeconds(pathRegenTimeout);
        while( activePath ) {
            Vector3 displacement = segments[currentPlayerSegment].transform.position - player.transform.position;
            if( displacement.magnitude > distanceLeftPath ) {
                Debug.Log( "Player left track from segment " + currentPlayerSegment );
                PlayerLeftTrack();
            }
            yield return null;
        }
    }
    
    void PlayerLeftTrack() {
        
        helperObject.position = player.transform.position;
        helperObject.rotation = Quaternion.identity;
        helperObject.Translate(0, -10f, 0, Space.World);
        Instantiate(
            newPathFloorPrefab,
            helperObject.position,
            helperObject.rotation
        );
        
        helperObject.position = player.transform.position;
        Vector3 displacement = player.transform.position - Camera.main.transform.position;
        helperObject.rotation = Quaternion.LookRotation( displacement.normalized, Vector3.up );
        helperObject.Translate( distanceForNewPath, Space.Self );
        GameObject newPathGO = Instantiate (
            pathGeneratorPrefab,
            helperObject.position,
            helperObject.rotation
        ) as GameObject;
        
        
        ProcPathGenerator newPath = newPathGO.GetComponent<ProcPathGenerator>();
        newPath.points = new List<Vector3>();
        newPath.currentAngling = new Vector2();
        newPath.segments = new List<Segment>();
        currentPlayerSegment = 0;
        GameObject helperObjectGO = Instantiate (
            helperObject.gameObject
        ) as GameObject;
        newPath.helperObject = helperObjectGO.GetComponent<Transform>();
        newPath.player = player;
        
        // begin destroying existing track
    }
    
    
    
}
