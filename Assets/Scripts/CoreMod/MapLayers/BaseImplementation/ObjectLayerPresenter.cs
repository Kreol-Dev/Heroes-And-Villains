using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;

namespace CoreMod
{
	public abstract class ObjectLayerPresenter<TObject, TLayerObject, TLayer, TInteractor> : BaseMapLayerPresenter<TObject, TLayer, TInteractor>
		where TLayer : class, IMapLayer, ITileMapLayer<TLayerObject> where TInteractor : BaseMapLayerInteractor<TLayer>, IObjectsInteractor<TLayerObject, TLayer>
	{
		HashSet<TileHandle> selectedTiles = new HashSet<TileHandle> ();
		HashSet<TileHandle> hoveredTiles = new HashSet<TileHandle> ();

		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				Interactor.ObjectSelected += Clicked;
				Interactor.ObjectDeSelected += DeClicked;
				Interactor.ObjectHovered += Hovered;
				Interactor.ObjectDeHovered += DeHovered;
				Layer.MassUpdate.AddListener (ObjectPresenter.Update);
				Layer.TileUpdated.AddListener (TileUpdated);
				break;
			case RepresenterState.NotActive:
				Interactor.ObjectSelected -= Clicked;
				Interactor.ObjectDeSelected -= DeClicked;
				Interactor.ObjectHovered -= Hovered;
				Interactor.ObjectDeHovered -= DeHovered;
				Layer.MassUpdate.RemoveListener (ObjectPresenter.Update);
				Layer.TileUpdated.RemoveListener (TileUpdated);
				break;
			}
		}

		public abstract TObject ObjectFromLayer (TLayerObject obj);

		void Clicked (TLayerObject tile)
		{
			ObjectPresenter.ShowObjectDesc (ObjectFromLayer (tile));
		}

		void DeClicked (TLayerObject tile)
		{
			ObjectPresenter.HideObjectDesc ();
		}

		void Hovered (TLayerObject tile)
		{
			ObjectPresenter.ShowObjectShortDesc (ObjectFromLayer (tile));
		}

		void DeHovered (TLayerObject tile)
		{
			ObjectPresenter.HideObjectShortDesc ();
		}

		void TileUpdated (TileHandle tile, TLayerObject obj)
		{

			ObjectPresenter.Update ();
		}

	}

}
