using UnityEngine;
using System.Collections;
using MapRoot;

namespace CoreMod
{
	public class TileMapLayer : MapLayer
	{
		public MapHandle MapHandle { get; internal set; }

		protected override void Setup (Demiurg.Core.Extensions.ITable definesTable)
		{
			MapHandle = Find.Root<TilesRoot> ().MapHandle;
		}

	}
}


