using UnityEngine;
using System.Collections;

public class ItemTrigger : MonoBehaviour {

	public int m_TriggerTimeout = 30;
	public float m_TriggerLastUseTime = 0;
	public bool m_TriggerIsActive = true;
	private Renderer rend;
	static string[] PossibleItems = new string[] {"ItemTypeShell", "ItemTypeShield"};

	void Start () {
		// rend = GetComponent(typeof(Renderer)) as Renderer;
		rend = transform.Find ("Plane").gameObject.GetComponent(typeof(Renderer)) as Renderer;
	}

	private void Update () {
		if (!m_TriggerIsActive && Time.time - m_TriggerLastUseTime > 10) {
			m_TriggerIsActive = true;

			Color matColor = rend.material.color;
			matColor.a = 1.0f;
			rend.material.SetColor ("_Color", matColor);

			Debug.Log ("ItemTrigger enabled");
		}
	}

	private void OnTriggerEnter (Collider other) {
		if (m_TriggerIsActive) {
			m_TriggerIsActive = false;
			m_TriggerLastUseTime = Time.time;

			string SelectedItem = PossibleItems [Random.Range (0, PossibleItems.Length)];

			other.gameObject.SendMessageUpwards ("SetUseableItem", SelectedItem);

			Color matColor = rend.material.color;
			matColor.a = 0.1f;
			rend.material.SetColor ("_Color", matColor);

			Debug.Log ("ItemTrigger disabled");
		}
	}
}
