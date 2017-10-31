using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicBezier : MonoBehaviour
{
    private List<Transform> controlPoints;
    public LineRenderer lineRenderer;
    public int SEGMENT_COUNT = 50;

    private List<Vector3> m_curvePoints;
    private List<Vector3> realControlPoints;

    public List<Vector3> CurvePoints
    {
        get
        {
            if (m_curvePoints == null)
                m_curvePoints = new List<Vector3>();
            return m_curvePoints;
        }

        set
        {
            m_curvePoints = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        List<Component> components = new List<Component>(this.GetComponentsInChildren(typeof(Transform)));
        controlPoints = components.ConvertAll(c => (Transform)c);

        controlPoints.Remove(this.transform);

        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        realControlPoints = new List<Vector3>();
    }

    public bool WithinCurveRange(int _id)
    {
        return _id >= 0 && _id < CurvePoints.Count;
    }

    public bool AtCurveEnds(int _id)
    {
        return _id == 0 || _id == CurvePoints.Count - 1;
    }

    public bool NotGoingToOutOfRange(int _id, int _step)
    {
        return _id + _step >= 0 && _id + _step < CurvePoints.Count;
    }

    public Vector3 GetCurveVector(int _id, int _step)
    {
        return CurvePoints[_id + _step] - CurvePoints[_id];
    }

    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        //for (int i = 1; i <= SEGMENT_COUNT; i++)
        //{
        //    float t = i / (float)SEGMENT_COUNT;
        //    Vector3 pixel = CalculateCubicBezierPoint(
        //        t,
        //        controlPoints[0].position,
        //        controlPoints[1].position,
        //        controlPoints[2].position,
        //        controlPoints[3].position);
        //    //lineRenderer.SetVertexCount(((j * SEGMENT_COUNT) + i));
        //    lineRenderer.positionCount = i;
        //    lineRenderer.SetPosition((i - 1), pixel);
        //}


        //print("angle between " + Vector3.SignedAngle(
        //    controlPoints[2].position - controlPoints[3].position,
        //    controlPoints[1].position - controlPoints[0].position, Vector3.back));

        GetAllPoints();
        lineRenderer.positionCount = m_curvePoints.Count;
        for (int i = 0; i < m_curvePoints.Count; ++i)
        {
            lineRenderer.SetPosition((i), m_curvePoints[i]);
        }
    }

    private void GetAllPoints()
    {
        realControlPoints.Clear();

        for (int i = 0; i < 4; ++i)
        {
            realControlPoints.Add(controlPoints[i].position);
        }

        CurvePoints.Clear();
        for (int i = 0; i <= SEGMENT_COUNT; i++)
        {
            float t = i / (float)SEGMENT_COUNT;
            Vector3 points = CalculateCubicBezierPoint(
                t,
                realControlPoints[0],
                realControlPoints[1],
                realControlPoints[2],
                realControlPoints[3]);

            CurvePoints.Add(points);
        }


        Debug.DrawLine(realControlPoints[1], realControlPoints[0], Color.red);
        Debug.DrawLine(realControlPoints[3], realControlPoints[2], Color.red);
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    public int GetPoint(int _id, float _speedInAFrame, ref Vector3 _offset)
    {
        if (_speedInAFrame.Sgn() == 0)
        {
            return _id;
        }

        // Is this necessary?
        GetAllPoints();

        int step = _speedInAFrame.Sgn();
        float totalDistance = 0;
        int i = _id;

        float offsetLength = _offset.magnitude;
        if (Vector3.Dot(_offset, GetCurveVector(i, 1)).Sgn() < 0)
        {
            offsetLength = -offsetLength;
        }

        while (totalDistance.FloatLess(step * (_speedInAFrame + offsetLength)))
        {
            i += step;
            if (WithinCurveRange(i))
            {
                totalDistance += (m_curvePoints[i] - m_curvePoints[i - step]).magnitude;
            }
            else
            {
                _offset = Vector3.zero;
                return i - step;
            }
        }

        i -= step;

        if (i == _id)
        {
            _offset = GetCurveVector(i, step) * Mathf.Clamp01(step * (_speedInAFrame + offsetLength) / totalDistance);
        }
        else
        {
            Vector3 curVector = GetCurveVector(i, step);
            _offset = curVector *
                Mathf.Clamp01(curVector.magnitude - (totalDistance - (_speedInAFrame + offsetLength)) / curVector.magnitude);
        }

        return i;
    }
}
