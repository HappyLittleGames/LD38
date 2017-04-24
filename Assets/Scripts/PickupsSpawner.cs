using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsSpawner : SemiClutter
{
    override protected void StartupPopulate()
    {   
        for (int i = 0; i < m_startupAmount; ++i)
        {
            AddRandomParticle(m_minRadius, m_spawnRadius, Random.Range(m_playerCamera.transform.position.y, m_heightVariance));
        }
    }

    override protected void AddParticle(Vector3 position)
    {
        m_upperLayerParticles.Add(Instantiate(m_sampleClutter, position, Random.rotation));
        m_upperLayerParticles[m_upperLayerParticles.Count - 1].transform.localScale = Vector3.one * m_startScaleModifier;
        m_upperLayerParticles[m_upperLayerParticles.Count - 1].transform.parent = this.transform;
        // m_upperLayerParticles[m_upperLayerParticles.Count - 1].GetComponent<Particle>().
    }
}
