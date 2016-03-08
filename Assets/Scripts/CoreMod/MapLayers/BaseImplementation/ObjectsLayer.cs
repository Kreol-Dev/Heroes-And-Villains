using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;
using UIO;
using UnityEngine.UI;

namespace CoreMod
{
	public class ObjectsLayer : MapLayer, IListMapLayer<GameObject>
	{
		public event ObjectDelegate<GameObject> ObjectAdded;
		public event ObjectDelegate<GameObject> ObjectRemoved;

		HashSet<GameObject> gos = new HashSet<GameObject> ();

		public bool AddObject (GameObject go)
		{
			if (gos.Add (go))
			{
				if (ObjectAdded != null)
					ObjectAdded (go);
				return true;
			}
			return false;
			
		}

		public bool RemoveObject (GameObject go)
		{
			if (gos.Remove (go))
			{
				if (ObjectRemoved != null)
					ObjectRemoved (go);
				return true;
			}
			return false;
		}

		public bool HasObject (GameObject go)
		{
			return gos.Contains (go);
		}

		protected override void Setup (ITable definesTable, MapRoot.Map mapRoot)
		{
		}

	}

	public class ObjectsPresenter : ObjectLayerPresenter<GameObject, GameObject, object, IObjectsInteractor<GameObject>>
	{
		public override GameObject ObjectFromLayer (GameObject obj)
		{
			return obj;
		}


	}

	public class GOPresenter : ObjectPresenter<GameObject>
	{
		Text selectionText;
		Text hoverText;

		public override void Setup (ITable definesTable)
		{
			selectionText = (Object.Instantiate (Resources.Load ("UI/Text")) as GameObject).GetComponent<Text> ();
			var selectionLayout = selectionText.gameObject.AddComponent<LayoutElement> ();
			selectionLayout.minHeight = 20;
			selectionLayout.minWidth = 100;
			hoverText = (Object.Instantiate (Resources.Load ("UI/Text")) as GameObject).GetComponent<Text> ();
			var hoverLayout = hoverText.gameObject.AddComponent<LayoutElement> ();
			hoverLayout.minHeight = 20;
			hoverLayout.minWidth = 100;
			hoverText.text = "";
			hoverText.gameObject.SetActive (false);
			selectionText.text = "";
			selectionText.gameObject.SetActive (false);
			Transform selectionGO = GameObject.Find ("SelectionPanel").transform;
			Transform hoverGO = GameObject.Find ("HoverPanel").transform;
			selectionText.transform.SetParent (selectionGO);
			hoverText.transform.SetParent (hoverGO);
		}

		public override void ShowObjectDesc (GameObject obj)
		{
			selectionText.gameObject.SetActive (true);
			selectionText.text = obj.name;
		}

		public override void HideObjectDesc ()
		{
			selectionText.gameObject.SetActive (false);
		}

		public override void ShowObjectShortDesc (GameObject obj)
		{
			hoverText.gameObject.SetActive (true);
			hoverText.text = obj.name;
		}

		public override void HideObjectShortDesc ()
		{
			hoverText.gameObject.SetActive (false);
		}
	}

}
