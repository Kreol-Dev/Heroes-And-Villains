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
			ReadRepresenters ();
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

			public RepresenterHandle (IMapLayer layer, IMapLayerInteractor interactor, IMapLayerPresenter presenter, IMapLayerRenderer renderer, Type objectPresenterType, RepresenterState defaultState)
			{
				Presenter = presenter;
				Renderer = renderer;
				state = defaultState;
				Presenter.Setup (layer, interactor, objectPresenterType, state);
				Renderer.Setup (layer, interactor, state);
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

		void ReadRepresenters ()
		{
			var mm = Find.Root<ModsManager> ();
			ITable table = mm.GetTable ("representations");
			foreach (var repName in table.GetKeys())
			{
				try
				{
					ITable repTable = table.GetTable (repName);

					string rendererTypeName = repTable.GetString ("renderer");
					string presenterTypeName = repTable.GetString ("presenter");
					string objectPresenterTypeName = repTable.GetString ("objectPresenter");

					Type rendererType = mm.GetType (rendererTypeName);
					Type presenterType = mm.GetType (presenterTypeName);
					Type objectPresenterType = mm.GetType (objectPresenterTypeName);

					IMapLayerRenderer repRenderer = Activator.CreateInstance (rendererType) as IMapLayerRenderer;
					IMapLayerPresenter repPresenter = Activator.CreateInstance (presenterType) as IMapLayerPresenter;
					ITable layersTable = repTable.GetTable ("layers");

					RepresenterState state = (RepresenterState)Enum.Parse (typeof(RepresenterState), repTable.GetString ("default_state"));
					string interactorName = repTable.GetString ("interactor");
					var interactor = interactors.GetInteractor (interactorName);
					foreach (var layerID in layersTable.GetKeys())
					{
						string layerName = layersTable.GetString (layerID);
						var layer = map.GetLayer (layerName);
						RepresenterHandle handle = new RepresenterHandle (layer, interactor, repPresenter, repRenderer, objectPresenterType, state);

						representers.Add (repName as string, handle);
					}
				} catch (ITableTypesMismatch e)
				{
					continue;
				} catch (ITableMissingID e)
				{
					continue;
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


