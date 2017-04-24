using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbitraryOrbit : MonoBehaviour
{
    [SerializeField] private Vector3 m_rotation;
    [SerializeField] private float m_rotationRate;
    [SerializeField] private GameObject m_orbital = null;
    private float m_startOffsetZ = 12.0f;
    private float m_maxOffsetZ = 24.0f;
    // Update is called once per frame

    public void Start()
    {
        m_maxOffsetZ = m_startOffsetZ * 4;
        if (m_orbital != null)
            m_startOffsetZ = m_orbital.transform.localPosition.z;
    }

    public void Update()
    {
        if (m_rotationRate > 0)
        {
            m_orbital.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(m_startOffsetZ, m_maxOffsetZ, Mathf.Abs(gameObject.transform.localRotation.y / 1)));
            if (m_orbital != null)
            {
                gameObject.transform.localRotation *= Quaternion.Euler(m_rotation.normalized * m_rotationRate * Time.deltaTime);
            }
        }
    }
}
