using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreMod
{
	public class ReacheableSpace : EntityComponent
	{
		System.Random random;
		ClearanceLayer clearanceLayer;
		IntTileLayer surfacesLayer;

		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<ReacheableSpace> ();
		}

		public override void PostCreate ()
		{
			random = new System.Random (Find.Root<ModsManager> ().GetTable ("defines").GetInt ("SEED"));
		}

		protected override void PostDestroy ()
		{
		}


		public override void OnInitPrefab ()
		{
			clearanceLayer = Find.Root<MapRoot.Map> ().GetLayer<ClearanceLayer> ("clearance_layer");
			surfacesLayer = Find.Root<MapRoot.Map> ().GetLayer<IntTileLayer> ("surfaces_layer");
		}

		List<TileHandle> tiles = new List<TileHandle> ();

		public void AddTile (TileHandle tile)
		{
			tiles.Add (tile);
		}

		public void RemoveTile (TileHandle tile)
		{
			tiles.Remove (tile);
		}

		public List<TileHandle> GetTiles (int surface, int size)
		{
			var tilesForSize = clearanceLayer.GetTilesForSize (size);
			List<TileHandle> list = new List<TileHandle> ();
			for (int i = 0; i < tiles.Count; i++)
			{
				if (tiles [i].Get (surfacesLayer.Tiles) == surface && tilesForSize.Contains (tiles [i]))
					list.Add (tiles [i]);
					
			}

			return list;
		}

		public TileHandle GetRandomTile (List<TileHandle> tiles)
		{
			if (tiles.Count == 0)
				return null;
			return tiles [random.Next (0, tiles.Count)];
		}
	}
}

