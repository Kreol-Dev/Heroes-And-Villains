using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using System;
using System.Linq;

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
			ReadRepresenters (map.GetAllCollections ());
			Fulfill.Dispatch ();
		}

		class RepresenterHandle
		{
			RepresenterState state;

			public RepresenterState State {
				get { return state; }
				internal set
				{
					state = value;
					Presenter.ChangeState (state);
					Renderer.ChangeState (state);
				}
			}

			public IMapLayerPresenter Presenter { get; internal set; }

			public IMapLayerRenderer Renderer { get; internal set; }

			public RepresenterHandle (IMapLayer layer, IMapCollectionInteractor interactor, IMapLayerPresenter presenter, IMapLayerRenderer renderer, Type objectPresenter, RepresenterState defaultState, IMapCollection collection)
			{
				Presenter = presenter;
				Renderer = renderer;
				state = defaultState;
				Presenter.Setup (layer, interactor, objectPresenter, state);
				Renderer.Setup (layer, collection, state);
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

		void ReadRepresenters (List<IMapCollection> collections)
		{
			ITable table = Find.Root<ModsManager> ().GetTable ("representations");
			foreach (var collection in collections)
			{
				ITable collectionRepTable = table.GetTable (collection.Name + "_representation") as ITable;
				if (collectionRepTable == null)
				{
					scribe.LogFormatWarning ("No representers for a layer {0} / {1}", collection, collection + "_representation");
					continue;
				}

				var interactor = interactors.GetInteractor (collection);

				if (interactor == null)
				{
					scribe.LogFormatError ("No interactor for a collection {0} could be found ({0}_representer)", collection.Name);
					continue;
				}
				foreach (var layerName in collectionRepTable.GetKeys())
				{
					ITable representationsTable = collectionRepTable.GetTable (layerName);

					foreach (var layerRepKey in representationsTable.GetKeys())
					{
						ITable layerRepTable = representationsTable.GetTable (layerRepKey);
						string rendererName = layerRepTable.GetString (1);
						string presenterName = layerRepTable.GetString (2);
						string objectPresenterName = layerRepTable.GetString (3);

						Type rendererType = Type.GetType (rendererName);
						Type presenterType = Type.GetType (presenterName);
						Type objectPresenterType = Type.GetType (objectPresenterName);

						string representerDefaultStateName = (string)layerRepTable.GetString (4);
						RepresenterState state = (RepresenterState)Enum.Parse (typeof(RepresenterState), representerDefaultStateName);


						var layer = collection.GetLayer (layerName as string);
						IMapLayerRenderer renderer = Activator.CreateInstance (rendererType) as IMapLayerRenderer;
						IMapLayerPresenter presenter = Activator.CreateInstance (presenterType) as IMapLayerPresenter;


						RepresenterHandle handle = new RepresenterHandle (layer, interactor, presenter, renderer, objectPresenterType, state, collection);

						representers.Add (collection.Name + "_" + layerRepKey, handle);
					}


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


