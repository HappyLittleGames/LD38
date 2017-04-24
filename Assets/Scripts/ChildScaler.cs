using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildScaler : MonoBehaviour
{
    [SerializeField] private GameObject rootType = null;
    private List<Transform> m_normalChildren = new List<Transform>();
    private List<SemiClutter> m_specialChildren = new List<SemiClutter>();
    private GameObject m_scalingRoot = null;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        m_scalingRoot = Instantiate(rootType, player.transform);
        m_scalingRoot.transform.position = player.transform.position;

        m_normalChildren.Clear();
        FindScalableChildren();

        Debug.Log("Found " + m_specialChildren.Count + " Spawners.");
    }

    private void FindScalableChildren()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            SemiClutter foundClutterer = gameObject.transform.GetChild(i).gameObject.GetComponent<SemiClutter>();
            if (foundClutterer != null)
                m_specialChildren.Add(foundClutterer);
            else
                m_normalChildren.Add(gameObject.transform.GetChild(i));
        }
    }

    public void TransposeChildren(Vector3 offset)
    {
        foreach (Transform trans in m_normalChildren)
        {
            trans.position += offset;
        }
        foreach (SemiClutter clutterer in m_specialChildren)
        {
            clutterer.TransposeParticles(offset);
        }
    }

    public void ScaleChildren(Vector3 deltaScale)
    {
        /*

        //foreach (Transform trans in m_normalChildren)
        //{
        //    if (trans.localScale.y > 0)
        //    {
        //        trans.localScale += deltaScale;
        //    }
        //}

        // clutter needs some special care
        foreach (SemiClutter clutterer in m_specialChildren)
        {
            clutterer.ScaleParticles(deltaScale, m_scalingRoot);
        }

        // do that scaling doe
        m_scalingRoot.transform.localScale += deltaScale;

        foreach (SemiClutter clutterer in m_specialChildren)
        {
            clutterer.ReparentParticles();
        }

        m_scalingRoot.transform.localScale = Vector3.one;

        bug fixed!
        */
    }
    
}
