using UnityEngine;
using System.Collections;
using MapRoot;
using UIO;

namespace CoreMod
{
	public class TileMapLayer : MapLayer
	{
		public MapHandle MapHandle { get; internal set; }

		protected override void Setup (ITable definesTable)
		{
			MapHandle = Find.Root<TilesRoot> ().MapHandle;
		}

	}
}


