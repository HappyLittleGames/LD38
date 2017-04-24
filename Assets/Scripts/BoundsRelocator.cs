using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsRelocator : MonoBehaviour {

    [SerializeField] private List<GameObject> m_realmBounds;
    [SerializeField] private GameObject m_playerRoot = null;
    [SerializeField] private float m_realmScale = 700;
    public float offsetLimits = 6000;

    private void Start()
    {
        foreach (Transform realm in GameObject.FindGameObjectWithTag("Player").GetComponent<WorldColor>().GetRealms())
        {
            m_realmBounds.Add(realm.gameObject);
        }
        for (int i = 0; i < m_realmBounds.Count; ++i)
        {
            m_realmBounds[i].transform.position = new Vector3(0, i * offsetLimits - 500, 0);
        }
    }

    void Update ()
    {
		foreach (GameObject bounds in m_realmBounds)
        {
            if (m_playerRoot != null)
            {
                if (bounds.transform.position.y < m_playerRoot.transform.position.y + -offsetLimits * 2) // we need more than exactly once offset because the world color uses these positions :):)):)
                {
                    bounds.transform.position = new Vector3(m_playerRoot.transform.position.x, offsetLimits * (m_realmBounds.Count - 1), m_playerRoot.transform.position.z);
                    bounds.transform.localScale = Vector3.one * m_realmScale;
                }
            }
        }
	}

    public void TransposeRealms(Vector3 offset)
    {
        foreach (GameObject realm in m_realmBounds)
        {
            realm.transform.position += offset;
        }
    }
}
