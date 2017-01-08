using UnityEngine;
using System.Collections;

public class ItemTypeShieldBehaviour : ItemBase {

	public Rigidbody m_body;
	public int m_ItemDuration = 30;
	private float m_ItemUseTime = 0;

	public override string ItemName
	{
		get {
			return "Shield";
		}
	}

	void Update () {
		if (m_ItemUseTime > 0) {
			if (Time.time - m_ItemUseTime < 10) {
				m_body.position = OriginTransform.position;
			} else {
				Destroy (gameObject);
			}
		}
	}

	void Use () {
		m_ItemUseTime = Time.time;

		Debug.Log("Item used: " + OriginTransform.ToString());
	}
}
