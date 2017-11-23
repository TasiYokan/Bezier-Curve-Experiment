using UnityEngine;
using System.Collections;

public class BezierPoint : MonoBehaviour
{
    private BezierAnchor m_anchor;
    [SerializeField]
    private BezierHandle m_primaryHandle;
    [SerializeField]
    private BezierHandle m_secondaryHandle;
    private BezierHandle m_activeHandle;

    [SerializeField]
    private bool m_isAutoSmooth;

    public bool IsAutoSmooth
    {
        get
        {
            return m_isAutoSmooth;
        }

        set
        {
            m_isAutoSmooth = value;
        }
    }

    public BezierAnchor Anchor
    {
        get
        {
            if(m_anchor == null)
                m_anchor = GetComponentInChildren<BezierAnchor>();
            return m_anchor;
        }

        set
        {
            m_anchor = value;
        }
    }

    public BezierHandle PrimaryHandle
    {
        get
        {
            if (m_primaryHandle == null)
                m_primaryHandle = GetComponentInChildren<BezierHandle>();
            return m_primaryHandle;
        }

        set
        {
            m_primaryHandle = value;
        }
    }

    public BezierHandle SecondaryHandle
    {
        get
        {
            if (m_secondaryHandle == null 
                || m_secondaryHandle.gameObject.activeInHierarchy == false)
                m_secondaryHandle = PrimaryHandle;
            return m_secondaryHandle;
        }

        set
        {
            m_secondaryHandle = value;
        }
    }

    public BezierHandle ActiveHandle
    {
        get
        {
            return m_activeHandle;
        }

        set
        {
            m_activeHandle = value;
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        UpdatePosition();
    }

    /// <summary>
    /// Adjsut secondary handle to the opposite postion of primary one.
    /// </summary>
    private void SmoothHandle(bool _basedOnPrimary = true)
    {
        if (PrimaryHandle == null || SecondaryHandle == null)
            return;

        if (_basedOnPrimary)
        {
            SecondaryHandle.Position = 2 * Anchor.Position - PrimaryHandle.Position;
        }
        else
        {
            PrimaryHandle.Position = 2 * Anchor.Position - SecondaryHandle.Position;
        }
    }

    private void UpdatePosition()
    {
        if (IsAutoSmooth)
        {
            SmoothHandle(m_activeHandle != m_secondaryHandle);
        }
    }
}
