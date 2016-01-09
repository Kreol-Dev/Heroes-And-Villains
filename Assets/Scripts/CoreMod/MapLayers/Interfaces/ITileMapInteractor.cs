using UnityEngine;
using System.Collections;

namespace CoreMod
{
    public delegate void TileDelegate<T> (TileHandle tile, T obj);
    public interface ITileMapInteractor<TObject, TLayerObject, TLayer> where TLayer : ITileMapLayer<TLayerObject>
    {
        event TileDelegate<TObject> TileClicked;
        event TileDelegate<TObject> TileDeClicked;
        event TileDelegate<TObject> TileRightClicked;
        event TileDelegate<TObject> TileDeRightClicked;
        event TileDelegate<TObject> TileHighlighted;
        event TileDelegate<TObject> TileDeHighlighted;
        event TileDelegate<TObject> TileHovered;
        event TileDelegate<TObject> TileDeHovered;
    }
}


