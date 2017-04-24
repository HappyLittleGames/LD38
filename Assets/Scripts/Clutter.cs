using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GrowUp))]
public class Clutter : MonoBehaviour
{
    [SerializeField] List<GameObject> m_clutterTypes;
    private List<Particle> m_activeClutter = new List<Particle>();
    [SerializeField] int m_startupAmount = 0;
    [SerializeField] float m_spawnRadius = 1;
    [SerializeField] float m_minRadius = 1;
    [SerializeField] float m_heightVariance = 0;
    [SerializeField] Vector3 m_velocity = Vector3.zero;
    [SerializeField] Vector3 m_rotation = Vector3.zero;
    private GrowUp m_growup;
	// Use this for initialization
	void Start ()
    {
        m_growup = GetComponent<GrowUp>();
        
        for (int i = 0; i < m_startupAmount; ++i)
        {
            SpawnClutter(m_clutterTypes[0], transform.position.y, m_minRadius, m_spawnRadius, m_heightVariance);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        // run some distance based pruning and spawning here obvvvv

        UpdateClutter(Time.deltaTime);
	}


    private void SpawnClutter(GameObject type, float yPosition, float minRange, float radius, float yRange = 0, bool randomRotation = false)
    {
        float sqrtRadius = Mathf.Sqrt(Random.Range(minRange, radius));
        float theta = Random.Range(0.0f, Mathf.PI * 2);
        Vector2 randomOnPlane = new Vector2(sqrtRadius * Mathf.Cos(theta), sqrtRadius * Mathf.Sin(theta)) * sqrtRadius;
        Vector3 randomLocation = new Vector3(randomOnPlane.x, yPosition + Random.Range(-yRange, yRange), randomOnPlane.y);

        m_activeClutter.Add(Instantiate(type, randomLocation, Quaternion.identity, this.transform).GetComponent<Particle>());
    }

    public void CleanupClutter()
    {
        foreach (Particle clutter in m_activeClutter)
        {
            Destroy(clutter.gameObject);
        }
        m_activeClutter.Clear();
    }

    private void UpdateClutter(float deltaTime)
    {
        foreach (Particle clutter in m_activeClutter)
        {
            clutter.gameObject.transform.rotation *= Quaternion.Euler(m_rotation * Time.deltaTime);
            clutter.gameObject.transform.position += m_velocity * Time.deltaTime;
        }

    }
}
