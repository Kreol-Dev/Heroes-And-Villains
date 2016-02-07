using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class GraphicsLayerPresenter : TileMapLayerPresenter<GraphicsTile, GraphicsTile, MapGraphicsLayer, GraphicsLayerInteractor>
	{
		public override GraphicsTile ObjectFromLayer (GraphicsTile obj)
		{
			return obj;
		}



	}
}


