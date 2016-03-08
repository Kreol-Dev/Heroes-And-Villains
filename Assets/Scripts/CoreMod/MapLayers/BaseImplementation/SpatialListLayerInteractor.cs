using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	public class SpatialListLayerInteractor : BaseMapLayerInteractor<IListMapLayer<GameObject>>, 
	IObjectsInteractor<GameObject>
	{
		HashSet<GameObject> selectedObjects = new HashSet<GameObject> ();

		public event ObjectDelegate<GameObject> ObjectSelected;

		public event ObjectDelegate<GameObject> ObjectDeSelected;

		HashSet<GameObject> hoveredObjects = new HashSet<GameObject> ();
		HashSet<GameObject> cachedHovered = new HashSet<GameObject> ();

		public event ObjectDelegate<GameObject> ObjectHovered;

		public event ObjectDelegate<GameObject> ObjectDeHovered;



		public override void OnHover (Vector2 point, HashSet<Transform> encounters, ref HashSet<object> hoveredObjects)
		{
			//Debug.Log (this.GetType ());
			cachedHovered.Clear ();
			foreach (var enc in encounters)
			{
				var all = enc.gameObject.GetComponent<InteractorAllegiance> ();
				if (all.Interactor == this)
				{
					hoveredObjects.Add (enc.gameObject);
					cachedHovered.Add (enc.gameObject);

					hoveredObjects.Add (enc.gameObject);
					if (!this.hoveredObjects.Contains (enc.gameObject))
					{
						ObjectHovered (enc.gameObject);

					}
				}
			}
			foreach (var obj in this.hoveredObjects)
				if (!cachedHovered.Contains (obj))
					ObjectDeHovered (obj);
			
			this.hoveredObjects.Clear ();
			var temp = cachedHovered;
			cachedHovered = this.hoveredObjects;
			this.hoveredObjects = temp;
		}

		public override object OnSelect (Vector2 point, HashSet<object> selectables)
		{
			foreach (var selectable in selectables)
			{
				GameObject go = selectable as GameObject;
				if (go == null)
					continue;
				var all = go.gameObject.GetComponent<InteractorAllegiance> ();
				if (all == null)
					continue;
				if (all.Interactor == this)
				{
					if (selectedObjects.Add (go))
						ObjectSelected (go);
					return go;
				}
			}
			//foreach (var selectedObject in selectedObjects)
			//	ObjectDeSelected (selectedObject);
			//selectedObjects.Clear ();
			return null;
		}

		public override object OnDeSelect (Vector2 point)
		{
			return null;
		}

		HashSet<GameObject> deselectAllCache = new HashSet<GameObject> ();

		public override IEnumerable<object> OnDeselectAll ()
		{
			deselectAllCache.Clear ();
			var tempSet = selectedObjects;
			selectedObjects = deselectAllCache;
			deselectAllCache = tempSet;
			foreach (var tile in deselectAllCache)
				ObjectDeSelected (tile);
			return deselectAllCache as IEnumerable<object>;
		}

		public override IEnumerable<object> OnMassSelect (Vector2 minCorner, Vector2 maxCorner)
		{
			return null;
		}

		void OnObjectAdded (GameObject go)
		{
			InteractorAllegiance all = go.GetComponent<InteractorAllegiance> ();
			if (all == null)
				all = go.AddComponent<InteractorAllegiance> ();
			all.Interactor = this;
		}

		void OnObjectRemoved (GameObject go)
		{
			InteractorAllegiance all = go.GetComponent<InteractorAllegiance> ();
			if (all != null)
				Object.Destroy (all);
		}

		protected override void Setup (ITable definesTable)
		{
			ObjectSelected += (obj) => Debug.LogFormat ("OBJECT SElECTED: {0}", obj);
			
			ObjectDeSelected += (obj) => Debug.LogFormat ("OBJECT DESELECTED: {0}", obj);
			
			ObjectHovered += (obj) => Debug.LogFormat ("OBJECT HOVERED: {0}", obj);
			
			ObjectDeHovered += (obj) => Debug.LogFormat ("OBJECT DEHOVERED: {0}", obj);
			Layer.ObjectAdded += OnObjectAdded;
			Layer.ObjectRemoved += OnObjectRemoved;
			GameObject go = GameObject.Find ("MapCollider");
			InteractorRealm handle = go.AddComponent<InteractorRealm> ();
			handle.Interactor = this;
		}
	}
}


