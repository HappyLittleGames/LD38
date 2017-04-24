using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiClutter : MonoBehaviour
{

    [SerializeField] protected float m_cullDistance = 1200;
    [SerializeField] protected Camera m_playerCamera = null;
    [SerializeField] protected GameObject m_sampleClutter = null;
    protected List<GameObject> m_activeParts = new List<GameObject>();
    protected List<GameObject> m_upperLayerParticles = new List<GameObject>();

    [SerializeField] protected float m_spawnRadius = 1;
    [SerializeField] protected float m_minRadius = 1;
    [SerializeField] protected float m_heightVariance = 0;
    [SerializeField] protected float m_startScaleModifier = 1;
    [SerializeField] protected float m_scaleModifier = 1;


    [SerializeField] protected int m_startupAmount;
    [SerializeField] protected int m_particleCap;
    protected int m_partsToPrep = 0;
    protected float m_smallPartScale = 0.1f;
    [SerializeField] protected bool m_drawDebug;

    protected void Start()
    {
        m_activeParts.Clear();
        m_upperLayerParticles.Clear();
        StartupPopulate();
    }

    public void Update()
    {
        PruneParticles();
        RefreshParticles();
    }

    protected virtual void StartupPopulate()
    {
        for (int i = 0; i < m_startupAmount; ++i)
        {
            AddRandomParticle(m_minRadius, m_spawnRadius, Random.Range(m_heightVariance * -1, m_heightVariance)/* this probably ought to be the scene root?*/);
        }
    }

    protected virtual void AddRandomParticle(float minRange, float radius, float height)
    {
        float sqrtRadius = Mathf.Sqrt(Random.Range(minRange, radius));
        float theta = Random.Range(0.0f, Mathf.PI * 2);
        Vector2 randomOnPlane = new Vector2(sqrtRadius * Mathf.Cos(theta), sqrtRadius * Mathf.Sin(theta)) * sqrtRadius;
        Vector3 randomLocation = new Vector3(randomOnPlane.x, m_playerCamera.transform.position.y + height, randomOnPlane.y);

        AddParticle(randomLocation);
    }

    protected virtual void AddParticle(Vector3 position)
    {
        m_upperLayerParticles.Add(Instantiate(m_sampleClutter, position, Random.rotation));
        m_upperLayerParticles[m_upperLayerParticles.Count - 1].transform.localScale = Vector3.one * m_startScaleModifier;
        m_upperLayerParticles[m_upperLayerParticles.Count - 1].transform.parent = this.transform;
    }

    protected void PruneParticles()
    {        
        m_partsToPrep = 0;
        for (int i = m_activeParts.Count - 1; i >= 0; --i)
        {
            if (m_activeParts[i] == null)
            {
                m_activeParts.RemoveAt(i);
                continue;
            }
            if (m_activeParts[i].transform.localScale.y < m_smallPartScale)
            {
                m_partsToPrep++;
            }
            if (m_activeParts[i].transform.localScale.x <= 0f)
            {   
                RemoveParticle(i);
            }
            else if ((m_activeParts[i].transform.position - this.m_playerCamera.transform.position).magnitude > m_cullDistance)
            {
                RemoveParticle(i);
                // also check for occulsion here tbh
            }            
        }
        // Debug.Log("parts to prep = " + m_partsToPrep);

        if (m_drawDebug)
        {
            foreach (GameObject part in m_upperLayerParticles)
            {
                Debug.DrawLine(part.transform.position, m_playerCamera.transform.position);
            }
            foreach (GameObject part in m_activeParts)
            {
                Debug.DrawLine(part.transform.position, m_playerCamera.transform.position);
            }
        }
    }

    virtual public void TransposeParticles(Vector3 offset)
    {
        foreach (GameObject part in m_upperLayerParticles)
        {
            if (part != null)
                part.transform.position += offset;
        }
        foreach (GameObject part in m_activeParts)
        {
            if (part != null)
                part.transform.position += offset;
        }

    }

    protected void RefreshParticles()
    {
        for (int i = m_upperLayerParticles.Count - 1; i > 0; --i)
        {
            if (m_upperLayerParticles[i].transform.position.y < m_playerCamera.transform.position.y + m_cullDistance)
            {
                m_activeParts.Add(m_upperLayerParticles[i]);
                m_upperLayerParticles.RemoveAt(i);                
            }
        }
        if (m_activeParts.Count < m_particleCap)
        {
            AddRandomParticle(m_minRadius, m_spawnRadius, m_cullDistance * .5f);
        }
        //if ((m_upperLayerParticles.Count < m_particleCap) && (m_upperLayerParticles.Count < m_partsToPrep))
        //{
        //    AddRandomParticle(m_minRadius, m_spawnRadius, m_cullDistance * .5f);
        //}
    }

    protected void RemoveParticle(int index)
    {
        GameObject.Destroy(m_activeParts[index]);
        m_activeParts.RemoveAt(index);
        //Debug.Log("Parts in list: " + m_activeParts.Count);          
        
    }

    virtual public void ScaleParticles(Vector3 deltaScale, GameObject scalingParent)
    {
        foreach (GameObject part in m_activeParts)
        {
            if (part != null)
            {
                if (part.transform.localScale.y > 0)
                {
                    //part.transform.localScale += deltaScale * m_scaleModifier;
                    part.transform.parent = scalingParent.transform;
                }
            }
        }
    }

    virtual public void ReparentParticles()
    {
        foreach (GameObject part in m_activeParts)
        {
            if (part != null)
                part.transform.parent = this.gameObject.transform;
        }
    }
}
