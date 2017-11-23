using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BezierCurve : MonoBehaviour
{
    private List<BezierPoint> m_points;
    private List<BezierFragment> m_fragments;

    public bool isAutoConnect;
    private LineRenderer m_lineRenderer;

    public int totalSampleCount = 10;

    // Use this for initialization
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();

        m_points = GetComponentsInChildren<BezierPoint>().ToList();
        m_points.Sort((lhs, rhs) =>
        {
            return lhs.gameObject.name.CompareTo(rhs.gameObject.name);
        });

        m_fragments = new List<BezierFragment>();
        for (int i = 0; i < m_points.Count - 1; i++)
        {
            m_fragments.Add(new BezierFragment(m_points[i], m_points[i + 1], totalSampleCount / m_points.Count));
        }

        if (isAutoConnect && m_points.Count > 1)
        {
            m_fragments.Add(new BezierFragment(m_points[m_points.Count - 1], m_points[0], totalSampleCount / m_points.Count));
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawDebugCurve();
    }

    private void DrawDebugCurve()
    {
        int totalPos = 0;
        foreach (var frag in m_fragments)
        {
            frag.UpdateSamplePos();
            totalPos += frag.SampleCount - 1;
        }

        totalPos++;
        m_lineRenderer.positionCount = totalPos;

        int curPos = 0;
        for (int i = 0; i < m_fragments.Count; ++i)
        {
            for (int j = 0; j < m_fragments[i].SamplePos.Count - 1; ++j)
            {
                m_lineRenderer.SetPosition(curPos + j, m_fragments[i].SamplePos[j]);
            }

            curPos += m_fragments[i].SamplePos.Count - 1;
        }

        List<Vector3> lastFragPoses = m_fragments[m_fragments.Count - 1].SamplePos;
        m_lineRenderer.SetPosition(totalPos - 1, lastFragPoses[lastFragPoses.Count - 1]);
    }
}
