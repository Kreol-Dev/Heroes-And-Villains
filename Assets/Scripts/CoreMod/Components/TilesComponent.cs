using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public delegate void NamedTileDelegate (TilesComponent cmp, TileHandle handle);
	public class TilesComponent : EntityComponent
	{
		public event NamedTileDelegate TileAdded;
		public event NamedTileDelegate TileRemoved;

		public void AddTile (TileHandle handle)
		{
			if (tiles.Add (handle))
				TileAdded (this, handle);
		}

		public void RemoveTile (TileHandle handle)
		{
			if (tiles.Remove (handle))
				TileRemoved (this, handle);
		}


		public override void LoadFromTable (Demiurg.Core.Extensions.ITable table)
		{
			
		}

		public override void CopyTo (GameObject go)
		{
		}


		public override void PostCreate ()
		{
			Find.Root<MapRoot.Map> ().GetCollection<GOCollection> (name).AddObject (this.gameObject);
		}

		public IEnumerable<TileHandle> Tiles { get { return tiles; } }

		HashSet<TileHandle> tiles = new HashSet<TileHandle> ();


	}

}


