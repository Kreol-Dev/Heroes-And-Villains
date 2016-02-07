using UnityEngine;
using System.Collections;
using Signals;
using MapRoot;

namespace CoreMod
{
	public interface ITileMapLayer<T>
	{
		T[,] Tiles { get; }

		Signal<TileHandle, T> TileUpdated { get; }

		Signal MassUpdate { get; }

	}

}