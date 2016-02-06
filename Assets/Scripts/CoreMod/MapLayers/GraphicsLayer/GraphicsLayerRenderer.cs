using UnityEngine;
using System.Collections;
using MapRoot;

namespace CoreMod
{
	public class GraphicsLayerRenderer : AbstractSpriteMapRenderer<GraphicsTile, MapGraphicsLayer, TileMapCollection>
	{
		protected override GraphicsTile GetLayerObject (int x, int y)
		{
			return Collection.MapHandle.GetHandle (x, y).Get (Layer.Tiles);
		}

		protected override void RegisterCallbacks (System.Action updateAll, System.Action<TileHandle> updateTile)
		{
			throw new System.NotImplementedException ();
		}

		protected override void UnregisterCallbacks (System.Action updateAll, System.Action<TileHandle> updateTile)
		{
			throw new System.NotImplementedException ();
		}

		protected override int SizeX {
			get
			{
				throw new System.NotImplementedException ();
			}
		}

		protected override int SizeY {
			get
			{
				throw new System.NotImplementedException ();
			}
		}

		protected override Sprite GetSprite (GraphicsTile obj)
		{
			return obj.Sprite;
		}
        

	}
    
}


