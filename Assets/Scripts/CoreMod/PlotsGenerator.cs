using UnityEngine;
using System.Collections;
using Demiurg.Core;
using System.Collections.Generic;
using System.Collections.Specialized;
using UIO;

namespace CoreMod
{
	[ASlotComponent ("Plot")]
	[AShared]
	public class Plot : Slot
	{
		public int Size;
		public Vector2 Center;
		public ObjectCreationHandle.PlotType PlotType;
		public int ID;

		public override void FillComponent (GameObject go)
		{
			base.FillComponent (go);
			go.transform.position = Center;
		}
	}

	public class PlotsGenerator : Demiurg.Core.Avatar
	{
		[AInput ("surfaces")]
		int[,] surfaces;
		[AConfig ("target_surface")]
		int targetSurface;
		[AConfig ("plots_count")]
		int plotsCount;
		[AConfig ("groups")]
		TagsCollection groups;
		[AOutput ("plots")]
		List<GameObject> plots;
		List<TileMask> sizesDistribution = new List<TileMask> ();
		Dictionary<ObjectCreationHandle.PlotType, Dictionary<int, int>> sizeToCount;
		[AOutput ("env")]
		int[,] environment;
		MapHandle map;
		Dictionary<TileHandle, int> updatedTiles = new Dictionary<TileHandle, int> ();
		Dictionary<int, OrderedDictionary> tilesByClearance = new Dictionary<int, OrderedDictionary> ();

		public override void Work ()
		{
			plots = new List<GameObject> ();
			map = Find.Root<TilesRoot> ().MapHandle;
			var namespaces = Find.Root<ObjectsRoot> ().GetAllNamespaces ();
			sizeToCount = new Dictionary<ObjectCreationHandle.PlotType, Dictionary<int, int>> ();
			foreach (var oNamespace in namespaces)
			{
				var objects = oNamespace.FindAvailable (groups);
				foreach (var obj in objects)
				{
					
					Dictionary<int, int> sizes = null;

					if (sizeToCount.ContainsKey (obj.Plot))
					{
						sizes = sizeToCount [obj.Plot];
					} else
					{
						sizes = new Dictionary<int, int> ();
						sizeToCount.Add (obj.Plot, sizes);
					}
					int count = -1;
					if (sizes.TryGetValue (obj.PlotSize, out count))
						sizes [obj.PlotSize] = count + 1;
					else
						sizes.Add (obj.PlotSize, 1);
				}
			}
			foreach (var sizeToCountPair in sizeToCount)
			{
				foreach (var sizePair in sizeToCountPair.Value)
				{
					TileMask mask = new TileMask (sizePair.Key, sizeToCountPair.Key);
					for (int i = 0; i < sizePair.Value; i++)
						sizesDistribution.Add (mask);
				}
			}
			environment = new int[surfaces.GetLength (0), surfaces.GetLength (1)];
			for (int i = 0; i < surfaces.GetLength (0); i++)
				for (int j = 0; j < surfaces.GetLength (1); j++)
					if (surfaces [i, j] != targetSurface)
						environment [i, j] = 0;
					else
						environment [i, j] = int.MaxValue;
			for (int i = 0; i < surfaces.GetLength (0); i++)
				for (int j = 0; j < surfaces.GetLength (1); j++)
					if (surfaces [i, j] != targetSurface)
						CalculateClearance (i, j);
			for (int i = 0; i < surfaces.GetLength (0); i++)
				CalculateClearance (surfaces.GetLength (0) - i, surfaces.GetLength (1));
			for (int i = 0; i < surfaces.GetLength (0); i++)
				CalculateClearance (surfaces.GetLength (0), surfaces.GetLength (1) - i);
			updatedTiles.Clear ();
			for (int i = 0; i < surfaces.GetLength (0); i++)
				for (int j = 0; j < surfaces.GetLength (1); j++)
				{
					OrderedDictionary tiles;
					if (!tilesByClearance.TryGetValue (environment [i, j], out tiles))
					{
						tiles = new OrderedDictionary ();
						tilesByClearance.Add (environment [i, j], tiles);
					}
					tiles.Add (map.GetHandle (i, j), map.GetHandle (i, j));
				}

			//plotsCount = 0;
			var sprite = Resources.Load<Sprite> ("Default");
			int id = 0;
			for (int i = 0; i < plotsCount; i++)
			{
				var mask = GetRandomMask ();
				OrderedDictionary tiles = null;
				foreach (var pair in tilesByClearance)
				{
					if (pair.Key >= mask.Size)
					{
						if (tiles == null)
							tiles = pair.Value;
						else if (tiles.Count == 0)
						{
							tiles = pair.Value;
						} else if (pair.Value.Count != 0 && Random.Next (0, 2) == 0)
							tiles = pair.Value;
					}
				}
				if (tiles.Count == 0)
					continue;
				int randomTile = Random.Next (tiles.Count);
				TileHandle tile = tiles [randomTile] as TileHandle;
				//tiles.Remove (randomTile)
				GameObject plotGO = new GameObject ("Plot");
				var regionCmp = plotGO.AddComponent<RegionSlot> ();
				plots.Add (plotGO);
				var plotCmp = plotGO.AddComponent<Plot> ();
				foreach (var tag in groups.Tags())
					plotCmp.Tags.AddTag (tag);
					
				plotCmp.Size = mask.Size;
				plotCmp.ID = id++;
				plotCmp.PlotType = mask.Type;
				float offset = (float)(mask.Size - 1) / 2f;
				plotCmp.Center = tile.Center + new Vector2 (offset, offset);
				plotGO.transform.position = plotCmp.Center;
				plotGO.AddComponent<SpriteRenderer> ().sprite = sprite;
				for (int x = 0; x < mask.mask.GetLength (0); x++)
					for (int y = 0; y < mask.mask.GetLength (1); y++)
					{
						if (mask.mask [x, y] == true)
						{
							
							var curTile = map.GetHandle (tile.X + x, tile.Y + y);
							regionCmp.Tiles.Add (curTile);
							if (!updatedTiles.ContainsKey (curTile))
								updatedTiles.Add (curTile, curTile.Get (environment));
							curTile.Set (environment, 0);
						}
					}
				for (int x = 0; x < mask.mask.GetLength (0); x++)
					for (int y = 0; y < mask.mask.GetLength (1); y++)
					{
						if (mask.mask [x, y] == true)
						{
							CalculateClearance (tile.X + x, tile.Y + y);
						}
					}
				
				foreach (var updatedPair in updatedTiles)
				{
					tilesByClearance [updatedPair.Value].Remove (updatedPair.Key);
					tilesByClearance [updatedPair.Key.Get (environment)].Add (updatedPair.Key, updatedPair.Key);
				}
				updatedTiles.Clear ();
			}

			FinishWork ();
		}

