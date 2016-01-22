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

		public event HitDelegate ObjectHover;
		public event HitDelegate EndObjectHover;
		public event HitDelegate ObjectHighlight;
		public event HitDelegate EndObjectHighlight;
		public event HitDelegate ObjectClick;
		public event HitDelegate EndObjectClick;

		MapRoot.Map map;
		InputManager manager;
		HitsGetter hitsGetter;
		ObjectHit currentHover;
		ObjectHit lastHover;

		int hoverObjectID = 0;

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
			ObjectHover += (obj, pos) => {
			};
			EndObjectHover += (obj, pos) => {
			};
			ObjectHighlight += (obj, pos) => {
			};
			EndObjectHighlight += (obj, pos) => {
			};
			ObjectClick += (obj, pos) => {
			};
			EndObjectClick += (obj, pos) => {
			};
		}




		public struct ObjectHit
		{
			public Transform Transform { get; internal set; }

			public Vector3 Position { get; internal set; }

			public ObjectHit (Transform transform, Vector3 position)
			{
				Transform = transform;
				Position = position;
			}
		}

		public class HitsGetter
		{
			MapInteractor interactor;

			public HitsGetter (MapInteractor interactor)
			{
				this.interactor = interactor;
			}

			RaycastHit[] hits = new RaycastHit[10];
			ObjectHit[] objectHits = new ObjectHit[10];

			public ObjectHit[] ObjectHits { get { return objectHits; } }

			public int ObjectHitsCount { get { return objectHitsCount; } }

			int objectHitsCount = 0;

			public int GetHits (Vector2 screenPoint, out ObjectHit[] providedHits)
			{
				objectHitsCount = 0;
				Ray ray = Camera.main.ScreenPointToRay (screenPoint);
				int collidersCount = Physics.RaycastNonAlloc (ray, hits, 30);
				if (collidersCount >= hits.Length) {
					hits = new RaycastHit[collidersCount + 10];
					objectHits = new ObjectHit[collidersCount + 10];
					collidersCount = Physics.RaycastNonAlloc (ray, hits, 30);
				}
				for (int i = 0; i < collidersCount; i++) {
					LayerHandle handle = hits [i].transform.gameObject.GetComponent<LayerHandle> ();
					if (handle == null)
						continue;
					if (interactor.GetLayerState (handle.Layer) == InteractorState.Active) {
                        
						objectHits [objectHitsCount++] = new ObjectHit (hits [i].transform, hits [i].point);
					}
				}
				providedHits = objectHits;
				return objectHitsCount;
			}
		}


		void OnRawHover (Vector2 screenPoint)
		{
			ObjectHit[] objectHits = null;
			int objectHitsCount = hitsGetter.GetHits (screenPoint, out objectHits);


			int hoverObjectID = 0;
			if (objectHitsCount == 0) {
				if (currentHover.Transform == null)
					return;
				EndObjectHover (currentHover.Transform, currentHover.Position);
				lastHover = currentHover;
				currentHover.Transform = null;
				return;
			}

			if (lastHover.Transform == null) {
				Hover (objectHits [0].Transform, objectHits [0].Position);
				this.hoverObjectID = 0;
				lastHover = currentHover;
				return;
			}


			ObjectHit tempHit = new ObjectHit () {
				Transform = null,
				Position = Vector3.zero
			};
			for (int i = 0; i < objectHitsCount; i++) {
				if (tempHit.Transform == null) {
					tempHit = objectHits [i];
					hoverObjectID = i;
				}
				if (objectHits [i].Transform == currentHover.Transform) {

					this.hoverObjectID = i;
					currentHover.Position = objectHits [i].Position;
					Hover (currentHover.Transform, currentHover.Position);
					return;
				}
			}
			currentHover = tempHit;
			this.hoverObjectID = hoverObjectID;
			if (currentHover.Transform != null)
				Hover (currentHover.Transform, currentHover.Position);

		}

		void Hover (Transform transform, Vector3 point)
		{
			if (currentHover.Transform != null) {
				lastHover = currentHover;
				EndObjectHover (lastHover.Transform, lastHover.Position);
			}
			currentHover.Transform = transform;
			currentHover.Position = point;
			ObjectHover (currentHover.Transform, currentHover.Position);

		}

		void OnRawWheel (WheelScrollDir dir)
		{
			if (hitsGetter.ObjectHitsCount > 1)
				switch (dir) {
				case WheelScrollDir.Up:
					hoverObjectID++;
					hoverObjectID %= hitsGetter.ObjectHitsCount;
					Hover (hitsGetter.ObjectHits [hoverObjectID].Transform, hitsGetter.ObjectHits [hoverObjectID].Position);
					break;
				case WheelScrollDir.Down:
					hoverObjectID--;
					if (hoverObjectID < 0)
						hoverObjectID = hitsGetter.ObjectHitsCount - hoverObjectID;
					Hover (hitsGetter.ObjectHits [hoverObjectID].Transform, hitsGetter.ObjectHits [hoverObjectID].Position);
					break;
				}
		}

		void OnRightClick (Vector2 point)
		{
			if (selectedObject.Transform != null) {

				EndObjectClick (selectedObject.Transform, selectedObject.Position);
				selectedObject.Transform = null;
			}
		}

		ObjectHit selectedObject;

		void OnRawClick (Vector2 screenPoint)
		{
			Debug.LogFormat ("MAP INTERACTOR CLICK {0}", screenPoint);
			if (currentHover.Transform == null) {
				if (selectedObject.Transform != null)
					EndObjectClick (selectedObject.Transform, selectedObject.Position);
				return;
			}

			if (selectedObject.Transform != null)
				EndObjectClick (selectedObject.Transform, selectedObject.Position);
			selectedObject = currentHover;
			ObjectClick (selectedObject.Transform, selectedObject.Position);
		}

		class InteractorBinding
		{
			InteractorState state;

			public InteractorState State {
				get { return state; }
				internal set {
					state = value;
					Interactor.ChangeState (state);
				}
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
			else {
				scribe.LogFormatWarning ("Can't get state of a layer interactor {0} ({1}) - it isn't registered in a states dictionary", layer, layer.GetType ());
				return InteractorState.NotActive;
			}
		}

		public void SetState (IMapLayer layer, InteractorState state)
		{
			InteractorBinding binding = null;
			interactors.TryGetValue (layer, out binding);
			if (binding == null) {
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
			else {
				scribe.LogFormatWarning ("Can't get interactor {0} ({1}) - it isn't registered in interactors dictionary", layer, layer.GetType ());
				return null;
			}
		}

		void ReadInteractors (string[] layerNames)
		{
			ITable table = Find.Root<ModsManager> ().GetTable ("interactors");
			foreach (var layerName in layerNames) {
				string interactorName = layerName + "_interactor";
				ITable interactorTable = table.Get (interactorName) as ITable;
				if (interactorTable == null) {
					scribe.LogFormatWarning ("Can't find table named {0}", interactorName);
					continue;
				}
				string interactorTypeName = (string)interactorTable.Get (1);
				Type interactorType = Type.GetType (interactorTypeName);
				IMapLayerInteractor interactor = Activator.CreateInstance (interactorType) as IMapLayerInteractor;
				if (interactor == null) {
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

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.yellow;
			for (int i = 0; i < hitsGetter.ObjectHitsCount; i++) {
                
				Gizmos.DrawSphere (hitsGetter.ObjectHits [i].Position, 0.2f);
			}

			Gizmos.color = Color.red;
			if (selectedObject.Transform != null)
				Gizmos.DrawSphere (selectedObject.Position, 0.3f);
			Gizmos.color = Color.blue;
			if (currentHover.Transform != null)
				Gizmos.DrawSphere (currentHover.Position, 0.35f);
		}
	}

	public enum InteractorState
	{
		NotActive,
		Active
	}
}
