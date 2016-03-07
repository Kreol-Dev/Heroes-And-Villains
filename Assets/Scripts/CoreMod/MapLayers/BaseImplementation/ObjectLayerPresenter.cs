using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;

namespace CoreMod
{
	public abstract class ObjectLayerPresenter<TObject, TInteractorObject, TLayer, TInteractor> : BaseMapLayerPresenter<TObject, TLayer, TInteractor>
		where TLayer : /*class, IMapLayer,*/ ITileMapLayer<TObject> where TInteractor : /*class, IMapLayerInteractor,*/ IObjectsInteractor<TInteractorObject> where TInteractorObject : class
	{
		HashSet<TileHandle> selectedTiles = new HashSet<TileHandle> ();
		HashSet<TileHandle> hoveredTiles = new HashSet<TileHandle> ();

		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				Interactor.ObjectSelected += Selected;
				Interactor.ObjectDeSelected += DeSelected;
				Interactor.ObjectHovered += Hovered;
				Interactor.ObjectDeHovered += DeHovered;
				break;
			case RepresenterState.NotActive:
				Interactor.ObjectSelected -= Selected;
				Interactor.ObjectDeSelected -= DeSelected;
				Interactor.ObjectHovered -= Hovered;
				Interactor.ObjectDeHovered -= DeHovered;
				break;
			}
		}

		public abstract TObject ObjectFromLayer (TInteractorObject obj);

		protected Dictionary<TInteractorObject, ObjectPresenter<TObject>> hoverPresenters = new Dictionary<TInteractorObject, ObjectPresenter<TObject>> ();
		protected Dictionary<TInteractorObject, ObjectPresenter<TObject>> selectionPresenters = new Dictionary<TInteractorObject, ObjectPresenter<TObject>> ();

		void Selected (TInteractorObject obj)
		{ 
			if (selectionPresenters.ContainsKey (obj))
				return;
			ObjectPresenter<TObject> objectPresenter = BorrowPresenter ();
			objectPresenter.ShowObjectDesc (ObjectFromLayer (obj));
			selectionPresenters.Add (obj, objectPresenter);
		}

		void DeSelected (TInteractorObject obj)
		{ 
			ObjectPresenter<TObject> objectPresenter = null;
			selectionPresenters.TryGetValue (obj, out objectPresenter);
			if (objectPresenter == null)
				return;
			objectPresenter.HideObjectDesc ();
			selectionPresenters.Remove (obj);
			FreePresenter (objectPresenter);
		}

		void Hovered (TInteractorObject obj)
		{ 
			if (hoverPresenters.ContainsKey (obj))
				return;
			ObjectPresenter<TObject> objectPresenter = BorrowPresenter ();
			objectPresenter.ShowObjectShortDesc (ObjectFromLayer (obj));
			hoverPresenters.Add (obj, objectPresenter);
		}

		void DeHovered (TInteractorObject obj)
		{ 
			ObjectPresenter<TObject> objectPresenter = null;
			hoverPresenters.TryGetValue (obj, out objectPresenter);
			if (objectPresenter == null)
				return;
			objectPresenter.HideObjectShortDesc ();
			hoverPresenters.Remove (obj);
			FreePresenter (objectPresenter);
		}



	}

}
