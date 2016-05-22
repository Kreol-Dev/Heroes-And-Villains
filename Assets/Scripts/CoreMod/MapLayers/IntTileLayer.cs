using UnityEngine;
using System.Collections;
using MapRoot;

namespace CoreMod
{
	public class IntTileLayer : MapLayer, ITileMapLayer<int>
	{
		protected override void Setup (UIO.ITable definesTable, MapRoot.Map mapRoot)
		{
			
		}

		public MapHandle MapHandle { get; internal set; }

		public int[,] Tiles { get; internal set; }

		public Signals.Signal<TileHandle> TileUpdated { get; internal set; }


		
	}
}
