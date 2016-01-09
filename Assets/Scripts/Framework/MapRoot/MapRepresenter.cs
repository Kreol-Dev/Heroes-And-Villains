using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using System;

namespace MapRoot
{
    [RootDependencies (typeof(MapRoot.Map), typeof(MapRoot.MapInteractor))]
    public class MapRepresenter : Root
    {
        Scribe scribe = Scribes.Find ("MAP REPRESENTER");
        MapInteractor interactors;
        Map map;

        protected override void CustomSetup ()
        {
            interactors = Find.Root<MapInteractor> ();
            map = Find.Root<Map> ();
            var layerNames = map.GetAllLayerNames ();
            ReadRepresenters (layerNames);
            Fulfill.Dispatch ();
        }

        class RepresenterHandle
        {
            RepresenterState state;

            public RepresenterState State
            {
                get { return state; }
                internal set {
                    state = value;
                    Presenter.ChangeState (state);
                    //Renderer.ChangeState(state);
                }
            }

            public IMapLayerPresenter Presenter { get; internal set; }

            public IMapLayerRenderer Renderer { get; internal set; }

            public RepresenterHandle (IMapLayer layer, IMapLayerInteractor interactor, IMapLayerPresenter presenter, IMapLayerRenderer renderer, IObjectPresenter objectPresenter, RepresenterState defaultState)
            {
                Presenter = presenter;
                Renderer = renderer;
                state = defaultState;
                Presenter.Setup (layer, interactor, objectPresenter, state);
                //Renderer.Set();
            }
        }

        Dictionary<string, RepresenterHandle> representers = new Dictionary<string, RepresenterHandle> ();

        public RepresenterState GetRepresenterState (string name)
        {
            RepresenterHandle representer;
            if (representers.TryGetValue (name, out representer))
                return representer.State;
            else
            {
                scribe.LogFormatWarning ("Can't get state of a layer representer {0} - it isn't registered in a states dictionary", name);
                return RepresenterState.NotActive;
            }
        }

        public void SetRepresenterState (string name, RepresenterState state)
        {
            RepresenterHandle binding = null;
            representers.TryGetValue (name, out binding);
            if (binding == null)
            {
                scribe.LogFormatWarning ("No such representer {0}. Representer state won't be changed", name);
                return;
            }
            binding.State = state;
        }

        void ReadRepresenters (string[] layerNames)
        {
            ITable table = Find.Root<ModsManager> ().GetTable ("representations");
            foreach (var key in table.GetKeys())
                Debug.Log (key);
            foreach (var layerName in layerNames)
            {
                
                ITable representationsTable = table.Get (layerName + "_representations") as ITable;

                if (representationsTable == null)
                {
                    scribe.LogFormatWarning ("No representers for a layer {0} / {1}", layerName, layerName + "_representations");
                    continue;
                }
                foreach (var key in representationsTable.GetKeys())
                {
                    string representerName = layerName + "_" + key;

                    ITable representerTable = representationsTable.Get (key.ToString ()) as ITable;
                    string rendererName = (string)representerTable.Get (1);
                    string presenterName = (string)representerTable.Get (2);
                    string objectPresenterName = (string)representerTable.Get (3);

                    Type rendererType = Type.GetType (rendererName);
                    Type presenterType = Type.GetType (presenterName);
                    Type objectPresenterType = Type.GetType (objectPresenterName);
                    Debug.LogFormat ("{0} {1} {2}", rendererType, presenterType, objectPresenterType);
                    IMapLayerRenderer renderer = Activator.CreateInstance (rendererType) as IMapLayerRenderer;
                    IMapLayerPresenter presenter = Activator.CreateInstance (presenterType) as IMapLayerPresenter;
                    IObjectPresenter objectPresenter = Activator.CreateInstance (objectPresenterType) as IObjectPresenter;

                    string representerDefaultStateName = (string)representerTable.Get (4);
                    RepresenterState state = (RepresenterState)Enum.Parse (typeof(RepresenterState), representerDefaultStateName);
                    var layer = map.GetLayer (layerName);
                    var interactor = interactors.GetInteractor (layer);
                    if (interactor == null)
                    {
                        scribe.LogFormatError ("No interactor for a layer {0} could be found (representation {1})", layer.Name, representerName);
                        continue;
                    }
                    RepresenterHandle handle = new RepresenterHandle (layer, interactor, presenter, renderer, objectPresenter, state);

                    representers.Add (representerName, handle);
                }

            }
        }
    }

    public enum RepresenterState
    {
        NotActive,
        Active
    }
}


