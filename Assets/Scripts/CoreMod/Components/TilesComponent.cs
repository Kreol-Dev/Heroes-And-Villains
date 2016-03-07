using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	public class TilesComponent : EntityComponent, ISlotted<RegionSlot>
	{
		public event TileDelegate TileAdded;
		public event TileDelegate TileRemoved;

		public override void LoadFromTable (ITable table)
		{
			
		}

		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<TilesComponent> ();
		}

		RegionsLayer layer;

		public void Receive (RegionSlot data)
		{
			tiles = new HashSet<TileHandle> (data.Tiles);
			layer = Find.Root<MapRoot.Map> ().GetLayer<RegionsLayer> (data.TargetLayerName);
			foreach (var tile in tiles)
			{
				tile.Set (layer.Tiles, this.gameObject);
				layer.TileUpdated.Dispatch (tile);
			}
			//layer.AddObject (this);
			//layer.gos.Add (this.gameObject);
		}


		HashSet<TileHandle> tiles;

		public IEnumerable<TileHandle> Tiles { get { return tiles; } }

		public bool AddTile (TileHandle handle)
		{
			if (handle.Get (layer.Tiles) != null)
				return false;
			bool success = tiles.Add (handle);
			if (success)
			{
				handle.Set (layer.Tiles, this.gameObject);
				TileAdded (handle);
			}
			return success;
				
		}

		public bool RemoveTile (TileHandle handle)
		{
			if (handle.Get (layer.Tiles) != this.gameObject)
				return false;
			bool success = tiles.Remove (handle);
			if (success)
			{
				handle.Set (layer.Tiles, null);
				TileRemoved (handle);
			}
			return success;
		}

		public TileHandle FindFree (TileHandle handle)
		{
			return null;
		}

		public IEnumerable<TileHandle> FindAllFreeNext (TileHandle handle)
		{
			return null;
		}

		public override void PostCreate ()
		{

		}

	}

}


