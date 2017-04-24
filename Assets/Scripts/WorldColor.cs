using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EColorZones
{
    CZBlack,
    CZBrown,
    CZGreen,
    CZBlue,
    CZYellow
};

public class WorldColor : MonoBehaviour
{
    [SerializeField] private Camera m_camera = null;
    [SerializeField] private Material m_cloudMaterial = null;
    [SerializeField] private Material m_handMaterial = null;
    [SerializeField] private Material m_goalMaterial = null;
    private bool m_autoShifting = true;

    [SerializeField] private List<Transform> m_realmBounds = new List<Transform>();
    [SerializeField] private List<Color> m_colorSequence = new List<Color>();

    private int m_realmIndex = 0;
    public Color worldColor { get; set; }
    [SerializeField] private ParticleSystem m_dustParticles = null;

    float realmCeil;
    float realmFloor;

    [SerializeField] private bool m_debug = false;

    private void Start()
    {
        if (m_realmBounds.Count != m_colorSequence.Count)
            Debug.Log("WARNING - realmBounds and colorSequence are different sizes");

        realmCeil = m_realmBounds[m_realmIndex].transform.position.y;
        realmFloor = m_realmBounds[NextRealmIndex()].transform.position.y;

    }

    void Update()
    {
        if (m_autoShifting)
            AutoShift();
	}

    public List<Transform> GetRealms() { return m_realmBounds;  }

    void AutoShift()
    {
        realmCeil = m_realmBounds[m_realmIndex].transform.position.y;
        realmFloor = m_realmBounds[NextRealmIndex()].transform.position.y;

        //realmCeil = realmFloor + realmCeil;
        float alpha = (Mathf.InverseLerp(realmCeil, realmFloor, m_camera.transform.position.y));

        if (m_debug)
        {
            Debug.Log("Alpha of realm shift: " + alpha + ", Floor: " + realmFloor + ", Ceil: " + realmCeil
                + ", current position: " + gameObject.transform.position.y +
                " Queries for indices: " + m_realmIndex + " and " + NextRealmIndex() + ".");

            Debug.DrawLine(m_camera.transform.position, new Vector3(0, realmCeil, 0));
            Debug.DrawLine(m_camera.transform.position, new Vector3(0, realmFloor, 0));
        }

        worldColor = Color.Lerp(m_colorSequence[m_realmIndex], m_colorSequence[NextRealmIndex()], alpha);
        if (alpha >= 1)
        {
            Debug.Log("Shift");
            gameObject.transform.position = new Vector3(0, realmFloor, 0);
            m_realmIndex++;
            if (m_realmIndex == m_colorSequence.Count)
            {
                m_realmIndex = 0;
            }
            m_goalMaterial.SetColor(Shader.PropertyToID("_Color"), m_colorSequence[NextRealmIndex()]);
        }
        

        m_camera.backgroundColor = worldColor;
        ParticleSystem.MainModule settings = m_dustParticles.main;

        settings.startColor = worldColor;
        m_cloudMaterial.SetColor(Shader.PropertyToID("_Color"), worldColor + new Color(.04f, .04f, .04f, 1));
        m_handMaterial.SetColor(Shader.PropertyToID("_Color"), new Color(1,1,1) - worldColor + new Color(0,0,0,1));

        UpdateFogColor(worldColor);
    }

    private void UpdateFogColor(Color fogColor)
    {
        RenderSettings.fogColor = fogColor;
    }

    int PreviousRealmIndex()
    {
        if (m_realmIndex == 0)
            return m_colorSequence.Count - 1;
        else
            return m_realmIndex--;
    }

    int NextRealmIndex()
    {
        if (m_realmIndex == m_colorSequence.Count -1)
            return 0;
        return m_realmIndex + 1;
    }
}
