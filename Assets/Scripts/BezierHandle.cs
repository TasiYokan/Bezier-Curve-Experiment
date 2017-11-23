﻿using UnityEngine;
using System.Collections;

public class BezierHandle : MonoBehaviour
{
    private Vector3 m_position;

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
}
