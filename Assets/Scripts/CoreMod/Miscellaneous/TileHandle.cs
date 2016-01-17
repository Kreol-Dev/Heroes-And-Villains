using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	public enum MapConnectivity
	{
		Flat,
		Cylinder,
		Sphere

	}

	public enum TileConnnectivity
	{
		Four = 4,
		Eight = 8
	}

	public enum TileDirection
	{
		North,
		South,
		East,
		West,
		SouthWest,
		SouthEast,
		NorthWest,
		NorthEast
	}


	public class MapHandle
	{
		public int SizeX { get; internal set; }

		public int SizeY { get; internal set; }

		public MapConnectivity MapConnectivity { get; internal set; }

		public TileConnnectivity TileConnectivity { get; internal set; }

		TileHandle[,] tiles;

		public MapHandle (int sizeX, int sizeY, MapConnectivity mapConnectivity, TileConnnectivity tileConnectivity)
		{
			SizeX = sizeX;
			SizeY = sizeY;
			MapConnectivity = mapConnectivity;
			TileConnectivity = tileConnectivity;
			tiles = new TileHandle[sizeX, sizeY];
			for (int i = 0; i < sizeX; i++)
				for (int j = 0; j < sizeY; j++)
					tiles [i, j] = new TileHandle (i, j, this);
			
		}


		public TileHandle GetHandle (Vector2 vec)
		{
			return GetHandle ((int)vec.x, (int)vec.y);
		}

		public TileHandle GetHandle (Vector3 vec)
		{
			return GetHandle ((int)vec.x, (int)vec.y);
		}

		public TileHandle GetHandle (int x, int y)
		{
			switch (MapConnectivity) {
			case MapConnectivity.Flat:
				if (x >= 0 && x < SizeX && y >= 0 && y < SizeY)
					return tiles [x, y];
				return null;
			case MapConnectivity.Cylinder:
				if (x < 0)
					x = SizeX - x % SizeX;
				else if (x >= SizeX)
					x = x % SizeX;
				if (y >= 0 && y < SizeY)
					return tiles [x, y];
				else
					return null;
			case MapConnectivity.Sphere:
				return null;
			}
			return null;
		}
	}

	[ATabled]
	public class TileHandle
	{
		public int X { get; internal set; }

		public int Y { get; internal set; }

		public MapHandle Map { get; internal set; }

		public TileHandle (int x, int y, MapHandle map)
		{
			X = x; 
			Y = y;
			Map = map;
		}

		public TileHandle GetNext (TileDirection direction)
		{
			TileHandle handle = null;
			switch (direction) {
			case TileDirection.East:
				handle = Map.GetHandle (X - 1, Y);
				break;
			case TileDirection.West:
				handle = Map.GetHandle (X + 1, Y);
				break;
			case TileDirection.North:
				handle = Map.GetHandle (X, Y + 1);
				break;
			case TileDirection.South:
				handle = Map.GetHandle (X, Y - 1);
				break;
			default:
				if (Map.TileConnectivity == TileConnnectivity.Eight)
					switch (direction) {
					case TileDirection.NorthEast:
						handle = Map.GetHandle (X - 1, Y + 1);
						break;
					case TileDirection.NorthWest:
						handle = Map.GetHandle (X + 1, Y + 1);
						break;
					case TileDirection.SouthWest:
						handle = Map.GetHandle (X - 1, Y - 1);
						break;
					case TileDirection.SouthEast:
						handle = Map.GetHandle (X + 1, Y - 1);
						break;
					}
				break;
			}
			return handle;
		}

		public T Get<T> (T[,] layer)
		{
			return layer [X, Y];
		}

		public void Set<T> (T[,] layer, T value)
		{
			layer [X, Y] = value;
		}

		public static bool operator == (TileHandle A, TileHandle B)
		{
			if (System.Object.ReferenceEquals (A, B))
				return true;
			if (((object)A == null) || ((object)B == null))
				return false;

			return  A.X == B.X && A.Y == B.Y;
		}

		public static bool operator != (TileHandle A, TileHandle B)
		{
			
			return !(A == B);
		}

		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			TileHandle handle = (TileHandle)obj;

			return X == handle.X && Y == handle.Y;

		}

		public override int GetHashCode ()
		{
			unchecked {         
				int hash = 27;
				hash = (13 * hash) + X.GetHashCode ();
				hash = (13 * hash) + Y.GetHashCode ();
				return hash;
			}
		}

	}
}

