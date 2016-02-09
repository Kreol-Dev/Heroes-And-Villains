using UnityEngine;
using System.Collections;
using MapRoot;

namespace CoreMod
{
	public class TileMapLayerPresenter<TObject, TLayer> : ObjectLayerPresenter<TObject, TileHandle, TLayer, TileMapLayerInteractor>
		where TLayer : class, IMapLayer, ITileMapLayer<TObject>
	{
		public override void ChangeState (RepresenterState state)
		{
			base.ChangeState (state);
			switch (state)
			{
			case RepresenterState.Active:
				Layer.TileUpdated.AddListener (OnTileUpdated);
				break;
			case RepresenterState.NotActive:
				Layer.TileUpdated.RemoveListener (OnTileUpdated);
				break;
			}
		}

		public override TObject ObjectFromLayer (TileHandle obj)
		{
			return obj.Get (Layer.Tiles);
		}


		void OnTileUpdated (TileHandle handle)
		{
			TObject obj = handle.Get (Layer.Tiles);
			ObjectPresenter<TObject> oPresenter = null;
			hoverPresenters.TryGetValue (handle, out oPresenter);
			if (oPresenter != null)
			{
				if (obj == null)
					oPresenter.HideObjectShortDesc ();
				else
					oPresenter.ShowObjectShortDesc (obj);
			}
			oPresenter = null;
			selectionPresenters.TryGetValue (handle, out oPresenter);
			if (oPresenter != null)
			{
				if (obj == null)
					oPresenter.HideObjectDesc ();
				else
					oPresenter.ShowObjectDesc (obj);
			}
		}
	}

}

