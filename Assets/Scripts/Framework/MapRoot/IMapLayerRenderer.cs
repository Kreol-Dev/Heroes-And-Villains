using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace MapRoot
{
	public interface IMapLayerRenderer
	{
		void Setup (IMapLayer layer, IMapCollection collection, RepresenterState defaultState);

		void ChangeState (RepresenterState state);
	}

	public abstract class BaseMapLayerRenderer<TLayer, TCollection> : IMapLayerRenderer where 
	TLayer : MapLayer<TCollection> where TCollection : class, IMapCollection
	{
		Scribe scribe = Scribes.Find ("MAP LAYER RENDERER");

		protected TLayer Layer { get; private set; }

		protected TCollection Collection { get; private set; }

		public void Setup (IMapLayer layer, IMapCollection collection, RepresenterState defaultState)
		{
			Layer = layer as TLayer;
			if (Layer == null)
			{
				scribe.LogFormatError ("Layer provided to renderer is of wrong type {0} while assumed {1}", layer.GetType (), typeof(TLayer));
				return;
			}
			Collection = collection as TCollection;
			if (Collection == null)
			{
				scribe.LogFormatError ("Collection provided to a renderer is of wrong type {0} while assumed {1}", layer.GetType (), typeof(TCollection));
				return;
			}
			ITable table = Find.Root<ModsManager> ().GetTable ("defines");
			if (table == null)
				return;
			Setup (table);
			ChangeState (defaultState);
		}

		public abstract void ChangeState (RepresenterState state);

		protected abstract void Setup (ITable definesTable);
        
	}
}


