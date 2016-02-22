using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace MapRoot
{
	public interface IMapLayerRenderer
	{
		void Setup (IMapLayer layer, IMapLayerInteractor interactor, RepresenterState defaultState, ITable rendererData);

		void ChangeState (RepresenterState state);
	}

	public abstract class BaseMapLayerRenderer<TLayer, TInteractor> : IMapLayerRenderer where 
	TLayer : class, IMapLayer where TInteractor : class, IMapLayerInteractor
	{
		Scribe scribe = Scribes.Find ("MAP LAYER RENDERER");

		protected TLayer Layer { get; private set; }

		protected TInteractor Interactor { get; private set; }

		public void Setup (IMapLayer layer, IMapLayerInteractor interactor, RepresenterState defaultState, ITable rendererData)
		{
			Layer = layer as TLayer;
			if (Layer == null)
			{
				scribe.LogFormatError ("Layer provided to presenter is of wrong type {0} while assumed {1}", layer.GetType (), typeof(TLayer));
				return;
			}
			Interactor = interactor as TInteractor;
			if (Interactor == null)
			{
				scribe.LogFormatError ("Interactor provided to presenter is of wrong type {0} while assumed {1}", interactor.GetType (), typeof(TInteractor));
				return;
			}
			ITable table = Find.Root<ModsManager> ().GetTable ("defines");
			if (table == null)
				return;
			Setup (table, rendererData);
			ChangeState (defaultState);
		}

		public abstract void ChangeState (RepresenterState state);

		protected abstract void Setup (ITable definesTable, ITable rendererData);
        
	}

	public class NullRenderer : IMapLayerRenderer
	{
		public void Setup (IMapLayer layer, IMapLayerInteractor interactor, RepresenterState defaultState, ITable rendererData)
		{
		}

		public void ChangeState (RepresenterState state)
		{
		}
		
	}
}


