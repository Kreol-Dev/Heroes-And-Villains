using UnityEngine;
using System.Collections;
using Signals;

namespace CoreMod
{
	public interface ITiledMapLayer<T, TIdentifier> where T : class
	{
		int MapSizeX { get; }

		int MapSizeY { get; }

		event ObjectDelegate<T> ObjectChanged;

		event TileDelegate TileChanged;

		T GetObject (TileHandle handle);

		T GetObject (TIdentifier id);


	}

}

