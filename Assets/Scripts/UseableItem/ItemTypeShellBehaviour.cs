using UnityEngine;
using System.Collections;

public class ItemTypeShellBehaviour : ItemBase {

	public Rigidbody m_body;

	public override string ItemName
	{
		get {
			return "Shell";
		}
	}

	void Update () {
		Vector3 newPos = new Vector3 (m_body.position.x, 1.0f, m_body.position.z);

		m_body.position = newPos;
	}

	void Use () {
		Debug.Log("Item used: " + OriginTransform.ToString());

		Vector3 newPos = new Vector3 (OriginTransform.position.x, 1.0f, OriginTransform.position.z);

		m_body.position = newPos + OriginTransform.forward * 5;
		m_body.rotation = OriginTransform.rotation; // Quaternion.Euler(0, OriginTransform.rotation.y, 0);
		m_body.velocity = 100f * OriginTransform.forward;
	}

	void OnCollisionEnter (Collision col)
	{
		Destroy (gameObject);
	}
}
