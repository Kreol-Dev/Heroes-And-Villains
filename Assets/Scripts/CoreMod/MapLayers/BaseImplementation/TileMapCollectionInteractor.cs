using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System.Text;

namespace CoreMod
{
	public class TileMapCollectionInteractor : BaseMapCollectionInteractor<TileMapCollection>, ITileMapInteractor
	{
		public event TileDelegate TileSelected;

		public event TileDelegate TileDeselected;

		public event TileDelegate TileHovered;

		public event TileDelegate TileDeHovered;


		TileHandle hoveredTile;
		TileHandle selectedTile;



		public override bool OnHover (Transform obj, Vector3 point)
		{
			TileHandle handle = Collection.MapHandle.GetHandle (point);
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
			TileHandle handle = Collection.MapHandle.GetHandle (point);
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

		protected override void Setup (ITable definesTable)
		{
			
			GameObject go = GameObject.Find ("MapCollider");
			CollectionHandle handle = go.AddComponent<CollectionHandle> ();
			//	handle.Layer = Find.Root<MapRoot.Map> ().GetLayer (Layer.Name);
			Transform transform = go.transform;
			transform.position = new Vector3 (Collection.MapHandle.SizeX / 2, Collection.MapHandle.SizeY / 2, 0);
			transform.localScale = new Vector3 (Collection.MapHandle.SizeX, Collection.MapHandle.SizeY, 0);

		}

	}
}

