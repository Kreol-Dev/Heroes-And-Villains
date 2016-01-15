using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System;

namespace CoreMod
{
	[RootDependencies (typeof(ModsManager))]
	public class TilesRoot : ModRoot
	{


		public MapHandle MapHandle { get; internal set; }

		protected override void CustomSetup ()
		{
			ITable definesTable = Find.Root<ModsManager> ().GetTable ("defines");
			int height = (int)(double)definesTable.Get ("MAP_HEIGHT");
			int width = (int)(double)definesTable.Get ("MAP_WIDTH");
			MapConnectivity connectivity = (MapConnectivity)Enum.Parse (typeof(MapConnectivity), (string)definesTable.Get ("MAP_CONNECTIVITY"));
			TileConnnectivity tilesConnectivity = (TileConnnectivity)Enum.Parse (typeof(TileConnnectivity), (string)definesTable.Get ("TILES_CONNECTIVITY"));
			MapHandle = new MapHandle (width, height, connectivity, tilesConnectivity);
		}


	}
}


