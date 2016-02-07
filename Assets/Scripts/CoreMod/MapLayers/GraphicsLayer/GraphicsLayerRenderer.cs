using UnityEngine;
using System.Collections;
using MapRoot;

namespace CoreMod
{
    public class GraphicsLayerRenderer : SpriteMapRenderer<GraphicsTile, GraphicsTile, MapGraphicsLayer, GraphicsLayerInteractor>
    {
        protected override Sprite GetSprite (GraphicsTile obj)
        {
            return obj.Sprite;
        }
        

    }
    
}


