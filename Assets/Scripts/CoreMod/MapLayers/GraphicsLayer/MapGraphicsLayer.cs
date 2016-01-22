using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System;

namespace CoreMod
{
	public class MapGraphicsLayer : MapLayer, ITileMapLayer<GraphicsTile>
	{
		public MapHandle MapHandle { get; internal set; }

		public Signals.Signal<TileHandle, GraphicsTile> TileUpdated { get; internal set; }

		public Signals.Signal MassUpdate { get; internal set; }

		public GraphicsTile[,] Tiles { get; internal set; }



		protected override void Setup (ITable definesTable)
		{
			foreach (var  UV in defaultTile.Sprite.uv)
				Debug.LogWarning (UV);
			TileUpdated = new Signals.Signal<TileHandle, GraphicsTile> ();
			MassUpdate = new Signals.Signal ();
			var map = Find.Root<TilesRoot> ().MapHandle;
				
			Tiles = new GraphicsTile[map.SizeX, map.SizeY];
			for (int i = 0; i < map.SizeX; i++)
				for (int j = 0; j < map.SizeY; j++)
				{

					Tiles [i, j] = defaultTile;

				}
			MassUpdate.Dispatch ();
		}

		readonly GraphicsTile defaultTile = new GraphicsTile (Resources.Load<Sprite> ("Default"), 0, "Placeholder Tile");

	}
}


