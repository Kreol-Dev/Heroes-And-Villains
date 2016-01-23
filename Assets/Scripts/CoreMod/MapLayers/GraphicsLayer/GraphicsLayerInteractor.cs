using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class GraphicsLayerInteractor : TileMapLayerInteractor<GraphicsTile, GraphicsTile, MapGraphicsLayer>
	{

		public override bool ObjectFromLayerObject (GraphicsTile obj, out GraphicsTile outObj)
		{
			outObj = obj;
			return obj != null;
		}



	}
}


