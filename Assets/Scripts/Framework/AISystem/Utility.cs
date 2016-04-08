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

	public class FullAgent : EntityComponent
	{
		List<UtilitySystem> systems;
		Utilities utilities;
		int planningIteration = 0;

		public override void OnInitPrefab ()
		{
			systems = Find.Root<AIRoot> ().GetSystemsForPrefab (gameObject);
		}

		public override EntityComponent CopyTo (GameObject go)
		{
			var agent = go.AddComponent<FullAgent> ();
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
			Action maxUtAction = null;
			float maxUt = float.MinValue;
			bool anyUpdates = false;
			for (int i = 0; i < systems.Count; i++)
			{
				var system = systems [i];
				anyUpdates = system.PrepareActions (utilities, planningIteration) || anyUpdates;

				foreach (var action in system)
				{
					if (action.Utility > maxUt)
					{
						maxUtAction = action;
						maxUt = action.Utility;
					}
				}
			}

			if (maxUtAction.IsPossible ())
			{
				maxUtAction.Perform ();
				for (int i = 0; i < systems.Count; i++)
					systems [i].ClearActions (utilities, maxUtAction);
				planningIteration = 0;
			} else if (!anyUpdates)
			{
				//TODO: If it is not possible at all to perform the most useful action, make utility of it smaller
			} 
		}




	}

	public abstract class Action
	{
		public float Utility { get; set; }

		public bool Approved { get; set; }

		public abstract float GetUtility (Utilities uts);

		public abstract bool IsPossible ();

		public abstract bool IsPossibleAtAll (GameObject go);

		public abstract void AssignUtility (float ut);

		public abstract void Perform ();

		public abstract void Discard (Utilities uts);

		public abstract void Approve (Utilities uts, int iteration);

	}

	public abstract class Promise
	{
		public abstract Type ConditionType { get; }

		public abstract float CheckCondition (Condition condition);
	}

	public abstract class Condition
	{
		//Поколение для отслеживания нужно ли добавлять полезность
		public int Generation;

		public float Utility;
		public object Target;

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
		public abstract bool PrepareActions (Utilities uts, int iteration);

		public abstract void ReceiveRelevantActions (List<Type> types);
	}

	public class MovementUS : UtilitySystem
	{

		static List<Type> allRelevatActions;
		static readonly Type relevantType = typeof(MovementAction);

		public override IEnumerator<Action>  GetEnumerator ()
		{
			return actionsList.GetEnumerator ();
		}


		public interface MovementAction
		{
			void Setup (C_InRange targetCondition);
		}


		//HashSet<KeyValuePair<Condition, Action>
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


		public override bool PrepareActions (Utilities uts, int iteration)
		{
			var conditions = uts.GetConditions (typeof(C_InRange));
			if (conditions == null)
				return false; //FIXME: Find random traversable target and check if it has some utility via side effects
			bool changed = false;
			for (int i = 0; i < actionsList.Count; i++)
			{
				if (!actionsList [i].IsPossible () && !actionsList [i].Approved)
				{
					actionsList [i].Approved = true;
					actionsList [i].Approve (uts, iteration);
				}
			}
			for (int i = 0; i < conditions.Count; i++)
			{
				for (int j = 0; j < relevantActions.Count; j++)
				{
					if (usedActions.Add (new KeyValuePair<Condition, Type> (conditions [i], relevantActions [j])))
					{
						MovementAction action = Activator.CreateInstance (relevantActions [j]) as MovementAction;
						action.Setup (conditions [i] as C_InRange);
						actionsList.Add (action as Action);
						changed = true;
					}
				}
			}

			for (int i = 0; i < actionsList.Count; i++)
			{
				float oldUtility = actionsList [i].Utility;
				float newUtility = actionsList [i].GetUtility (uts);
				actionsList [i].Utility = newUtility;
				changed |= Mathf.Abs (newUtility - oldUtility) > Mathf.Epsilon;
			}
			return changed;
		}
	}

	public class A_TeleportTo : Action, MovementUS.MovementAction
	{
		P_InRange promise = new P_InRange ();
		float utility;

		public void Setup (C_InRange targetCondition)
		{
			promise.PromisedPosition = targetCondition.TargetPosition;
			promise.TargetTransform = targetCondition.TargetTransform;
		}

		public override float GetUtility (Utilities uts)
		{
			utility = uts.GetUtility (promise);
			return utility;
		}

		public override bool IsPossible ()
		{
			return true;
		}

		public override bool IsPossibleAtAll (GameObject go)
		{
			return true;
		}

		public override void AssignUtility (float ut)
		{
			throw new NotImplementedException ();
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