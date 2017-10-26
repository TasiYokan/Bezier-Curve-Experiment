using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierTest : MonoBehaviour
{
    private List<Transform> controlPoints;
    public LineRenderer lineRenderer;
    public int SEGMENT_COUNT = 50;

    private List<Vector3> curvePoints;
    private List<Vector3> realControlPoints;

    // Use this for initialization
    void Start()
    {
        List<Component> components = new List<Component>(this.GetComponentsInChildren(typeof(Transform)));
        controlPoints = components.ConvertAll(c => (Transform)c);

        controlPoints.Remove(this.transform);

        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        curvePoints = new List<Vector3>();
        realControlPoints = new List<Vector3>();
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
        lineRenderer.positionCount = curvePoints.Count;
        for (int i = 0; i < curvePoints.Count; ++i)
        {
            lineRenderer.SetPosition((i), curvePoints[i]);
        }
    }

    private void GetAllPoints()
    {
        realControlPoints.Clear();

        if (Vector3.Angle(
            controlPoints[2].position - controlPoints[3].position,
            controlPoints[1].position - controlPoints[0].position) < 180)
        {
            for (int i = 0; i < 4; ++i)
            {
                realControlPoints.Add(controlPoints[i].position);
            }

            float factor = 0.8f;
            Vector3 midPoint = (1 - factor) * controlPoints[0].position + factor * controlPoints[3].position;
            Vector3 p1 = (midPoint + Quaternion.Euler(0, 0, 90) * (controlPoints[2].position - controlPoints[3].position));
            //Vector3 p2 = (midPoint + Quaternion.Euler(0, 0, -90) * (controlPoints[2].position - controlPoints[3].position));
            //Vector3 p2 = midPoint - (p1 - midPoint) * 0.8f;
            Vector3 firstPoint, secondPoint;
            if (
            Vector3.SignedAngle(
            controlPoints[2].position - controlPoints[3].position,
            controlPoints[1].position - controlPoints[0].position, Vector3.back) < 0)
            {
                midPoint = controlPoints[3].position + Quaternion.Euler(0, 0, 90) * (controlPoints[2].position - controlPoints[3].position);
                float angleBetweenlastAndMid = Vector3.Angle(
                    controlPoints[2].position - controlPoints[3].position,
                    controlPoints[3].position - controlPoints[1].position);
                float angleFactor = Mathf.Min(1, 10f / (angleBetweenlastAndMid + 0.1f));
                print("angle factor " + angleFactor);
                float mag = (controlPoints[2].position - controlPoints[3].position).magnitude * angleFactor;
                firstPoint = midPoint - (controlPoints[3].position - controlPoints[1].position).normalized * mag;
                secondPoint = midPoint + (controlPoints[3].position - controlPoints[1].position).normalized * mag;
            }
            else
            {
                midPoint = controlPoints[3].position + Quaternion.Euler(0, 0, -90) * (controlPoints[2].position - controlPoints[3].position);
                float mag = (controlPoints[2].position - controlPoints[3].position).magnitude;
                firstPoint = midPoint - (controlPoints[3].position - controlPoints[1].position).normalized * mag;
                secondPoint = midPoint + (controlPoints[3].position - controlPoints[1].position).normalized * mag;
            }

            realControlPoints.Insert(2, firstPoint);
            realControlPoints.Insert(3, midPoint);
            realControlPoints.Insert(4, midPoint);
            realControlPoints.Insert(5, secondPoint);

            curvePoints.Clear();
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                Vector3 points = CalculateCubicBezierPoint(
                    t,
                    realControlPoints[0],
                    realControlPoints[1],
                    realControlPoints[2],
                    realControlPoints[3]);

                curvePoints.Add(points);
            }

            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                Vector3 points = CalculateCubicBezierPoint(
                    t,
                    realControlPoints[4],
                    realControlPoints[5],
                    realControlPoints[6],
                    realControlPoints[7]);

                curvePoints.Add(points);
            }

            Debug.DrawLine(realControlPoints[0], realControlPoints[1], Color.red);
            Debug.DrawLine(realControlPoints[3], realControlPoints[2], Color.red);
            Debug.DrawLine(realControlPoints[4], realControlPoints[5], Color.red);
            Debug.DrawLine(realControlPoints[7], realControlPoints[6], Color.red);
        }
        else
        {
            for (int i = 0; i < 4; ++i)
            {
                realControlPoints.Add(controlPoints[i].position);
            }

            curvePoints.Clear();
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                Vector3 points = CalculateCubicBezierPoint(
                    t,
                    realControlPoints[0],
                    realControlPoints[1],
                    realControlPoints[2],
                    realControlPoints[3]);

                curvePoints.Add(points);
            }


            Debug.DrawLine(realControlPoints[1], realControlPoints[0], Color.red);
            Debug.DrawLine(realControlPoints[3], realControlPoints[2], Color.red);
        }
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
}
