using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using System.Reflection;
using System;

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




		protected override void CustomSetup ()
		{
			manager = Find.Root<InputManager> ();
			manager.Hover += OnRawHover;
			manager.LeftClick += OnRawClick;
			manager.WheelScroll += OnRawWheel;
			manager.RightClick += OnRightClick;
			hitsGetter = new HitsGetter (this);
			map = Find.Root<MapRoot.Map> ();
			var layerNames = map.GetAllLayerNames ();
			ReadInteractors (layerNames);
			Fulfill.Dispatch ();
		}

		protected override void PreSetup ()
		{
		}


		void Update ()
		{
			foreach (var interactor in interactors)
				interactor.Value.Interactor.OnUpdate ();
		}



		public int currentTargetID = 0;
		ObjectHit currentTarget;
		ObjectHit lastTarget;

		void SwitchCurrentTargetTo (ObjectHit hit, int currentObjectID)
		{
			if (currentTarget.Transform != null)
				lastTarget = currentTarget;
			currentTarget = hit;
			currentTargetID = currentObjectID;
		}


		void TrySwitchToTarget (ObjectHit target, int objectHitsCount, ObjectHit[] objectHits)
		{
			ObjectHit newTargetHit = new ObjectHit (null, Vector3.zero, null);
			int targetID = 0;
			for (int i = 0; i < objectHitsCount; i++)
			{

				if (objectHits [i].Interactor.OnHover (objectHits [i].Transform, objectHits [i].Position))
				{
					if (newTargetHit.Transform == null)
					{
						newTargetHit = objectHits [i];
						targetID = i;
					}

					if (newTargetHit.Interactor != target.Interactor && objectHits [i].Interactor == target.Interactor)
					{
						newTargetHit = objectHits [i];
						targetID = i;
					}

					if (objectHits [i].Transform == target.Transform && objectHits [i].Interactor == target.Interactor)
					{
						newTargetHit = objectHits [i];
						targetID = i;
					}
				}

					

			}
			SwitchCurrentTargetTo (newTargetHit, targetID);
		}

		void OnRawHover (Vector2 screenPoint)
		{
			ObjectHit[] objectHits = null;
			int objectHitsCount = hitsGetter.GetHits (screenPoint, out objectHits);
			if (objectHitsCount == 0)
			{
				SwitchCurrentTargetTo (new ObjectHit (null, Vector3.zero, null), -1);
				return;
			}

			if (currentTarget.Transform == null)
			{
				if (lastTarget.Transform != null)
				{
					TrySwitchToTarget (lastTarget, objectHitsCount, objectHits);
				} else
				{
					SwitchCurrentTargetTo (objectHits [0], 0);
				}
				return;
			}

			TrySwitchToTarget (currentTarget, objectHitsCount, objectHits);

		}


		void OnRawWheel (WheelScrollDir dir)
		{
			Debug.Log (dir);
			if (hitsGetter.ObjectHitsCount > 1)
			{
				switch (dir)
				{
				case WheelScrollDir.Up:
					currentTargetID++;
					currentTargetID %= hitsGetter.ObjectHitsCount;
					break;
				case WheelScrollDir.Down:
					currentTargetID--;
					if (currentTargetID < 0)
						currentTargetID = hitsGetter.ObjectHitsCount + currentTargetID;
					break;
				}
				SwitchCurrentTargetTo (hitsGetter.ObjectHits [currentTargetID], currentTargetID);
			}
				
		}

		void OnRightClick (Vector2 point)
		{
			if (currentTarget.Interactor != null && currentTarget.Transform != null)
				currentTarget.Interactor.OnAltClick (currentTarget.Transform, currentTarget.Position);
		}

		ObjectHit selectedObject;

		void OnRawClick (Vector2 screenPoint)
		{
			
			if (currentTarget.Interactor != null && currentTarget.Transform != null)
				currentTarget.Interactor.OnClick (currentTarget.Transform, currentTarget.Position);
		}

		class InteractorBinding
		{
			InteractorState state;

			public InteractorState State {
				get { return state; }
				internal set{ state = value; }
			}

			public IMapLayerInteractor Interactor { get; internal set; }

			public InteractorBinding (IMapLayer layer, IMapLayerInteractor interactor, InteractorState defaultState)
			{
				Interactor = interactor;
				state = defaultState;
				interactor.Setup (layer, defaultState);
			}
		}

		Dictionary<IMapLayer, InteractorBinding> interactors = new Dictionary<IMapLayer, InteractorBinding> ();

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
				scribe.LogFormatWarning ("Can't get interactor {0} ({1}) - it isn't registered in interactors dictionary", layer, layer.GetType ());
				return null;
			}
		}

		void ReadInteractors (string[] layerNames)
		{
			ITable table = Find.Root<ModsManager> ().GetTable ("interactors");
			foreach (var layerName in layerNames)
			{
				string interactorName = layerName + "_interactor";
				ITable interactorTable = table.Get (interactorName) as ITable;
				if (interactorTable == null)
				{
					scribe.LogFormatWarning ("Can't find table named {0}", interactorName);
					continue;
				}
				string interactorTypeName = (string)interactorTable.Get (1);
				Type interactorType = Type.GetType (interactorTypeName);
				IMapLayerInteractor interactor = Activator.CreateInstance (interactorType) as IMapLayerInteractor;
				if (interactor == null)
				{
					scribe.LogFormatWarning ("Interactor with the name {0} and type {1} doesn't inherit IMapLayerInteractor, while it should", interactorName, interactorTypeName);
					continue;
				}
				string interactorDefaultState = (string)interactorTable.Get (2);
				InteractorState state = (InteractorState)Enum.Parse (typeof(InteractorState), interactorDefaultState);
				var layer = map.GetLayer (layerName);
				InteractorBinding binding = new InteractorBinding (layer, interactor, state);
				interactors.Add (layer, binding);

			}
		}

	}

	public enum InteractorState
	{
		NotActive,
		Active
	}
}
