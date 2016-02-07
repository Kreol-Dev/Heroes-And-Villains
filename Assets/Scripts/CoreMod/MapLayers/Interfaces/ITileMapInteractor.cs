using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public delegate void TileDelegate (TileHandle tile);
	public interface ITileMapInteractor
	{
		event TileDelegate TileSelected;

		event TileDelegate TileDeselected;

		event TileDelegate TileHovered;

		event TileDelegate TileDeHovered;
	}

	public delegate void ObjectDelegate<T> (T obj);
	public interface IObjectsInteractor<TObject, TLayer>
	{
		event ObjectDelegate<TObject> ObjectSelected;

		event ObjectDelegate<TObject> ObjectDeSelected;

		event ObjectDelegate<TObject> ObjectHovered;

		event ObjectDelegate<TObject> ObjectDeHovered;
	}
}


