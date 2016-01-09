using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace MapRoot
{
    public interface IMapLayerPresenter
    {
        void Setup (IMapLayer layer, IMapLayerInteractor interactor, IObjectPresenter objectPresenter, RepresenterState defaultState);

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

        public abstract void HideObjectDesc (TObject obj);

        public abstract void ShowObjectShortDesc (TObject obj);

        public abstract void HideObjectShortDesc (TObject obj);
    }

    public abstract class BaseMapLayerPresenter<TObject, TLayer, TInteractor> : IMapLayerPresenter
        where TLayer : class, IMapLayer where TInteractor : BaseMapLayerInteractor<TLayer>
    {
        Scribe scribe = Scribes.Find ("LAYER PRESENTER");

        protected ObjectPresenter<TObject> ObjectPresenter { get; private set; }

        protected TLayer Layer { get; private set; }

        protected TInteractor Interactor { get; private set; }

        public void Setup (IMapLayer layer, IMapLayerInteractor interactor, IObjectPresenter objectPresenter, RepresenterState defaultState)
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
            this.ObjectPresenter = objectPresenter as ObjectPresenter<TObject>; 
            if (this.ObjectPresenter == null)
            {
                scribe.LogFormatError ("Object presenter provided to presenter is of wrong type {0} while assumed {1}", objectPresenter.GetType (), typeof(ObjectPresenter<TObject>));
                return;
            }
            ITable table = Find.Root<ModsManager> ().GetTable ("defines");
            if (table == null)
                return;
            objectPresenter.Setup (table);
            ChangeState (defaultState);

        }

        public abstract void ChangeState (RepresenterState state);
    }

}

