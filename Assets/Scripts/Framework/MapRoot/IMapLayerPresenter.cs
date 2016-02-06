using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System;

namespace MapRoot
{
	public interface IMapLayerPresenter
	{
		void Setup (IMapLayer layer, IMapCollectionInteractor interactor, Type objectPresenter, RepresenterState defaultState);

		void ChangeState (RepresenterState state);

	}

	public interface IObjectPresenter
	{
		void Setup (ITable definesTable);

		void Update ();

	}

	public abstract class ObjectPresenter<TObject> : IObjectPresenter
	{
		public abstract void Setup (ITable definesTable);

		public abstract void Update ();

		public abstract void ShowObjectDesc (TObject obj);

		public abstract void HideObjectDesc ();

		public abstract void ShowObjectShortDesc (TObject obj);

		public abstract void HideObjectShortDesc ();
	}

	public abstract class BaseMapLayerPresenter<TObject, TLayer, TInteractor> : IMapLayerPresenter
		where TLayer : class, IMapLayer where TInteractor : class, IMapCollectionInteractor
	{
		Scribe scribe = Scribes.Find ("LAYER PRESENTER");

		Type objectPresenter;

		protected TLayer Layer { get; private set; }

		protected TInteractor Interactor { get; private set; }

		ITable definesTable = Find.Root<ModsManager> ().GetTable ("defines");

		protected ObjectPresenter<TObject> NewPresenter ()
		{
			ObjectPresenter<TObject> presenter = Activator.CreateInstance (objectPresenter) as ObjectPresenter<TObject>;
			presenter.Setup (definesTable);
			return presenter;
		}

		public void Setup (IMapLayer layer, IMapCollectionInteractor interactor, Type objectPresenter, RepresenterState defaultState)
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
			this.objectPresenter = objectPresenter;
			if (!typeof(ObjectPresenter<TObject>).IsAssignableFrom (objectPresenter))
			{
				scribe.LogFormatError ("Object presenter provided to presenter is of wrong type {0} while assumed {1}", objectPresenter, typeof(ObjectPresenter<TObject>));
				return;
			}
			ChangeState (defaultState);

		}

		public abstract void ChangeState (RepresenterState state);
	}

}

