using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
	public enum GripType
	{
		Grass,
		Mud,
		Ice
	}

  	Rigidbody m_body;
  	float m_deadZone = 0.1f;
	[SerializeField] GripType defaultGripType;
  	public float m_hoverForce = 500f;
	public float m_gravityForce = 1000f;
  	public float m_hoverHeight = 2.0f;
  	public GameObject[] m_hoverPoints;

  	float m_forwardAcl = 100.0f;
  	float m_backwardAcl = 25.0f;
  	float m_currThrust = 0.0f;

 	public float m_turnStrength = 10f;
  	float m_currTurn = 0.0f;

  	public GameObject m_leftAirBrake;
  	public GameObject m_rightAirBrake;
	public ParticleSystem[] dustTrails = new ParticleSystem[4];
	float m_defaultDrag;

 	int m_layerMask;

	GripType m_gripType = GripType.Grass;
	public GripType gripType {
		get
		{
			return m_gripType;
		}
		set
		{
			switch(value)
			{
			case GripType.Grass:
				m_body.drag = 3f;
				m_forwardAcl = 10000;
				m_backwardAcl = 2500;
				break;
			case GripType.Mud:
				m_body.drag = 1f;
				m_forwardAcl = 5000;
				m_backwardAcl = 1250;
				break;
			case GripType.Ice:
				m_body.drag = 0.5f;
				m_forwardAcl = 2500;
				m_backwardAcl = 500;
				break;
			}
			m_defaultDrag = m_body.drag;
		}
	}

  void Start()
  {
	    m_body = GetComponent<Rigidbody>();
		m_body.centerOfMass = Vector3.down;

	    m_layerMask = 1 << LayerMask.NameToLayer("Characters");
	    m_layerMask = ~m_layerMask;

		gripType = defaultGripType;
  }

  void OnDrawGizmos()
  {

    //  Hover Force
    RaycastHit hit;
    for (int i = 0; i < m_hoverPoints.Length; i++)
    {
      var hoverPoint = m_hoverPoints [i];
      if (Physics.Raycast(hoverPoint.transform.position, 
                          -Vector3.up, out hit,
                          m_hoverHeight, 
                          m_layerMask))
      {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
        Gizmos.DrawSphere(hit.point, 0.5f);
      } else
      {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hoverPoint.transform.position, 
                       hoverPoint.transform.position - Vector3.up * m_hoverHeight);
      }
    }
  }
	
  void Update()
  {

    // Main Thrust
    m_currThrust = 0.0f;
    float aclAxis = Input.GetAxis("Vertical");
    if (aclAxis > m_deadZone)
      m_currThrust = aclAxis * m_forwardAcl;
    else if (aclAxis < -m_deadZone)
      m_currThrust = aclAxis * m_backwardAcl;

    // Turning
    m_currTurn = 0.0f;
    float turnAxis = Input.GetAxis("Horizontal");
    if (Mathf.Abs(turnAxis) > m_deadZone)
      m_currTurn = turnAxis;
  }

  void FixedUpdate()
  {

    //  Hover Force
	RaycastHit hit;
	bool  grounded = false;
	for (int i = 0; i < m_hoverPoints.Length; i++)
	{
		if(dustTrails[i] != null)
		{
			var emission =dustTrails[i].emission;
			emission.enabled =  false;
		}
  		var hoverPoint = m_hoverPoints [i];
  		if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit,m_hoverHeight, m_layerMask))
		{
			m_body.AddForceAtPosition(Vector3.up * m_hoverForce* (1.0f - (hit.distance / m_hoverHeight)), hoverPoint.transform.position);
			grounded = true;
			if(dustTrails[i] != null)
			{
				var emission =dustTrails[i].emission;
				emission.enabled =  m_body.velocity.sqrMagnitude>0.2f;
			}
		}
  		else
  		{
        if (transform.position.y > hoverPoint.transform.position.y)
			{
				m_body.AddForceAtPosition(hoverPoint.transform.up * m_gravityForce, hoverPoint.transform.position);
			}
        else
          	m_body.AddForceAtPosition(
			hoverPoint.transform.up * -m_gravityForce,
            hoverPoint.transform.position);
  		}
	}

	if(grounded)
	{
		m_body.drag = m_defaultDrag;
	}
	else
	{
		m_body.drag = 0f;
		m_currThrust /= 5f;
	}

    // Forward
	if (Mathf.Abs (m_currThrust) > 0) {
		m_body.AddForce (transform.forward * m_currThrust);
	}

    // Turn
    if (m_currTurn > 0)
    {
      m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
    } else if (m_currTurn < 0)
    {
      m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
    }
  }
}
