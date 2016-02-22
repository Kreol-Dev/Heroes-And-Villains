using UnityEngine;
using System.Collections;
using MapRoot;

namespace CoreMod
{
	public class GraphicsLayerRenderer : SpriteMapRenderer<GraphicsTile, MapGraphicsLayer>
	{

		protected override void ReadRules (Demiurg.Core.Extensions.ITable rulesTable)
		{
		}

		protected override Sprite GetSprite (GraphicsTile obj)
		{
			return obj.Sprite;
		}
        

	}
    
}


