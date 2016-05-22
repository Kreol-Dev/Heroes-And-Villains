using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreMod
{
	public class ClearanceLayer : MapLayer
	{
		int[,] environment;
		Dictionary<TileHandle, int> updatedTiles = new Dictionary<TileHandle, int> ();
		Dictionary<int, OrderedDictionary> tilesByClearance = new Dictionary<int, OrderedDictionary> ();
		MapHandle map;

		protected override void Setup (UIO.ITable definesTable, MapRoot.Map mapRoot)
		{
			map = Find.Root<TilesRoot> ().MapHandle;
			environment = new int[map.SizeX, map.SizeY];
			for (int i = 0; i < map.SizeX; i++)
				CalculateClearance (map.SizeX - i, map.SizeY);
			for (int i = 0; i < map.SizeY; i++)
				CalculateClearance (map.SizeX, map.SizeY - i);
			updatedTiles.Clear ();
			for (int i = 0; i < map.SizeX; i++)
				for (int j = 0; j < map.SizeY; j++)
				{
					OrderedDictionary tiles;
					if (!tilesByClearance.TryGetValue (environment [i, j], out tiles))
					{
						tiles = new OrderedDictionary ();
						tilesByClearance.Add (environment [i, j], tiles);
					}
					tiles.Add (map.GetHandle (i, j), map.GetHandle (i, j));
				}
		}

		public OrderedDictionary GetTilesForSize (int size)
		{
			OrderedDictionary tiles = null;
			tilesByClearance.TryGetValue (size, out tiles);
			return tiles;
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


		public void PlaceStructure (TileHandle tile, Structure structure)
		{
			for (int x = 0; x < structure.Mask.GetLength (0); x++)
				for (int y = 0; y < structure.Mask.GetLength (1); y++)
				{
					if (structure.Mask [x, y] == true)
					{

						var curTile = map.GetHandle (tile.X + x, tile.Y + y);
						//regionCmp.Tiles.Add (curTile);
						if (!updatedTiles.ContainsKey (curTile))
							updatedTiles.Add (curTile, curTile.Get (environment));
						curTile.Set (environment, 0);
					}
				}
			for (int x = 0; x < structure.Mask.GetLength (0); x++)
				for (int y = 0; y < structure.Mask.GetLength (1); y++)
				{
					if (structure.Mask [x, y] == true)
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



	}
}


