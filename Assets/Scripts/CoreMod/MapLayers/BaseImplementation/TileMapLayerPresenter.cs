using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;

namespace CoreMod
{
	public abstract class TileMapLayerPresenter<TObject, TLayerObject, TLayer, TInteractor, TCollection> : BaseMapLayerPresenter<TObject, TLayer, TInteractor>
		where TLayer : MapLayer<TCollection>, ITileMapLayer<TLayerObject> where TInteractor : BaseMapCollectionInteractor<TCollection>, ITileMapInteractor
		where TCollection : class, IMapCollection
	{
		

		Dictionary<TileHandle, ObjectPresenter<TObject>> hovered = new Dictionary<TileHandle, ObjectPresenter<TObject>> ();
		Dictionary<TileHandle, ObjectPresenter<TObject>> selected = new Dictionary<TileHandle, ObjectPresenter<TObject>> ();
		Stack<ObjectPresenter<TObject>> freePresenters = new Stack<ObjectPresenter<TObject>> ();

		ObjectPresenter<TObject> GetFreePresenter ()
		{
			ObjectPresenter<TObject> presenter;
			if (freePresenters.Count > 0)
				presenter = freePresenters.Pop ();
			else
				presenter = NewPresenter ();


			presenter.HideObjectDesc ();
			presenter.HideObjectShortDesc ();
			return presenter;
		}

		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				Interactor.TileSelected += OnSelect;
				Interactor.TileDeselected += OnDeSelect;
				Interactor.TileHovered += OnHover;
				Interactor.TileDeHovered += OnDeHover;
				Layer.MassUpdate.AddListener (MassUpdate);
				Layer.TileUpdated.AddListener (OnObjectChanged);
				break;
			case RepresenterState.NotActive:
				Interactor.TileSelected -= OnSelect;
				Interactor.TileDeselected -= OnDeSelect;
				Interactor.TileHovered -= OnHover;
				Interactor.TileDeHovered -= OnDeHover;
				Layer.MassUpdate.RemoveListener (MassUpdate);
				Layer.TileUpdated.RemoveListener (OnObjectChanged);
				break;
			}
		}

		protected abstract TObject GetObjectFromLayer (TLayerObject obj);

		void OnHover (TileHandle handle)
		{
			if (hovered.ContainsKey (handle))
				return;
			TLayerObject layerObj = handle.Get (Layer.Tiles);
			if (layerObj == null)
				return;
			TObject obj = GetObjectFromLayer (layerObj);

			ObjectPresenter<TObject> presenter = GetFreePresenter ();
			presenter.ShowObjectShortDesc (obj);
			hovered.Add (handle, presenter);
		}


		void OnDeHover (TileHandle handle)
		{
			if (!hovered.ContainsKey (handle))
				return;

			ObjectPresenter<TObject> presenter = GetFreePresenter ();
			presenter.HideObjectShortDesc ();
			hovered.Remove (handle);

		}

		void OnSelect (TileHandle handle)
		{
			if (selected.ContainsKey (handle))
				return;
			TLayerObject layerObj = handle.Get (Layer.Tiles);
			if (layerObj == null)
				return;
			TObject obj = GetObjectFromLayer (layerObj);

			ObjectPresenter<TObject> presenter = GetFreePresenter ();
			presenter.ShowObjectDesc (obj);
			selected.Add (handle, presenter);
		}

		void OnDeSelect (TileHandle handle)
		{
			if (!selected.ContainsKey (handle))
				return;

			ObjectPresenter<TObject> presenter = GetFreePresenter ();
			presenter.HideObjectShortDesc ();
			selected.Remove (handle);
		}

		void OnObjectChanged (TileHandle handle, TLayerObject obj)
		{
			if (selected.ContainsKey (handle))
			{
				selected [handle].Update ();
			}

			if (hovered.ContainsKey (handle))
			{
				hovered [handle].Update ();
			}
		}

		void MassUpdate ()
		{
			foreach (var hover in hovered)
				hover.Value.Update ();
			foreach (var select in selected)
				select.Value.Update ();
		}


	}

}

