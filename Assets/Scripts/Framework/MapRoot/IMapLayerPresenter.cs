using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System;
using System.Collections.Generic;

namespace MapRoot
{
	public interface IMapLayerPresenter
	{
		void Setup (IMapLayer layer, IMapLayerInteractor interactor, Type objectPresenterType, RepresenterState defaultState);

		void ChangeState (RepresenterState state);

	}

	public interface IObjectPresenter
	{
		void Setup (ITable definesTable);
	}

	public abstract class ObjectPresenter<TObject> : IObjectPresenter
	{
		public abstract void Setup (ITable definesTable);

		public abstract void ShowObjectDesc (TObject obj);

		public abstract void HideObjectDesc ();

		public abstract void ShowObjectShortDesc (TObject obj);

		public abstract void HideObjectShortDesc ();
	}

	public abstract class BaseMapLayerPresenter<TObject, TLayer, TInteractor> : IMapLayerPresenter
		where TLayer : class, IMapLayer where TInteractor : class, IMapLayerInteractor
	{
		Scribe scribe = Scribes.Find ("LAYER PRESENTER");

		Type objectPresenterType;

		protected TLayer Layer { get; private set; }

		protected TInteractor Interactor { get; private set; }

		ITable defines;

		Stack<ObjectPresenter<TObject>> freePresenters = new Stack<ObjectPresenter<TObject>> ();

		protected ObjectPresenter<TObject> BorrowPresenter ()
		{
			ObjectPresenter<TObject> objectPresenter = null; 
			if (freePresenters.Count == 0)
			{
				objectPresenter = Activator.CreateInstance (objectPresenterType) as ObjectPresenter<TObject>;
				objectPresenter.Setup (defines);
			} else
				objectPresenter = freePresenters.Pop ();
			return objectPresenter;

		}

		protected void FreePresenter (ObjectPresenter<TObject> presenter)
		{
			freePresenters.Push (presenter);
			presenter.HideObjectDesc ();
			presenter.HideObjectShortDesc ();
		}

		public void Setup (IMapLayer layer, IMapLayerInteractor interactor, Type objectPresenterType, RepresenterState defaultState)
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
			if (!objectPresenterType.IsSubclassOf (typeof(ObjectPresenter<TObject>)))
			{
				scribe.LogFormatError ("Object presenter provided to presenter is of wrong type {0} while assumed {1}", objectPresenterType, typeof(ObjectPresenter<TObject>));
				return;
			}
			this.objectPresenterType = objectPresenterType;
			defines = Find.Root<ModsManager> ().GetTable ("defines");
			ChangeState (defaultState);

		}

		public abstract void ChangeState (RepresenterState state);
	}


	public class NullLayerPresenter : IMapLayerPresenter
	{
		public void Setup (IMapLayer layer, IMapLayerInteractor interactor, Type objectPresenterType, RepresenterState defaultState)
		{
		}

		public void ChangeState (RepresenterState state)
		{
		}
	}

	public class NullObjectPresenter : ObjectPresenter<object>
	{
		public override void Setup (ITable definesTable)
		{
		}

		public override void ShowObjectDesc (object obj)
		{
		}

		public override void HideObjectDesc ()
		{
		}

		public override void ShowObjectShortDesc (object obj)
		{
		}

		public override void HideObjectShortDesc ()
		{
		}
		
	}
}

