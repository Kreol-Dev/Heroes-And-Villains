using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;

namespace CoreMod
{
    public class MapGraphicsLayer : MapLayer, ITileMapLayer<GraphicsTile>
    {
        public Signals.Signal<TileHandle, GraphicsTile> TileUpdated { get; internal set; }

        public Signals.Signal MassUpdate { get; internal set; }

        public GraphicsTile[,] Tiles { get; internal set; }



        protected override void Setup (ITable definesTable)
        {
            TileUpdated = new Signals.Signal<TileHandle, GraphicsTile> ();
            MassUpdate = new Signals.Signal ();
            int height = (int)(double)definesTable.Get ("MAP_HEIGHT");
            int width = (int)(double)definesTable.Get ("MAP_WIDTH");
            Tiles = new GraphicsTile[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {

                    Tiles [i, j] = defaultTile;

                }
            MassUpdate.Dispatch ();
        }

        readonly GraphicsTile defaultTile = new GraphicsTile (Resources.Load<Sprite> ("Default"), 0, "Placeholder Tile");

    }
}


