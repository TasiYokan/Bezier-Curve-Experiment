using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Single fragment curve with 2 points which consists the whole curve
/// </summary>
public class BezierFragment
{
    public BezierPoint startPoint;
    public BezierPoint endPoint;

    private int m_sampleCount;
    private List<Vector3> m_samplePos;

    public List<Vector3> SamplePos
    {
        get
        {
            return m_samplePos;
        }

        set
        {
            m_samplePos = value;
        }
    }

    public int SampleCount
    {
        get
        {
            return m_sampleCount;
        }

        set
        {
            m_sampleCount = value;
        }
    }

    public BezierFragment(BezierPoint _start, BezierPoint _end, int _sampleCount = 10)
    {
        startPoint = _start;
        endPoint = _end;
        m_sampleCount = _sampleCount;

        SamplePos = new List<Vector3>();
        UpdateSamplePos();
    }

    public void UpdateSamplePos()
    {
        SamplePos.Clear();
        for (int i = 0; i < SampleCount; ++i)
        {
            Vector3 pos = CalculateCubicBezierPos(i / (float)SampleCount);

            SamplePos.Add(pos);
        }
    }

    private Vector3 CalculateCubicBezierPos(float _t)
    {
        float u = 1 - _t;
        float t2 = _t * _t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * _t;

        Vector3 p = u3 * startPoint.Anchor.Position
            + t3 * endPoint.Anchor.Position
            + 3 * u2 * _t * startPoint.PrimaryHandle.Position
            + 3 * u * t2 * endPoint.SecondaryHandle.Position;

        return p;
    }
}
