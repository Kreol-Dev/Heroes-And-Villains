using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using System.Reflection;
using System;
using System.Linq;
using System.Text;

namespace MapRoot
{
	[RootDependencies (typeof(InputManager), typeof(MapRoot.Map), typeof(ModsManager))]
	public class MapInteractor : Root
	{
		Scribe scribe = Scribes.Find ("MAP INTERACTOR");

		public delegate void HitDelegate (Transform hitTransform, Vector3 hitPoint);


		MapRoot.Map map;
		InputManager manager;
		HitsGetter hitsGetter;
		HashSet<object> nonSelectables = new HashSet<object> ();
		HashSet<object> hoveredObjects = new HashSet<object> ();

		protected override void CustomSetup ()
		{
			manager = Find.Root<InputManager> ();
			manager.Hover += OnRawHover;
			manager.LeftClick += OnRawClick;
			manager.RightClick += OnRightClick;		
			map = Find.Root<MapRoot.Map> ();
			var layerNames = map.GetAllLayerNames ();
			ReadInteractors (layerNames);
			hitsGetter = new HitsGetter ((from bind in interactors
			                              select bind.Value.Interactor).Distinct ());
			Fulfill.Dispatch ();
		}

		protected override void PreSetup ()
		{
		}




		void OnRawHover (Vector2 screenPoint)
		{
			hoveredObjects.Clear ();
			hitsGetter.GetHits (screenPoint);
			for (int i = 0; i < hitsGetter.ObjectHitsCount; i++)
			{
				var realmHit = hitsGetter.RealmHits [i];
				realmHit.Interactor.OnHover (realmHit.Position, hitsGetter.AllegianceHits [realmHit.Interactor], ref hoveredObjects);
					
			}
			if (nonSelectables.Count > 0)
			{
				nonSelectables.IntersectWith (hoveredObjects);
			}
		}


		void OnRightClick (Vector2 point)
		{
			foreach (var interactor in interactors)
				interactor.Value.Interactor.OnDeselectAll ();
		}


		void OnRawClick (Vector2 screenPoint)
		{
			if (nonSelectables.Count == hoveredObjects.Count)
				nonSelectables.Clear ();
			else
				hoveredObjects.ExceptWith (nonSelectables);
			foreach (var interactor in interactors)
				interactor.Value.Interactor.OnDeselectAll ();
			for (int i = 0; i < hitsGetter.ObjectHitsCount; i++)
			{
				var realmHit = hitsGetter.RealmHits [i];
				object selectedObject = realmHit.Interactor.OnSelect (realmHit.Position, hoveredObjects);
				if (selectedObject == null)
					continue;
				nonSelectables.Add (selectedObject);
				break;
			}
		}

		class InteractorBinding
		{
			InteractorState state;
			static int id = 0;

			public InteractorState State {
				get { return state; }
				internal set{ state = value; }
			}

			public IMapLayerInteractor Interactor { get; internal set; }

			public InteractorBinding (IMapLayer layer, IMapLayerInteractor interactor, InteractorState defaultState)
			{
				Interactor = interactor;
				state = defaultState;
				interactor.Setup (layer, defaultState, id++);
			}
		}

		Dictionary<IMapLayer, InteractorBinding> interactors = new Dictionary<IMapLayer, InteractorBinding> ();
		Dictionary<string, InteractorBinding> interactorsByName = new Dictionary<string, InteractorBinding> ();

		public InteractorState GetLayerState (IMapLayer layer)
		{
			InteractorBinding interactor;
			if (interactors.TryGetValue (layer, out interactor))
				return interactor.State;
			else
			{
				scribe.LogFormatWarning ("Can't get state of a layer interactor {0} ({1}) - it isn't registered in a states dictionary", layer, layer.GetType ());
				return InteractorState.NotActive;
			}
		}

		public void SetState (IMapLayer layer, InteractorState state)
		{
			InteractorBinding binding = null;
			interactors.TryGetValue (layer, out binding);
			if (binding == null)
			{
				scribe.LogFormatWarning ("No such layer {0}. Interactor state won't be changed", layer.Name);
				return;
			}
			binding.State = state;
		}

		public IMapLayerInteractor GetInteractor (IMapLayer layer)
		{
			InteractorBinding interactor;
			if (interactors.TryGetValue (layer, out interactor))
				return interactor.Interactor;
			else
			{
				scribe.LogFormatWarning ("Can't get interactor for a layer {0} ({1}) - it isn't registered in interactors dictionary", layer.Name, layer.GetType ());
				return null;
			}
		}

		public IMapLayerInteractor GetInteractor (string name)
		{
			InteractorBinding interactor;
			if (interactorsByName.TryGetValue (name, out interactor))
				return interactor.Interactor;
			else
			{
				scribe.LogFormatWarning ("Can't get interactor {0}  - it isn't registered in interactors dictionary", name);
				return null;
			}
		}

		void ReadInteractors (string[] layerNames)
		{
			var mm = Find.Root<ModsManager> ();
			ITable table = mm.GetTable ("interactors");

			foreach (var interactorName in table.GetKeys())
			{
				try
				{
					ITable interactorTable = table.GetTable (interactorName);

					string typeName = interactorTable.GetString ("interactor_type");
					Type type = mm.GetType (typeName);
					IMapLayerInteractor interactor = Activator.CreateInstance (type) as IMapLayerInteractor;

					string defaultState = interactorTable.GetString ("default_state");
					InteractorState state = (InteractorState)Enum.Parse (typeof(InteractorState), defaultState);

					string targetLayerName = interactorTable.GetString ("layer");

					var layer = map.GetLayer (targetLayerName);

					InteractorBinding binding = new InteractorBinding (layer, interactor, state);
					interactors.Add (layer, binding);
					interactorsByName.Add (interactorName as string, binding);
					Debug.LogWarning ("Added interactor " + interactorName);
				} catch
				{
					continue;
				}
			}
		}

	}

	public enum InteractorState
	{
		NotActive,
		Active
	}
}
