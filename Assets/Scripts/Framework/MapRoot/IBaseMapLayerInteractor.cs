using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace MapRoot
{
    public interface IMapLayerInteractor
    {
        void Setup (IMapLayer layer, InteractorState defaultState);

        void ChangeState (InteractorState newState);
    }

    public abstract class BaseMapLayerInteractor<TLayer> : IMapLayerInteractor where TLayer : class, IMapLayer
    {
        protected Scribe Scribe = Scribes.Find ("INTERACTORS");
        TLayer layer;

        protected TLayer Layer { get { return layer; } }

        public void ChangeState (InteractorState newState)
        {
            if (layer == null)
                return;
            switch (newState)
            {
            case InteractorState.NotActive:

                mapInteractor.ObjectHover -= OnHover;
                mapInteractor.EndObjectHover -= OnEndHover;
                mapInteractor.ObjectClick -= OnClick;
                mapInteractor.EndObjectClick -= OnEndClick;
                mapInteractor.ObjectHighlight -= OnHighlight;
                mapInteractor.EndObjectHighlight -= OnEndHighlight;
                break;
            case InteractorState.Active:

                mapInteractor.ObjectHover += OnHover;
                mapInteractor.EndObjectHover += OnEndHover;
                mapInteractor.ObjectClick += OnClick;
                mapInteractor.EndObjectClick += OnEndClick;
                mapInteractor.ObjectHighlight += OnHighlight;
                mapInteractor.EndObjectHighlight += OnEndHighlight;
                break;
            }
        }

        MapInteractor mapInteractor;

        public void Setup (IMapLayer layer, InteractorState defaultState)
        {

            Scribe.LogFormat ("Interactor {0} start working with a layer {1}", this.GetType (), layer.Name);
            this.layer = layer as TLayer;
            if (this.layer == null)
            {
                Scribe.LogFormatError ("Interactor doesn't match layer provided: interactor type is {0} while layer {1}", this.GetType (), layer.GetType ());
                return;
            }
            mapInteractor = Find.Root<MapInteractor> ();
            ChangeState (defaultState);
            ITable table = Find.Root<ModsManager> ().GetTable ("defines");
            if (table != null)
                this.Setup (table);
        }

        protected abstract void Setup (ITable definesTable);

        protected abstract void OnHover (Transform obj, Vector3 point);

        protected abstract void OnEndHover (Transform obj, Vector3 point);

        protected abstract void OnClick (Transform obj, Vector3 point);

        protected abstract void OnEndClick (Transform obj, Vector3 point);

        protected abstract void OnHighlight (Transform obj, Vector3 point);

        protected abstract void OnEndHighlight (Transform obj, Vector3 point);

    }
}

