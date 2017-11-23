using UnityEngine;
using System.Collections;

public class BezierHandle : MonoBehaviour, IDraggable
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

    Vector3 IDraggable.DragOffset
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

    void IDraggable.OnDragged(Vector3 _dragStartPos)
    {
        m_dragOffset = transform.position - _dragStartPos;
        transform.parent.GetComponent<BezierPoint>().ActiveHandle = this;
    }

    void IDraggable.OnDropped(Vector3 _dragEndPos)
    {
    }

    void IDraggable.OnDragStay(Vector3 _dragCurPos)
    {
        transform.position = _dragCurPos + m_dragOffset;
    }
}
