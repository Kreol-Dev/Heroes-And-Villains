using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;

namespace CoreMod
{

	public abstract class TiledObjectsCollectionInteractor<TCollectionObject, TCollection> : BaseMapCollectionInteractor<TCollection>, 
	IObjectsInteractor<TCollectionObject> where TCollectionObject: class where TCollection : TiledObjectsMapCollection<TCollectionObject>
	{
		public event ObjectDelegate<TCollectionObject> ObjectSelected;

		public event ObjectDelegate<TCollectionObject> ObjectDeSelected;

		public event ObjectDelegate<TCollectionObject> ObjectHovered;

		public event ObjectDelegate<TCollectionObject> ObjectDeHovered;

		


		TCollectionObject hoveredObject;
		TCollectionObject selectedObject;


		TilesRoot tilesRoot;


		public override bool OnHover (Transform obj, Vector3 point)
		{
			TileHandle handle = tilesRoot.MapHandle.GetHandle (point);
			if (handle == null)
			{
				if (hoveredObject != null)
				{
					ObjectDeHovered (hoveredObject);
					hoveredObject = null;
				}
				return false;
			}

			TCollectionObject tileContent = Collection.GetObjectID (handle);
			if (tileContent == null)
			{
				if (hoveredObject != null)
				{
					ObjectDeHovered (hoveredObject);
					hoveredObject = null;
				}
				return false;
			}
			if (hoveredObject == tileContent)
			{
				return true;
			} else
			{
				if (hoveredObject != null)
					ObjectDeHovered (hoveredObject);
				hoveredObject = tileContent;
				ObjectHovered (hoveredObject);
				return true;
			}
		}


		public override bool OnClick (Transform obj, Vector3 point)
		{
			TileHandle handle = tilesRoot.MapHandle.GetHandle (point);
			if (handle == null)
			{
				if (selectedObject != null)
				{
					ObjectDeSelected (selectedObject);
					selectedObject = null;
				}
				return false;
			}


			TCollectionObject tileContent = Collection.GetObjectID (handle);
			if (tileContent == null)
			{
				if (selectedObject != null)
				{
					ObjectDeSelected (selectedObject);
					selectedObject = null;
				}
				return false;
			} else
			{
				if (selectedObject == tileContent)
				{
					ObjectDeSelected (selectedObject);
					selectedObject = null;			
					return false;
				}
				if (selectedObject != null)
					ObjectDeSelected (selectedObject);
				selectedObject = tileContent;
				ObjectSelected (selectedObject);
				return true;
			}

		}




		protected override void Setup (ITable definesTable)
		{

			tilesRoot = Find.Root<TilesRoot> ();
			GameObject go = GameObject.Find ("MapCollider");
			CollectionHandle handle = go.AddComponent<CollectionHandle> ();
			//	handle.Layer = Find.Root<MapRoot.Map> ().GetLayer (Layer.Name);
			Transform transform = go.transform;
			transform.position = new Vector3 (tilesRoot.MapHandle.SizeX / 2, tilesRoot.MapHandle.SizeY / 2, 0);
			transform.localScale = new Vector3 (tilesRoot.MapHandle.SizeX, tilesRoot.MapHandle.SizeY, 0);

		}
	}
}