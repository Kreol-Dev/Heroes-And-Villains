using UnityEngine;
using System.Collections;
using Demiurg.Core;
using System.Collections.Generic;
using System.Linq;

namespace CoreMod
{
	public class RegionsModule : SlotsProcessor
	{
		[AConfig ("is_region")]
		bool isRegion;
		[AConfig ("size")]
		int maxSize;
		[AConfig ("density")]
		int density;
		[AConfig ("name")]
		string slotName;
		[AConfig ("target_layer")]
		string targetLayerName;
		System.Random random;
		MapHandle map;
		Deck<TileDirection> dirs;
		[AOutput ("environment")]
		int[,] env;

		int regionID = 0;

		public override void Work ()
		{
			random = new System.Random (Random.Next ());

			map = Find.Root<TilesRoot> ().MapHandle;
			if (map.TileConnectivity == TileConnnectivity.Four)
				dirs = new Deck<TileDirection> (random, TileDirection.East, TileDirection.West, TileDirection.North, TileDirection.South);
			else
				dirs = new Deck<TileDirection> (random, TileDirection.East, TileDirection.West, TileDirection.North, TileDirection.South,
				                                TileDirection.NorthEast, TileDirection.NorthWest, TileDirection.SouthEast, TileDirection.SouthWest);

			OutputObjects = new List<GameObject> ();
			env = new int[map.SizeX, map.SizeY];
			for (int i = 0; i < map.SizeX; i++)
				for (int j = 0; j < map.SizeY; j++)
					env [i, j] = -2;
			
			foreach (var go in InputObjects)
			{
				ProcessChunk (go.GetComponent<ChunkSlot> ());

			}

			FinishWork ();
		}



		void ProcessChunk (ChunkSlot chunk)
		{
			int startingPointsCount = chunk.Tiles.Length / density + 1;
			foreach (var tile in chunk.Tiles)
				tile.Set (env, -1);
			HashSet<int> tileIDs = new HashSet<int> ();
			while (tileIDs.Count < startingPointsCount)
			{
				int id;
				do
				{
					id = random.Next (0, chunk.Tiles.Length);
				} while (tileIDs.Contains (id));
				tileIDs.Add (id);
			}

			var handles = from id in tileIDs
			              select chunk.Tiles [id];

			List<Region> regions = new List<Region> ();
			foreach (var handle in handles)
			{
				Region region = new Region (regionID++, handle, env, dirs, maxSize);
				regions.Add (region);
			}

			bool updated = false;
			do
			{
				updated = false;
				//Debug.LogWarning ("STEP __________________________________ " + steps);
				foreach (var region in regions)
				{

					dirs.Shuffle ();
					if (region.Update ())
					{
						//Debug.LogWarningFormat ("UPDATED REGION {0}", region.ID);
						updated = true;
					}
				}
			} while(updated);

			foreach (var region in regions)
			{
				GameObject go = new GameObject (slotName + " Region " + region.ID);
				RegionSlot regionSlot = go.AddComponent<RegionSlot> ();
				regionSlot.Tiles = region.Tiles;
				regionSlot.TargetLayerName = targetLayerName;
				regionSlot.IsRegion = isRegion;
				go.AddComponent<Slot> ();
				go.AddComponent<SlotSurface> ().SurfaceID = chunk.Surface;
				OutputObjects.Add (go);
			}
		}



	}

	public class Region
	{
		public List<TileHandle> Tiles { get { return tiles; } }

		List<TileHandle> tiles = new List<TileHandle> ();
		List<TileHandle> frontier = new List<TileHandle> ();
		List<TileHandle> cachedFrontier = new List<TileHandle> ();

		public int ID { get; internal set; }

		int[,] env;
		Deck<TileDirection> dirs;
		int maxSize;

		public Region (int id, TileHandle startHandle, int[,] env, Deck<TileDirection> dirs, int maxSize)
		{
			if (maxSize == 0)
				this.maxSize = int.MaxValue;
			else
				this.maxSize = maxSize;
			this.ID = id;
			this.frontier.Add (startHandle);
			startHandle.Set (env, id);
			this.env = env;
			this.dirs = dirs;
		}

		public bool Update ()
		{
			cachedFrontier.Clear ();

			if (tiles.Count + frontier.Count > maxSize)
			{
				Debug.LogFormat ("MAX {0} COUNT {1}", maxSize, tiles.Count + frontier.Count);
				foreach (var tile in frontier)
					tiles.Add (tile);
				frontier.Clear ();
			}
			bool update = frontier.Count > 0;

			for (int i = 0; i < frontier.Count; i++)
			{
				int j = 0;
				for (; j < dirs.values.Length; j++)
				{
					TileHandle nextHandle = GrowTile (frontier [i], dirs.values [j]);
					if (nextHandle != null)
					{
						nextHandle.Set (env, ID);
						cachedFrontier.Add (nextHandle);
						break;
					}
				}
				if (j < dirs.values.Length)
					cachedFrontier.Add (frontier [i]);
				else
					tiles.Add (frontier [i]);
					
					
				
			}
			var temp = frontier;
			temp.Clear ();
			frontier = cachedFrontier;
			cachedFrontier = temp;
//			if (update)
//				Debug.LogWarningFormat ("UPDATED Region {0} tiles {1} frontier {2}", ID, tiles.Count, frontier.Count);

			return frontier.Count > 0;
		}

		TileHandle GrowTile (TileHandle handle, TileDirection direction)
		{
			TileHandle nextHandle = null;
			int nextID = -1;

			nextHandle = handle.GetNext (direction);
			if (nextHandle != null)
			{
				nextID = nextHandle.Get (env);
				if (nextID == -1)
					return nextHandle;
			}
			return null;
		}
	}



}


