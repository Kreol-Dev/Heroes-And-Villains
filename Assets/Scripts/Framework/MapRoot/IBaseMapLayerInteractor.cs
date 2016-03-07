using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;

namespace MapRoot
{
	public interface IMapLayerInteractor
	{
		int Priority { get; }

		void Setup (IMapLayer layer, InteractorState defaultState, int priority);

		void OnHover (Vector2 point, HashSet<Transform> encounters, ref HashSet<object> hoveredObjects);

		object OnSelect (Vector2 point, HashSet<object> selectables);

		object OnDeSelect (Vector2 point);

		IEnumerable<object> OnDeselectAll ();

		IEnumerable<object> OnMassSelect (Vector2 minCorner, Vector2 maxCorner);
	}

	public abstract class BaseMapLayerInteractor<TLayer> : IMapLayerInteractor  /* where TLayer :class, IMapLayer*/
	{
		public int Priority { get; internal set; }

		public abstract void OnHover (Vector2 point, HashSet<Transform> encounters, ref HashSet<object> hoveredObjects);

		public abstract object OnSelect (Vector2 point, HashSet<object> selectables);

		public abstract object OnDeSelect (Vector2 point);

		public abstract IEnumerable<object> OnDeselectAll ();

		public abstract IEnumerable<object> OnMassSelect (Vector2 minCorner, Vector2 maxCorner);

		

		protected Scribe Scribe = Scribes.Find ("INTERACTORS");
		TLayer layer;

		protected TLayer Layer { get { return layer; } }


		public void Setup (IMapLayer layer, InteractorState defaultState, int priority)
		{
			Priority = priority;
			Scribe.LogFormat ("Interactor {0} start working with a layer {1}", this.GetType (), layer.Name);
			try
			{
				this.layer = (TLayer)layer;
			} catch
			{
				Scribe.LogFormatError ("Interactor doesn't match layer provided: interactor type is {0} while layer {1}", this.GetType (), layer.GetType ());
				return;
			}
			ITable table = Find.Root<ModsManager> ().GetTable ("defines");
			if (table != null)
				this.Setup (table);
		}

		protected abstract void Setup (ITable definesTable);
	}
}

