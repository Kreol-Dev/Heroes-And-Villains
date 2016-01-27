using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public delegate void TileDelegate<T> (TileHandle tile, T obj);
	public interface ITileMapInteractor<TObject, TLayerObject, TLayer> where TLayer : ITileMapLayer<TLayerObject>
	{
		event TileDelegate<TObject> TileSelected;

		event TileDelegate<TObject> TileDeselected;

		event TileDelegate<TObject> TileHovered;

		event TileDelegate<TObject> TileDeHovered;
	}
}


