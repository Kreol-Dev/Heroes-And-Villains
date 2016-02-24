using UnityEngine;
using System.Collections;
using MapRoot;
using UIO;

namespace CoreMod
{
	public class TilesLayer<TObject> : MapLayer, ITileMapLayer<TObject> where TObject : class
	{
		public MapHandle MapHandle { get; internal set; }

		public Signals.Signal<TileHandle> TileUpdated { get; internal set; }

		public Signals.Signal MassUpdate { get; internal set; }

		public TObject[,] Tiles { get; internal set; }



		protected override void Setup (ITable definesTable)
		{
			TileUpdated = new Signals.Signal<TileHandle> ();
			MassUpdate = new Signals.Signal ();
			var map = Find.Root<TilesRoot> ().MapHandle;
			MapHandle = map;
			Tiles = new TObject[map.SizeX, map.SizeY];
			for (int i = 0; i < map.SizeX; i++)
				for (int j = 0; j < map.SizeY; j++)
				{

					Tiles [i, j] = null;

				}
			MassUpdate.Dispatch ();
		}


	}
}

