using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

    [SerializeField] private int m_pairingChance = 7;
    private Vector3 m_velocity = Vector3.zero;
    private Vector3 m_rotation = Vector3.zero;
    private float m_drainRate = 11.0f;
    private bool m_grantsPoints = false;
	// Use this for initialization
	void Start ()
    {
        if (Random.Range(0, m_pairingChance) > 1)
        {
            Vector3 newOffset = GetComponent<SphereCollider>().ClosestPointOnBounds(Random.onUnitSphere);
            GameObject particle = Instantiate(gameObject, newOffset, Quaternion.identity, this.transform);
            particle.transform.localScale = Vector3.one;
        }

        transform.rotation = Random.rotation;
	}
	
	public float DrainMass(float amount)
    {
        float drainedAmount = m_drainRate;
        if (gameObject.transform.localScale.y > 0)
            gameObject.transform.localScale += Vector3.one * m_drainRate * Time.deltaTime * amount;
        else
            drainedAmount *= 0;
        // Debug.Log("Getting touched!");

        return m_drainRate * Time.deltaTime;
    }

    public void UpdateParticle(float deltaTime)
    {
        gameObject.transform.rotation *= Quaternion.Euler(m_rotation * deltaTime);
        gameObject.transform.position += m_velocity * deltaTime;
    }
}
