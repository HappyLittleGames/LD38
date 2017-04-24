using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    float swimSpeed = 0.2f;
    [SerializeField] public Climb m_climber { get; private set; }
    [SerializeField] private float m_maxSpeed = 25f;
    [SerializeField] private float m_speedModifier = 1.0f;
    [SerializeField] public WorldColor m_worldColor { get; private set; }
    [SerializeField] private bool m_debugControls = false;
    [SerializeField] private TextMesh m_scoreText = null;
    [SerializeField] private AudioSource m_audioSource = null;
    [SerializeField] private AudioSource m_leftHandAudio = null;
    [SerializeField] private AudioSource m_rightHandAudio = null;
    private double m_score;


    private Vector3 previousLeftPos;
    private Vector3 previousRightPos;
    private List<GameObject> hands = new List<GameObject>();

    void Start()
    {
        //Debug.Log("LTouch connected " + (OVRInput.GetConnectedControllers() == OVRInput.Controller.LTouch));
        m_scoreText = GameObject.FindGameObjectWithTag("Finish").GetComponent<TextMesh>();

        m_climber = GetComponent<Climb>();
        m_worldColor = GetComponent<WorldColor>();
        hands.Clear();
        foreach (GameObject hand in GameObject.FindGameObjectsWithTag("Hand"))
        {
            hands.Add(hand);
        }
        Debug.Log("Has " + hands.Count + " hands");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        DigInput(Time.deltaTime);

        FindHandOverlaps();

        m_scoreText.text = "Score: " + Mathf.Abs((int)m_score / 100 );
    }

    void UpdateSounds(float leftHandVolume, float rightHandVolume)
    {
        m_leftHandAudio.volume = Mathf.Clamp(leftHandVolume,0 ,1);
        m_leftHandAudio.volume = Mathf.Clamp(rightHandVolume, 0, 1);
    }

    void DigInput(float deltaTime)
    {
        //OVRInput.Update();
        Vector3 leftAccel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);// - previousLeftPos;
        Vector3 rightAccel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);// - previousRightPos;

        // Debug.Log(leftAccel + " is tracked = " + OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch));
        // Debug.Log(rightAccel + " is tracked = " + OVRInput.GetControllerPositionTracked(OVRInput.Controller.RTouch));

        //leftAccel  = new Vector3(leftAccel.x * leftAccel.x, leftAccel.y * leftAccel.y, leftAccel.z * leftAccel.z) ;
        //rightAccel = new Vector3(rightAccel.x * rightAccel.x, rightAccel.y * rightAccel.y, rightAccel.z * rightAccel.z);
        Debug.Log(leftAccel.magnitude / 8 + " and " + rightAccel.magnitude / 8);
        UpdateSounds(leftAccel.magnitude / 8, rightAccel.magnitude / 8);

        leftAccel *= leftAccel.sqrMagnitude * swimSpeed;
        rightAccel *= rightAccel.sqrMagnitude * swimSpeed;

        Vector3 translation = Vector3.ClampMagnitude(leftAccel + rightAccel, m_maxSpeed) * m_speedModifier;
        m_score += translation.y;
        m_climber.Transpose(translation);
        //previousLeftPos  = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        //previousRightPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
    }

    void FindHandOverlaps()
    {
        foreach (GameObject hand in hands)
        {
            float absorbed = 0.0f;
            SphereCollider handCollider = hand.GetComponent<SphereCollider>();
            Collider[] collided = Physics.OverlapBox(handCollider.transform.position, handCollider.bounds.extents * 0.5f);
            // Debug.Log(collided.Length);
            foreach (Collider col in collided)
            {
                if (col.gameObject.tag == "Respawn")
                {
                    absorbed += col.GetComponent<Particle>().DrainMass(100 * Time.deltaTime) + m_speedModifier * 0.001f;
                    Destroy(col.gameObject); // who needs feedback anywhooo??
                    m_audioSource.pitch = Random.Range(0.8f, 1.2f);
                    m_audioSource.Play();
                    //m_climber.ScaleWorld(Vector3.one * -100 * Time.deltaTime);
                }
            }
            m_speedModifier += absorbed;
        }
    }

    public double GetScore()
    {
        return Mathf.Abs((int)m_score / 100);
    }
}
