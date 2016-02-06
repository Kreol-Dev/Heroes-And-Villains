using UnityEngine;
using System.Collections;
using MapRoot;

namespace CoreMod
{
	public abstract class TiledSpriteRenderer<TLayerObject, TLayer, TCollectionObject, TCollection> : AbstractSpriteMapRenderer<TLayerObject, TLayer, TCollection>
		where TLayer : MapLayer<TCollection>, ITiledMapLayer<TLayerObject, TCollectionObject> where TCollection: TiledObjectsMapCollection<TCollectionObject> where TLayerObject : class where TCollectionObject : class
	{

		protected override TLayerObject GetLayerObject (int x, int y)
		{
			return Layer.GetObject (Collection.GetObjectID (Collection.MapHandle.GetHandle (x, y)));
		}

		protected override void RegisterCallbacks (System.Action updateAll, System.Action<TileHandle> updateTile)
		{
			
		}

		protected override void UnregisterCallbacks (System.Action updateAll, System.Action<TileHandle> updateTile)
		{
		}

		protected override int SizeX {
			get
			{
			}
		}

		protected override int SizeY {
			get
			{
			}
		}



	}
}


