using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class GraphicsLayerPresenter : TileMapLayerPresenter<GraphicsTile, GraphicsTile, MapGraphicsLayer, TileMapCollectionInteractor, TileMapCollection>
	{
		protected override GraphicsTile GetObjectFromLayer (GraphicsTile obj)
		{
			return obj;
		}




	}
}


