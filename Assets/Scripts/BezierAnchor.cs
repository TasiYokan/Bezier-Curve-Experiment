using UnityEngine;
using System.Collections;

public class BezierAnchor : MonoBehaviour
{
    private Vector3 m_position;
    private Vector3 m_dragOffset;

    public Vector3 Position
    {
        get
        {
            m_position = transform.position;
            return m_position;
        }

        set
        {
            m_position = value;
            transform.position = m_position;
        }
    }

    Vector3 DragOffset
    {
        get
        {
            return m_dragOffset;
        }

        set
        {
            m_dragOffset = value;
        }
    }

    private void OnMouseDown()
    {
        m_dragOffset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + m_dragOffset;
    }

    private void OnMouseUp()
    {

    }

    private Vector3 GetMouseWorldPos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }
}
