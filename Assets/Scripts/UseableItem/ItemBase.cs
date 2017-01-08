using UnityEngine;
using System.Collections;

abstract public class ItemBase : MonoBehaviour {

	private Transform originTransform;

	abstract public string ItemName {get;}

	public Transform OriginTransform {
		get {
			return originTransform;
		}
		set {
			originTransform = value;

			Debug.Log ("OriginTransform: X = " + originTransform.position.x + " | Y = " + originTransform.position.y);
		}
	}
}
