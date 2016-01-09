using UnityEngine;
using System.Collections;

namespace CoreMod
{
    public class GraphicsLayerInteractor : TileMapLayerInteractor<GraphicsTile, GraphicsTile, MapGraphicsLayer>
    {

        public override GraphicsTile ObjectFromLayerObject (GraphicsTile obj)
        {
            return obj;
        }



    }
}


