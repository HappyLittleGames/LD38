using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWholeTutorial : MonoBehaviour {

    [SerializeField] TextMesh m_textMesh = null;
    [SerializeField] Player m_playuer = null;
    private string m_tutorial = "try swimming up";

	void Update ()
    {
        if ((Time.realtimeSinceStartup > 20) && (m_playuer.GetScore() < 10))
            m_textMesh.text = m_tutorial;
        else
            m_textMesh.text = "";
    }
}
