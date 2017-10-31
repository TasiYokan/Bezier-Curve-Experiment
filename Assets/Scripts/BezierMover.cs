using UnityEngine;
using System.Collections;

public class BezierMover : MonoBehaviour
{
    public BasicBezier bezierPath;
    public float speed;
    private int m_curId = 0;
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
        if (bezierPath.WithinCurveRange(m_curId))
        {
            transform.position = bezierPath.CurvePoints[m_curId] + m_offset;

            // If the offset is too tiny, we could hardly notice it actually
            //Debug.DrawLine(path.CurvePoints[curId], transform.position, Color.green);
        }

        // For debug
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speed += 0.001f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            speed -= 0.001f;
        }
        //speed = Mathf.Max(0, speed);
    }

    private IEnumerator MoveAlongPath()
    {
        yield return null;

        while (bezierPath.WithinCurveRange(m_curId)
            && bezierPath.NotGoingToOutOfRange(m_curId, speed.Sgn()))
        {
            int previousId = m_curId;
            m_curId = bezierPath.GetPoint(m_curId, speed, ref m_offset);
            //print("id is " + m_curId + " offset " + m_offset.magnitude);

            transform.position = bezierPath.CurvePoints[m_curId] + m_offset;
            if (bezierPath.NotGoingToOutOfRange(m_curId, speed.Sgn()))
                transform.forward = bezierPath.GetCurveVector(m_curId, speed.Sgn());
            yield return null;
        }

        print("Finish moving");
    }
}
