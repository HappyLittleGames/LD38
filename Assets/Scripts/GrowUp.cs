using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowUp : MonoBehaviour {

    [SerializeField] private bool m_runByController = false;
    // ought to be finbonnaci I guess
    [SerializeField] private float growthRate = 1.0f;
	

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_runByController)
        {
            if (gameObject.transform.localScale.x > 0)
                gameObject.transform.localScale = gameObject.transform.localScale + gameObject.transform.localScale * growthRate * Time.deltaTime * OVRInput.Get(OVRInput.RawAxis1D.Any);
        }
        else if (gameObject.transform.localScale.x > 0)
            gameObject.transform.localScale = gameObject.transform.localScale + Vector3.one * growthRate * Time.deltaTime;

    }

    public void GainScale(Vector3 transposedAmount)
    {

    }
}
