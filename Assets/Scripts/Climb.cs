using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour {

    // negative vertical motion propels the player, exactly that
    private GrowUp m_growUp;
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Vector3 m_velocity;
    private bool m_debugMode = true;
    [SerializeField] private float m_growthRate = 1.0f;
    [SerializeField] private ChildScaler m_childScaler = null;
    [SerializeField] private ParticleSystem m_dustPartSys = null;
    private ParticleSystem.Particle[] m_dustParts = null;
    private bool m_climbingCausesGrowing = true;
    [SerializeField] private bool m_debugControls = false;

    private List<GameObject> m_climbingHands = new List<GameObject>();
	

	void Update ()
    {
        if (m_debugControls)
        {
            if ((OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.0f) || (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.0f))
            {
                Transpose(m_velocity * -Time.deltaTime);

                //if (playerRoot.position.y > 6610)
                //    playerRoot.position = new Vector3(playerRoot.position.x, -6610, playerRoot.position.z);
            }
        }

        Vector3 scaleAmount = Vector3.one * OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) * m_growthRate
                              + Vector3.one * OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) * m_growthRate
                              * Time.deltaTime;

        m_childScaler.ScaleChildren(scaleAmount);

        //m_childScaler.ScaleChildren(Vector3.one * Time.deltaTime * OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) * m_growthRate);
        //m_childScaler.ScaleChildren(Vector3.one * Time.deltaTime * OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) * m_growthRate);
    }

    public void ScaleWorld(Vector3 deltaScale)
    {
        m_childScaler.ScaleChildren(deltaScale);
    }

    public void Transpose(Vector3 offset)
    {      
        m_childScaler.TransposeChildren(offset);
        if (m_dustPartSys != null)
        {
            m_dustParts = new ParticleSystem.Particle[m_dustPartSys.main.maxParticles];
            int partAmount = m_dustPartSys.GetParticles(m_dustParts);
            // Debug.Log(m_dustParts.Length);
            
            for (int i = 0; i < partAmount; i++)
            {
                m_dustParts[i].position += offset;
            }
        }
    }

    public void Grab(GameObject hand)
    {
        m_climbingHands.Add(hand);
        Debug.Log("grabbing");
    }

    public void Drop(GameObject hand)
    {
        m_climbingHands.Remove(hand);
        Debug.Log("dropping " + m_climbingHands.Count + " hands remaining");
    }
}
