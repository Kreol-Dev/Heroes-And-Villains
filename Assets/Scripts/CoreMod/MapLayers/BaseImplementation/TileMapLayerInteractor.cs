using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System.Text;

namespace CoreMod
{
	public abstract class TileMapLayerInteractor<TObject, TLayerObject, TLayer> : BaseMapLayerInteractor<TLayer>, 
    ITileMapInteractor<TObject, TLayerObject, TLayer> where TLayer : class, IMapLayer, ITileMapLayer<TLayerObject>
	{
		public event TileDelegate<TObject> TileSelected;

		public event TileDelegate<TObject> TileDeselected;

		public event TileDelegate<TObject> TileHovered;

		public event TileDelegate<TObject> TileDeHovered;


		TileHandle hoveredTile;
		TObject hoveredObject;
		TObject selectedObject;
		TileHandle lastSelectedTile;


		public abstract bool ObjectFromLayerObject (TLayerObject obj, out TObject outObject);

		TilesRoot tilesRoot;


		public override bool OnHover (Transform obj, Vector3 point)
		{
			TileHandle handle = tilesRoot.MapHandle.GetHandle (point);
			if (handle != hoveredTile)
			{
				TObject tileContent;

				if (hoveredTile != null && hoveredObject != null)
					TileDeHovered (hoveredTile, hoveredObject);

				hoveredTile = handle;
				if (!ObjectFromLayerObject (handle.Get (Layer.Tiles), out tileContent))
				{
					hoveredObject = default(TObject);
					return false;
				}
					
				TileHovered (handle, tileContent);
				hoveredObject = tileContent;
			}

			return true;
		}


		public override bool OnClick (Transform obj, Vector3 point)
		{
			TileHandle handle = tilesRoot.MapHandle.GetHandle (point);
			TObject tileContent;

			if (lastSelectedTile != null)
				TileDeselected (lastSelectedTile, selectedObject);

			if (!ObjectFromLayerObject (handle.Get (Layer.Tiles), out tileContent))
				return false;
			TileSelected (handle, tileContent);
			lastSelectedTile = handle;
			selectedObject = tileContent;
			return true;
		}

		public override void OnAltClick (Transform obj, Vector3 point)
		{
			TileDeselected (lastSelectedTile, selectedObject);
			lastSelectedTile = null;

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
			transform.position = new Vector3 (Layer.Tiles.GetLength (0) / 2, Layer.Tiles.GetLength (1) / 2, 0);
			transform.localScale = new Vector3 (Layer.Tiles.GetLength (0), Layer.Tiles.GetLength (1), 0);

		}

	}
}

