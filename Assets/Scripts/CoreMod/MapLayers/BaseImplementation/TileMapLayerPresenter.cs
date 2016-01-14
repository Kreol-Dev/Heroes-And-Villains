using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;

namespace CoreMod
{
    public class TileMapLayerPresenter<TObject, TLayerObject, TLayer, TInteractor> : BaseMapLayerPresenter<TObject, TLayer, TInteractor>
        where TLayer : class, IMapLayer, ITileMapLayer<TLayerObject> where TInteractor : BaseMapLayerInteractor<TLayer>, ITileMapInteractor<TObject, TLayerObject, TLayer>
    {
        HashSet<TileHandle> selectedTiles = new HashSet<TileHandle> ();
        HashSet<TileHandle> hoveredTiles = new HashSet<TileHandle> ();

        public override void ChangeState (RepresenterState state)
        {
            switch (state)
            {
            case RepresenterState.Active:
                Interactor.TileClicked += Clicked;
                Interactor.TileDeClicked += DeClicked;
                Interactor.TileHovered += Hovered;
                Interactor.TileDeHovered += DeHovered;
                Layer.MassUpdate.AddListener (ObjectPresenter.Update);
                Layer.TileUpdated.AddListener (TileUpdated);
                break;
            case RepresenterState.NotActive:
                Interactor.TileClicked -= Clicked;
                Interactor.TileDeClicked -= DeClicked;
                Interactor.TileHovered -= Hovered;
                Interactor.TileDeHovered -= DeHovered;
                Layer.MassUpdate.RemoveListener (ObjectPresenter.Update);
                Layer.TileUpdated.RemoveListener (TileUpdated);
                break;
            }
        }

        void Clicked (TileHandle tile, TObject obj)
        {
            if (selectedTiles.Add (tile))
                ObjectPresenter.ShowObjectDesc (obj);
        }

        void DeClicked (TileHandle tile, TObject obj)
        {
            if (selectedTiles.Remove (tile))
                ObjectPresenter.HideObjectDesc (obj);
        }

        void Hovered (TileHandle tile, TObject obj)
        {
            if (hoveredTiles.Add (tile))
                ObjectPresenter.ShowObjectShortDesc (obj);
        }

        void DeHovered (TileHandle tile, TObject obj)
        {
            if (hoveredTiles.Remove (tile))
                ObjectPresenter.HideObjectShortDesc (obj);
        }

        void TileUpdated (TileHandle tile, TLayerObject obj)
        {
            
            if (selectedTiles.Contains (tile) || hoveredTiles.Contains (tile))
                ObjectPresenter.Update ();
        }

    }

}

