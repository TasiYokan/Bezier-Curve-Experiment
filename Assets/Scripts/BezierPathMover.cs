using UnityEngine;
using System.Collections;

public class BezierPathMover : MonoBehaviour
{
    public BezierCurve bezierPath;
    public float speed;
    private int m_curFragId = 0;
    private int m_curSampleId = 0;
    // Offset from corresponding curve point 
    private Vector3 m_offset;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(MoveAlongPath());
    }

    // Update is called once per frame
    void Update()
    {
        // Force to update position in case we change the path at runtime
        transform.position =
            bezierPath.Fragments[m_curFragId].SamplePos[m_curSampleId] + m_offset;

        // If the offset is too tiny, we could hardly notice it actually
        //Debug.DrawLine(path.CurvePoints[curId], transform.position, Color.green);

        // For debug
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speed *= 1.01f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            speed *= 0.99f;
        }
        //speed = Mathf.Max(0, speed);
    }

    private IEnumerator MoveAlongPath()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            bezierPath.GetCurvePos(ref m_curFragId, ref m_curSampleId, speed, ref m_offset);

            transform.position =
                bezierPath.Fragments[m_curFragId].SamplePos[m_curSampleId] + m_offset;

            transform.forward = bezierPath.GetSampleVectorAmongAllFrags(m_curFragId, m_curSampleId, speed.Sgn());

            yield return null;
        }
    }
}
