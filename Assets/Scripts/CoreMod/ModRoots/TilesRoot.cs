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
			int height = definesTable.GetInt ("MAP_HEIGHT");
			int width = definesTable.GetInt ("MAP_WIDTH");
			MapConnectivity connectivity = (MapConnectivity)Enum.Parse (typeof(MapConnectivity), definesTable.GetString ("MAP_CONNECTIVITY"));
			TileConnnectivity tilesConnectivity = (TileConnnectivity)Enum.Parse (typeof(TileConnnectivity), definesTable.GetString ("TILES_CONNECTIVITY"));
			MapHandle = new MapHandle (width, height, connectivity, tilesConnectivity);
			Fulfill.Dispatch ();
		}


	}
}


