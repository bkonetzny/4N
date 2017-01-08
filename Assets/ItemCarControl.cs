using UnityEngine;
using System.Collections;

public class ItemCarControl : MonoBehaviour {

	public UnityEngine.UI.Text m_UIText;
	public string m_ItemType;
	public GameObject m_Item;
	public ItemBase m_ItemBase;

	// Use this for initialization
	void Start () {
	}

	public void SetUseableItem (string ItemClass) {
		if (!HasItem()) {
			Debug.Log ("SetUseableItem: " + ItemClass);

			// Load an inactive instance of the item.
			m_Item = Instantiate (Resources.Load (ItemClass), gameObject.transform.position, gameObject.transform.rotation) as GameObject;

			Debug.Log ("SetUseableItem m_Item: " + m_Item.ToString ());

			m_Item.SetActive (false);
			m_ItemBase = m_Item.GetComponent (typeof(ItemBase)) as ItemBase;

			Debug.Log ("SetUseableItem m_ItemBase: " + m_ItemBase.ToString ());

			// Display item name in UI.
			m_UIText.text = "Current Item: " + m_ItemBase.ItemName;
		}
	}

	private void Update ()
	{
		if (Input.GetButtonDown ("Jump"))
		{
			UseItem();
		}
	}

	private void UseItem ()
	{
		if (HasItem())
		{
			m_UIText.text = "Use Item: " + m_ItemBase.ItemName;

			Debug.Log ("Transform: X = " + transform.position.x + " | Y = " + transform.position.y);

			// Activate the item so it renders and trigger items 'Use' method.
			m_ItemBase.OriginTransform = transform;
			m_Item.SetActive (true);
			m_Item.SendMessage ("Use");

			m_UIText.text = "Item " + m_ItemBase.ItemName + " used!";

			m_Item = null;
			m_ItemBase = null;
		}
	}

	private bool HasItem ()
	{
		return m_Item != null;
	}
}