		void CalculateClearance (int x, int y)
		{
			int diag = Mathf.Min (x, y);
			for (int i = 0; i <= diag; i++)
			{
				int dX = x - i;
				int dY = y - i;
				if (dX >= environment.GetLength (0) || dY >= environment.GetLength (1))
					continue;



				if (environment [dX, dY] <= i && i != 0)
					break;
				if (!updatedTiles.ContainsKey (map.GetHandle (dX, dY)))
					updatedTiles.Add (map.GetHandle (dX, dY), environment [dX, dY]);
				environment [dX, dY] = i;
				
				if (dX - 1 >= 0)
					for (int j = dX - 1; j >= 0; j--)
						if (environment [j, dY] > (dX - j + i))
						{
							if (!updatedTiles.ContainsKey (map.GetHandle (j, dY)))
								updatedTiles.Add (map.GetHandle (j, dY), environment [j, dY]);
							environment [j, dY] = dX - j + i;
						} else
							break;
				if (dY - 1 >= 0)
					for (int j = dY - 1; j >= 0; j--)
						if (environment [dX, j] > (dY - j + i))
						{
							if (!updatedTiles.ContainsKey (map.GetHandle (dX, j)))
								updatedTiles.Add (map.GetHandle (dX, j), environment [dX, j]);
							environment [dX, j] = dY - j + i;
						} else
							break;
			}
				
		}



		TileMask GetRandomMask ()
		{
			return sizesDistribution [Random.Next (sizesDistribution.Count)];
		}

		struct PlotType
		{
			public int Size;
			public ObjectCreationHandle.PlotType Type;

			public override int GetHashCode ()
			{
				return Size.GetHashCode () + Type.GetHashCode ();
			}
		}

		class TileMask
		{
			public int Size { get; internal set; }

			public ObjectCreationHandle.PlotType Type { get; internal set; }

			public bool[,] mask { get; internal set; }

			public TileMask (int size, ObjectCreationHandle.PlotType plotType)
			{
				Size = size;
				Type = plotType;
				mask = new bool[size, size];
				if (plotType == ObjectCreationHandle.PlotType.Rect || size <= 3)
				{
					for (int i = 0; i < mask.GetLength (0); i++)
						for (int j = 0; j < mask.GetLength (1); j++)
							mask [i, j] = true;
				} else
				{
					//TODO: Actually make circles!
					for (int i = 0; i < mask.GetLength (0); i++)
						for (int j = 0; j < mask.GetLength (1); j++)
							mask [i, j] = true;
				}
			}
		}
	}


}
