using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{

	public abstract class TiledObjectsLayerInteractor<TLayerObject, TLayer> : BaseMapLayerInteractor<TLayer>, 
	IObjectsInteractor<TLayerObject> where TLayer : /*class, IMapLayer,*/ ITileMapLayer<TLayerObject> where TLayerObject: class
	{
		HashSet<TLayerObject> selectedObjects = new HashSet<TLayerObject> ();

		public event ObjectDelegate<TLayerObject> ObjectSelected;

		public event ObjectDelegate<TLayerObject> ObjectDeSelected;

		TLayerObject hoveredObject = null;

		public event ObjectDelegate<TLayerObject> ObjectHovered;

		public event ObjectDelegate<TLayerObject> ObjectDeHovered;



		public override void OnHover (Vector2 point, HashSet<Transform> encounters, ref HashSet<object> hoveredObjects)
		{
			TileHandle handle = Layer.MapHandle.GetHandle (point);
			if (handle != null)
			{
				TLayerObject obj = handle.Get (Layer.Tiles);
				if (hoveredObject != obj)
				{
					if (hoveredObject != null)
						ObjectDeHovered (hoveredObject);
					hoveredObject = obj;
					ObjectHovered (hoveredObject);
				}
				if (obj != null)
					hoveredObjects.Add (obj);
			} else
			{
				if (hoveredObject != null)
				{
					ObjectDeHovered (hoveredObject);
					hoveredObject = null;
				}
			}
		}

		public override object OnSelect (Vector2 point, HashSet<object> selectables)
		{
			TileHandle handle = Layer.MapHandle.GetHandle (point);
			if (handle == null)
				return null;
			TLayerObject obj = handle.Get (Layer.Tiles);
			if (obj == null)
				return null;
			if (!selectables.Contains (obj))
				return null;
			else if (selectedObjects.Add (obj))
			{
				ObjectSelected (obj);
				return obj;
			} 		
			return null;
		}

		public override object OnDeSelect (Vector2 point)
		{
			TileHandle handle = Layer.MapHandle.GetHandle (point);
			if (handle == null)
				return null;
			TLayerObject obj = handle.Get (Layer.Tiles);
			if (obj == null)
				return null;
			if (selectedObjects.Remove (obj))
			{
				ObjectDeSelected (obj);
				return obj;
			}
			return null;
		}

		HashSet<TLayerObject> cachedSet = new HashSet<TLayerObject> ();

		public override IEnumerable<object> OnDeselectAll ()
		{
			cachedSet.Clear ();
			HashSet<TLayerObject> tempSet = selectedObjects;
			selectedObjects = cachedSet;
			cachedSet = tempSet;
			foreach (var tile in cachedSet)
				ObjectDeSelected (tile);
			return cachedSet as IEnumerable<object>;
		}

		public override IEnumerable<object> OnMassSelect (Vector2 minCorner, Vector2 maxCorner)
		{
			return null;
		}


		protected override void Setup (ITable definesTable)
		{
//			ObjectSelected += (obj) => Debug.LogFormat ("OBJECT SElECTED: {0}", obj);
//
//			ObjectDeSelected += (obj) => Debug.LogFormat ("OBJECT DESELECTED: {0}", obj);
//
//			ObjectHovered += (obj) => Debug.LogFormat ("OBJECT HOVERED: {0}", obj);
//
//			ObjectDeHovered += (obj) => Debug.LogFormat ("OBJECT DEHOVERED: {0}", obj);
//			ObjectSelected += (obj) => {};
//
//			ObjectDeSelected += (obj) => {};
//
//			ObjectHovered += (obj) => {};
//
//			ObjectDeHovered += (obj) => {};
			GameObject go = GameObject.Find ("MapCollider");
			InteractorRealm handle = go.AddComponent<InteractorRealm> ();
			handle.Interactor = this;
			Transform transform = go.transform;
			transform.position = new Vector3 (Layer.MapHandle.SizeX / 2, Layer.MapHandle.SizeY / 2, 0);
			transform.localScale = new Vector3 (Layer.MapHandle.SizeX, Layer.MapHandle.SizeY, 0);

		}
	}
}