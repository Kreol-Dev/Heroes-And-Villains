using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;

namespace CoreMod
{
	public abstract class TileMapLayerPresenter<TObject, TLayerObject, TLayer, TInteractor> : BaseMapLayerPresenter<TObject, TLayer, TInteractor>
		where TLayer : class, IMapLayer, ITileMapLayer<TLayerObject> where TInteractor : BaseMapLayerInteractor<TLayer>, ITileMapInteractor
	{
		HashSet<TileHandle> selectedTiles = new HashSet<TileHandle> ();
		HashSet<TileHandle> hoveredTiles = new HashSet<TileHandle> ();

		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				Interactor.TileSelected += Clicked;
				Interactor.TileDeselected += DeClicked;
				Interactor.TileHovered += Hovered;
				Interactor.TileDeHovered += DeHovered;
				Layer.MassUpdate.AddListener (ObjectPresenter.Update);
				Layer.TileUpdated.AddListener (TileUpdated);
				break;
			case RepresenterState.NotActive:
				Interactor.TileSelected -= Clicked;
				Interactor.TileDeselected -= DeClicked;
				Interactor.TileHovered -= Hovered;
				Interactor.TileDeHovered -= DeHovered;
				Layer.MassUpdate.RemoveListener (ObjectPresenter.Update);
				Layer.TileUpdated.RemoveListener (TileUpdated);
				break;
			}
		}

		public abstract TObject ObjectFromLayer (TLayerObject obj);

		void Clicked (TileHandle tile)
		{
			ObjectPresenter.ShowObjectDesc (ObjectFromLayer (tile.Get (Layer.Tiles)));
		}

		void DeClicked (TileHandle tile)
		{
			ObjectPresenter.HideObjectDesc ();
		}

		void Hovered (TileHandle tile)
		{
			ObjectPresenter.ShowObjectShortDesc (ObjectFromLayer (tile.Get (Layer.Tiles)));
		}

		void DeHovered (TileHandle tile)
		{
			ObjectPresenter.HideObjectShortDesc ();
		}

		void TileUpdated (TileHandle tile, TLayerObject obj)
		{
            
			ObjectPresenter.Update ();
		}

	}

}

