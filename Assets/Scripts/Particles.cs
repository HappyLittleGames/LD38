using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

    public Vector3 rotation { get; set; }
    public Vector3 velocity { get; set; }

    private List<GameObject> particles = new List<GameObject>();
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateParticles(Time.deltaTime);
	}

    private void UpdateParticles(float deltaTime)
    {
        foreach (GameObject part in particles)
        {
            part.transform.rotation *= Quaternion.Euler(rotation * Time.deltaTime);
            part.transform.position += velocity * Time.deltaTime;
        }
        
    }

    private void SpawnParticles()
    {
        
    }
}
