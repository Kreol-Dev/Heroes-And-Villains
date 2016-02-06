using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace MapRoot
{
	public interface IMapCollectionInteractor
	{
		void Setup (IMapCollection collection, InteractorState defaultState);

		bool OnHover (Transform target, Vector3 point);

		bool OnClick (Transform target, Vector3 point);

	}

	public abstract class BaseMapCollectionInteractor<TCollection> : IMapCollectionInteractor where TCollection : class, IMapCollection
	{

		protected Scribe Scribe = Scribes.Find ("INTERACTORS");

		protected TCollection Collection { get; private set; }

		MapInteractor mapInteractor;

		public void Setup (IMapCollection collection, InteractorState defaultState)
		{

			Scribe.LogFormat ("Interactor {0} start working with  collection {1}", this.GetType (), collection.Name);
			this.Collection = collection as TCollection;
			if (this.Collection == null)
			{
				Scribe.LogFormatError ("Interactor doesn't match collection provided: interactor type is {0} while layer {1}", this.GetType (), collection.GetType ());
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
	}
}

