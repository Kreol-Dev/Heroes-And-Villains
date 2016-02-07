using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace MapRoot
{
	public interface IMapLayerInteractor
	{
		void Setup (IMapLayer layer, InteractorState defaultState);

		bool OnHover (Transform target, Vector3 point);

		bool OnClick (Transform target, Vector3 point);


		void OnUpdate ();
	}

	public abstract class BaseMapLayerInteractor<TLayer> : IMapLayerInteractor where TLayer : class, IMapLayer
	{

		protected Scribe Scribe = Scribes.Find ("INTERACTORS");
		TLayer layer;

		protected TLayer Layer { get { return layer; } }

		MapInteractor mapInteractor;

		public void Setup (IMapLayer layer, InteractorState defaultState)
		{

			Scribe.LogFormat ("Interactor {0} start working with a layer {1}", this.GetType (), layer.Name);
			this.layer = layer as TLayer;
			if (this.layer == null)
			{
				Scribe.LogFormatError ("Interactor doesn't match layer provided: interactor type is {0} while layer {1}", this.GetType (), layer.GetType ());
				return;
			}
			mapInteractor = Find.Root<MapInteractor> ();
			ITable table = Find.Root<ModsManager> ().GetTable ("defines");
			if (table != null)
				this.Setup (table);
		}

		protected abstract void Setup (ITable definesTable);

		public abstract bool OnHover (Transform obj, Vector3 point);

		public abstract bool OnClick (Transform obj, Vector3 point);


		public abstract void OnUpdate ();
	}
}

