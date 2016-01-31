using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineGenerator : MonoBehaviour {
    
    [HeaderAttribute("Setup")]
    public GameObject curveSegmentPrefab;
    public float radius;
    public int segmentsPerCurve;
    public float radiusFromCurve;
    public BezierControlPointMode preferedMode;
    [HeaderAttribute("Starting Generation")]
    public int curves;
    
    [SerializeField]
    private List<Vector3> points = new List<Vector3>();
    private List<BezierControlPointMode> modes = new List<BezierControlPointMode>();
    private bool loop;

	// Use this for initialization
	void Start () {
        for( int i = 0; i < curves; i++) {
            Debug.Log("Starting curve #" + i);
            GenerateNewCurveSegment();
            PlaceSegments();
        }
	}
    
    public void GenerateNewCurveSegment() {
        for( int i = 0; i < 4; i++ ) {
            if( points.Count == 0) {
                Vector3 firstPoint = Random.onUnitSphere * radius;
                points.Add( firstPoint );
            } else {
                Vector3 newPoint = points[ points.Count - 1] + Random.insideUnitSphere * radius;
                points.Add( newPoint );
            }
        }
        modes.Add( preferedMode );
        EnforceMode(points.Count - 4 );
    }
    
    public void PlaceSegments() {
        for( int i = 0; i < segmentsPerCurve; i++ ) {
            float t =  ( i * 1.0f ) / segmentsPerCurve;
            Vector3 center = GetPoint( t );
            Vector3 velocity = GetVelocity( t );
            // Debug.Log( "Time " + t + ", center " + center + ", velocity" + velocity );
            GameObject newSegment = Instantiate( curveSegmentPrefab, center, Quaternion.LookRotation( velocity.normalized, transform.up ) ) as GameObject;
            Vector3 segmentPosition = Random.onUnitSphere * radiusFromCurve;
            segmentPosition.z = 0;
            newSegment.transform.Translate( segmentPosition, Space.Self );
            newSegment.transform.LookAt( center );
            newSegment.transform.rotation *= Quaternion.Euler( Vector3.right * 90f);
        }
    }
    
    // ==============
    // Retained from example:
    // http://catlikecoding.com/unity/tutorials/curves-and-splines/
    // ==============
    
    public int ControlPointCount {
		get {
			return points.Count;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return points[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - points[index];
			if (loop) {
				if (index == 0) {
					points[1] += delta;
					points[points.Count - 2] += delta;
					points[points.Count - 1] = point;
				}
				else if (index == points.Count - 1) {
					points[0] = point;
					points[1] += delta;
					points[index - 1] += delta;
				}
				else {
					points[index - 1] += delta;
					points[index + 1] += delta;
				}
			}
			else {
				if (index > 0) {
					points[index - 1] += delta;
				}
				if (index + 1 < points.Count) {
					points[index + 1] += delta;
				}
			}
		}
		points[index] = point;
		EnforceMode(index);
	}

	public BezierControlPointMode GetControlPointMode (int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode (int index, BezierControlPointMode mode) {
		int modeIndex = (index + 1) / 3;
		modes[modeIndex] = mode;
		if (loop) {
			if (modeIndex == 0) {
				modes[modes.Count - 1] = mode;
			}
			else if (modeIndex == modes.Count - 1) {
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	private void EnforceMode (int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = preferedMode;
		if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Count - 1)) {
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			if (fixedIndex < 0) {
				fixedIndex = points.Count - 2;
			}
			enforcedIndex = middleIndex + 1;
			if (enforcedIndex >= points.Count) {
				enforcedIndex = 1;
			}
		}
		else {
			fixedIndex = middleIndex + 1;
			if (fixedIndex >= points.Count) {
				fixedIndex = 1;
			}
			enforcedIndex = middleIndex - 1;
			if (enforcedIndex < 0) {
				enforcedIndex = points.Count - 2;
			}
		}

		Vector3 middle = points[middleIndex];
		Vector3 enforcedTangent = middle - points[fixedIndex];
		if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
		}
		points[enforcedIndex] = middle + enforcedTangent;
	}

	public int CurveCount {
		get {
			return (points.Count - 1) / 3;
		}
	}

	public Vector3 GetPoint (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}
	
	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}
}
