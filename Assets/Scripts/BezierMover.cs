using UnityEngine;
using System.Collections;

public class BezierMover : MonoBehaviour
{
    public BasicBezier path;
    public float speed;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(MoveAlongPath());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator MoveAlongPath()
    {
        yield return null;

        int pointId = 0;
        while(pointId < path.CurvePoints.Count-1)
        {
            pointId = path.GetPoint(speed * Time.deltaTime);
            print("id is " + pointId);
            transform.position = path.CurvePoints[pointId];
            yield return null;
        }
    }
}
