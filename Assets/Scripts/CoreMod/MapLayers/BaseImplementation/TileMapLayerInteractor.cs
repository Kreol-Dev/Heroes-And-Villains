using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System.Text;

namespace CoreMod
{
	public abstract class TileMapLayerInteractor<TLayer> : BaseMapLayerInteractor<TLayer>, 
	ITileMapInteractor where TLayer : class, IMapLayer
	{
		public event TileDelegate TileSelected;

		public event TileDelegate TileDeselected;

		public event TileDelegate TileHovered;

		public event TileDelegate TileDeHovered;


		TileHandle hoveredTile;
		TileHandle selectedTile;



		TilesRoot tilesRoot;


		public override bool OnHover (Transform obj, Vector3 point)
		{
			TileHandle handle = tilesRoot.MapHandle.GetHandle (point);
			if (handle == null)
			{
				if (hoveredTile != null)
				{
					TileDeHovered (hoveredTile);
					hoveredTile = null;
				}
				return false;
			}

			if (handle != hoveredTile)
			{
				if (hoveredTile != null)
					TileDeHovered (hoveredTile);
				hoveredTile = handle;
				TileHovered (hoveredTile);
			}
			return true;
		}


		public override bool OnClick (Transform obj, Vector3 point)
		{
			TileHandle handle = tilesRoot.MapHandle.GetHandle (point);
			if (handle == null)
			{
				if (selectedTile != null)
				{
					TileDeselected (selectedTile);
					selectedTile = null;
				}
				return false;
			}
			if (handle == selectedTile)
			{
				TileDeselected (selectedTile);
				selectedTile = null;
				return false;
			} else
			{
				TileSelected (handle);
				selectedTile = handle;
			}

			return true;
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

