using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System;
using MapRoot;

namespace CoreMod
{
	public class TileMapCollection : BaseMapCollection
	{

		public MapHandle MapHandle { get; private set; }

		protected override void Setup (ITable definesTable)
		{
			MapHandle = Find.Root<TilesRoot> ().MapHandle;
		}
	}

}