using UnityEngine;
using System.Collections;

public class HudVelocity : MonoBehaviour {

	public Rigidbody m_Rigidbody;
	public UnityEngine.UI.Text m_UIText;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void FixedUpdate () {
		var velocity = (int) Mathf.Ceil (m_Rigidbody.velocity.magnitude);
		m_UIText.text = velocity.ToString();
	}
}
