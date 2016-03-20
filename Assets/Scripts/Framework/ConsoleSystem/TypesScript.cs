using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypesScript : MonoBehaviour
{
	public GameObject parent;
	[SerializeField]
	GameObject panel;
	[SerializeField]
	MessagesScript mes;
	int FilterSize = 0;
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void AddType (InternalMessage.MessageType type)
	{
		GameObject f = GameObject.Instantiate (parent);
		f.transform.SetParent (panel.transform, false);
		f.gameObject.name = "Type" + FilterSize.ToString ();
		f.GetComponent<FilterScript> ().cat = null;
		f.GetComponent<FilterScript> ().type = type;
		f.GetComponent<FilterScript> ().Set (type.ToString (), true);
		f.GetComponent<FilterScript> ().messages = mes;
		f.GetComponent<Image> ().color = mes.GetHash (type.ToString ());
		//f.GetComponentInChildren<Toggle>().onValueChanged += mes.ShowMessagePool();
		FilterSize++;
	}

}
