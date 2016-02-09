using UnityEngine;
using System.Collections;
using Signals;
using MapRoot;

namespace CoreMod
{
	public interface ITileMapLayer<T>
	{
		MapHandle MapHandle { get; }

		T[,] Tiles { get; }

		Signal<TileHandle> TileUpdated { get; }
	}

}