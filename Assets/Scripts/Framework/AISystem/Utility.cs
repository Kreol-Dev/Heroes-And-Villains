using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace UAI
{
	[RootDependencies (typeof(ModsManager))]
	public class AIRoot : Root
	{
		Dictionary<Type, UtilitySystem> systems = new Dictionary<Type, UtilitySystem> ();

		protected override void CustomSetup ()
		{
			var mm = Find.Root<ModsManager> ();
			var allTypes = mm.GetAllTypes ();
			var usTypes = from type in allTypes
			              where type.IsSubclassOf (typeof(UtilitySystem))
			              select type;
			foreach (var usType in usTypes)
			{
				var system = Activator.CreateInstance (usType) as UtilitySystem;
				systems.Add (usType, system);
			}
			var aTypes = from type in allTypes
			             where type.IsSubclassOf (typeof(Action))
			             select type;
			foreach (var usType in usTypes)
			{
				var field = usType.GetField ("allRelevantTypes", BindingFlags.NonPublic | BindingFlags.Static);
				if (field == null)
					continue;
				var relType = usType.GetField ("relevantType", BindingFlags.NonPublic | BindingFlags.Static);
				var relATypes = from type in aTypes
				                where relType.FieldType.IsAssignableFrom (type)
				                select type;
				field.SetValue (null, new List<Type> (relATypes));
			}
			Fulfill.Dispatch ();
		}

		public List<UtilitySystem> GetSystemsForPrefab (GameObject go)
		{
			List<UtilitySystem> list = new List<UtilitySystem> ();
			foreach (var system in systems)
			{
				var aTypes = system.Value.GetRelevantActions (go);
				if (aTypes == null || aTypes.Count == 0)
					continue;
				list.Add (Activator.CreateInstance (system.Key) as UtilitySystem);
				list [list.Count - 1].ReceiveRelevantActions (aTypes);
			}
			return list;
		}


	}

	public class Agent : EntityComponent
	{
		Action performedAction = null;
		List<UtilitySystem> systems;
		Utilities utilities;
		int planningIteration = 0;
		static float iterationUtilityMultiplier = 0.9f;
		static float conditionalUtilityMultiplier = 0.7f;

		public override void OnInitPrefab ()
		{
			systems = Find.Root<AIRoot> ().GetSystemsForPrefab (gameObject);
		}

		public override EntityComponent CopyTo (GameObject go)
		{
			var agent = go.AddComponent<Agent> ();
			return agent;
		}

		void Awake ()
		{
			utilities = gameObject.AddComponent<Utilities> ();
		}

		public override void PostCreate ()
		{
			Find.Root<AI.Ticker> ().Tick += OnTick;
		}

		protected override void PostDestroy ()
		{
			Find.Root<AI.Ticker> ().Tick -= OnTick;
		}

		void OnTick ()
		{
			if (performedAction == null)
			{
				Action maxUtAction = null;
				float maxUt = float.MinValue;
				Action maxUtPossibleAction = null;
				bool anyUpdates = false;
				for (int i = 0; i < systems.Count; i++)
				{
					var system = systems [i];
					anyUpdates = system.PrepareActions (utilities, planningIteration, conditionalUtilityMultiplier, iterationUtilityMultiplier) || anyUpdates;

					foreach (var action in system)
					{
						if (action.Utility > maxUt)
						{
							maxUtAction = action;
							maxUt = action.Utility;
							if (action.IsPossible ())
								maxUtPossibleAction = action;
						}
					}
				}

				if (maxUtAction != null && maxUtAction == maxUtPossibleAction)
				{
					maxUtAction.Perform ();
					for (int i = 0; i < systems.Count; i++)
						systems [i].ClearActions (utilities, maxUtAction);
					planningIteration = 0;
				} else if (!anyUpdates)
				{
					if (maxUtPossibleAction != null)
					{
						maxUtPossibleAction.Perform ();
						for (int i = 0; i < systems.Count; i++)
							systems [i].ClearActions (utilities, maxUtAction);
					}
					planningIteration = 0;
				}
			} else
				performedAction.Tick ();


		}

		void Update ()
		{
			performedAction.Update ();
		}



	}

	public abstract class Action
	{
		public float Utility { get; set; }

		public bool Approved { get; set; }

		public float GetUtility (Utilities uts, float conditionalUtilityMultiplier, float iterationMultiplier, int iteration)
		{
			int unConditions;
			int aIteration;
			float utility = ComputeUtility (uts, out unConditions, out aIteration);
			return utility * Mathf.Pow (conditionalUtilityMultiplier, unConditions) * Mathf.Pow (iterationMultiplier, iteration - aIteration);
		}

		public abstract float ComputeUtility (Utilities uts, out int notSatisfiedConditions, out int actionIteration);

		public abstract bool IsPossible ();

		public abstract bool IsPossibleAtAll (GameObject go);

		public abstract void Perform ();

		public abstract void Discard (Utilities uts);

		public abstract void Approve (Utilities uts, int iteration);

		public virtual void Tick ()
		{
			
		}

		public virtual void Update ()
		{
			
		}
	}

	public abstract class Promise
	{
		public abstract Type ConditionType { get; }

		public int Iteration;

		public abstract float CheckCondition (Condition condition);
	}

	public abstract class Condition
	{
		//Поколение для отслеживания нужно ли добавлять полезность
		public int Generation;
		public int Iteration;
		public float Utility;

		public abstract bool Satisfied { get; }

	}

	public abstract class UtilitySystem : IEnumerable<Action>
	{
		
		public abstract IEnumerator<Action> GetEnumerator ();

		IEnumerator IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		protected readonly HashSet<KeyValuePair<Condition, Type>> usedActions = new HashSet<KeyValuePair<Condition, Type>> ();
		protected readonly List<Action> actionsList = new List<Action> ();

		public abstract List<Type> GetRelevantActions (GameObject gameObject);

		//Убирает все неиспользованные action'ы из списка,
		//performedAction - не нужно удалять или очищать, но нужно убрать из списка заранее
		public abstract void ClearActions (Utilities uts, Action performedAction);
		//Возвращает true если были добавлены новые action'ы или изменены utility старых
		public abstract bool PrepareActions (Utilities uts, int iteration, float conditionalUtilityMultiplier, float iterationMultiplier);

		public abstract void ReceiveRelevantActions (List<Type> types);
	}

	public abstract class ProtoUtSystem<T, S> : UtilitySystem where S : ProtoUtSystem<T, S>
	{
		static protected readonly List<Type> allRelevatActions;
		static readonly Type relevantType = typeof(T);

		public override IEnumerator<Action> GetEnumerator ()
		{
			return actionsList.GetEnumerator ();
		}

		List<Type> relevantActions;

		public override void ReceiveRelevantActions (List<Type> types)
		{
			relevantActions = types;
		}

		public override void ClearActions (Utilities uts, Action performedAction)
		{
			for (int i = 0; i < actionsList.Count; i++)
				actionsList [i].Discard (uts);
			usedActions.Clear ();
			actionsList.Clear ();
		}

		public override List<Type> GetRelevantActions (GameObject gameObject)
		{
			List<Type> types = new List<Type> ();
			foreach (var aType in allRelevatActions)
			{
				Action a = Activator.CreateInstance (aType) as Action;
				if (a.IsPossibleAtAll (gameObject))
					types.Add (aType);
			}
			return types;
		}


		public override bool PrepareActions (Utilities uts, int iteration, float conditionalUtilityMultiplier, float iterationMultiplier)
		{
			var conditions = uts.GetConditions (typeof(C_InRange));
			bool changed = false;
			for (int i = 0; i < actionsList.Count; i++)
			{
				if (!actionsList [i].Approved && !actionsList [i].IsPossible ())
				{
					actionsList [i].Approved = true;
					actionsList [i].Approve (uts, iteration);
				}
			}
			changed |= CreateActions (conditions, iteration);			

			for (int i = 0; i < actionsList.Count; i++)
			{
				float oldUtility = actionsList [i].Utility;
				float newUtility = actionsList [i].GetUtility (uts, conditionalUtilityMultiplier, iterationMultiplier, iteration);
				actionsList [i].Utility = newUtility;
				changed |= Mathf.Abs (newUtility - oldUtility) > Mathf.Epsilon;
			}
			return changed;
		}

		protected abstract bool CreateActions (List<Condition> conditions, int iteration);
	}

	public interface MovementAction
	{
		void Setup (C_InRange targetCondition, int iteration);
	}

	public class MovementUS : ProtoUtSystem<MovementAction, MovementUS>
	{
		public override IEnumerator<Action> GetEnumerator ()
		{
			return actionsList.GetEnumerator ();
		}

		List<Type> relevantActions;

		public override void ReceiveRelevantActions (List<Type> types)
		{
			relevantActions = types;
		}

		public override void ClearActions (Utilities uts, Action performedAction)
		{
			for (int i = 0; i < actionsList.Count; i++)
				actionsList [i].Discard (uts);
			usedActions.Clear ();
			actionsList.Clear ();
		}

		public override List<Type> GetRelevantActions (GameObject gameObject)
		{
			List<Type> types = new List<Type> ();
			foreach (var aType in allRelevatActions)
			{
				Action a = Activator.CreateInstance (aType) as Action;
				if (a.IsPossibleAtAll (gameObject))
					types.Add (aType);
			}
			return types;
		}


		protected override bool CreateActions (List<Condition> conditions, int iteration)
		{
			bool changed = false;
			for (int i = 0; i < conditions.Count; i++)
			{
				for (int j = 0; j < relevantActions.Count; j++)
				{
					if (usedActions.Add (new KeyValuePair<Condition, Type> (conditions [i], relevantActions [j])))
					{
						MovementAction action = Activator.CreateInstance (relevantActions [j]) as MovementAction;
						action.Setup (conditions [i] as C_InRange, iteration);
						actionsList.Add (action as Action);
						changed = true;
					}
				}
			}
			return changed;
		}
	}

	public class A_TeleportTo : Action, MovementAction
	{
		P_InRange promise = new P_InRange ();
		float utility;

		public void Setup (C_InRange targetCondition, int iteration)
		{
			promise.Iteration = iteration;
			promise.PromisedPosition = targetCondition.TargetPosition;
			promise.TargetTransform = targetCondition.TargetTransform;
		}

		public override float ComputeUtility (Utilities uts, out int notSatisfiedConditions, out int actionIteration)
		{
			notSatisfiedConditions = 0;
			actionIteration = promise.Iteration;
			return uts.GetUtility (promise);
		}

		public override bool IsPossible ()
		{
			return true;
		}

		public override bool IsPossibleAtAll (GameObject go)
		{
			return true;
		}


		public override void Perform ()
		{
			promise.TargetTransform.position = promise.PromisedPosition;
		}

		public override void Discard (Utilities uts)
		{
			//throw new NotImplementedException ();
		}

		public override void Approve (Utilities uts, int iteration)
		{
			//throw new NotImplementedException ();
		}
		
	}

	public class P_InRange : Promise
	{
		public Vector3 PromisedPosition;
		public Transform TargetTransform;

		public override Type ConditionType {
			get
			{
				return typeof(C_InRange);
			}
		}

		public override float CheckCondition (Condition condition)
		{
			C_InRange c = condition as C_InRange;
			return (c.TargetPosition - PromisedPosition).magnitude < c.Range ? 1f : 0f;
		}
	}

	public class C_InRange : Condition
	{
		public float Range;
		public Transform TargetTransform;
		public Vector3 TargetPosition;

		public override bool Satisfied { get { return (TargetPosition - TargetTransform.position).magnitude < Range; } }
	}


	public class Utilities : MonoBehaviour
	{
		Dictionary<Type, List<Condition>> conditions = new Dictionary<Type, List<Condition>> ();

		public float GetUtility (Promise promise)
		{
			List<Condition> promisedConditions = null;
			conditions.TryGetValue (promise.ConditionType, out promisedConditions);
			if (promisedConditions == null || promisedConditions.Count == 0)
				return 0f;
			float sumUt = 0f;
			for (int i = 0; i < promisedConditions.Count; i++)
			{
				if (promise.Iteration <= promisedConditions [i].Iteration)
					sumUt += promise.CheckCondition (promisedConditions [i]);
			}
			return sumUt;
		}

		public List<Condition> GetConditions (Type type)
		{
			List<Condition> list = null;
			conditions.TryGetValue (type, out list);
//			if (list == null)
//				return new List<Condition> ();
			return list;
		}

		public void AddUtility (Condition condition)
		{
			List<Condition> list = null;
			Type cType = condition.GetType ();
			if (!conditions.TryGetValue (cType, out list))
			{
				list = new List<Condition> ();
				conditions.Add (cType, list);
			}
			list.Add (condition);

		}

		public void RemoveUtility (Condition condition)
		{
			List<Condition> list = null;
			Type cType = condition.GetType ();
			if (conditions.TryGetValue (cType, out list))
				list.Remove (condition);

		}

	}



}