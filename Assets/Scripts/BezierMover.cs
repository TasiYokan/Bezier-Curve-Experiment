using UnityEngine;
using System.Collections;

public class BezierMover : MonoBehaviour
{
    public BasicBezier path;
    public float speed;
    public int curId = 0;
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(MoveAlongPath());
    }

    // Update is called once per frame
    void Update()
    {
        if (path.CurvePoints != null && curId < path.CurvePoints.Count)
        {
            transform.position = path.CurvePoints[curId] + offset;

            Debug.DrawLine(path.CurvePoints[curId], transform.position, Color.green);
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            speed += 0.001f;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            speed -= 0.001f;
        }
        speed = Mathf.Max(0, speed);
    }

    private IEnumerator MoveAlongPath()
    {
        yield return null;

        while (curId < path.CurvePoints.Count - 1)
        {
            int previousId = curId;
            curId = path.GetPoint(curId, speed, offset);
            print("id is " + curId);

            if (curId == previousId && curId < path.CurvePoints.Count - 1)
            {
                offset = (path.CurvePoints[curId + 1] - path.CurvePoints[curId]) 
                    * Mathf.Clamp01((offset.magnitude +speed)/ (path.CurvePoints[curId + 1] - path.CurvePoints[curId]).magnitude);
            }
            else
            {
                offset = Vector3.zero;
            }

            transform.position = path.CurvePoints[curId] + offset;
            yield return null;
        }
    }
}
