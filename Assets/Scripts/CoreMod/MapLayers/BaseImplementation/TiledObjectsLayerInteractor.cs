using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;

namespace CoreMod
{

	public abstract class TiledObjectsLayerInteractor<TObject, TLayerObject, TLayer> : BaseMapLayerInteractor<TLayer>, 
	IObjectsInteractor<TObject, TLayer> where TLayer : class, IMapLayer, ITileMapLayer<TLayerObject> where TObject: class
	{
		public event ObjectDelegate<TObject> ObjectSelected;

		public event ObjectDelegate<TObject> ObjectDeSelected;

		public event ObjectDelegate<TObject> ObjectHovered;

		public event ObjectDelegate<TObject> ObjectDeHovered;

		


		TObject hoveredObject;
		TObject selectedObject;


		public abstract bool ObjectFromLayerObject (TLayerObject obj, out TObject outObject);

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

			TObject tileContent;
			if (!ObjectFromLayerObject (handle.Get (Layer.Tiles), out tileContent))
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


			TObject tileContent;
			if (!ObjectFromLayerObject (handle.Get (Layer.Tiles), out tileContent))
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


		public override void OnUpdate ()
		{

		}


		protected override void Setup (ITable definesTable)
		{

			tilesRoot = Find.Root<TilesRoot> ();
			GameObject go = GameObject.Find ("MapCollider");
			LayerHandle handle = go.AddComponent<LayerHandle> ();
			handle.Layer = Find.Root<MapRoot.Map> ().GetLayer (Layer.Name);
			Transform transform = go.transform;
			transform.position = new Vector3 (tilesRoot.MapHandle.SizeX / 2, tilesRoot.MapHandle.SizeY / 2, 0);
			transform.localScale = new Vector3 (tilesRoot.MapHandle.SizeX, tilesRoot.MapHandle.SizeY, 0);

		}
	}
}