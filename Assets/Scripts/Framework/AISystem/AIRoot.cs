using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AI
{
	[RootDependencies (typeof(ModsManager))]
	public class AIRoot : Root
	{
		Dictionary<Type, ActionsPool> pools = new Dictionary<Type, ActionsPool> ();
		Dictionary<string, Type> conditions = new Dictionary<string, Type> ();
		ActionGraph fullActionsGraph;

		protected override void CustomSetup ()
		{
			var allTypes = Find.Root<ModsManager> ().GetAllTypes ();
			var actionsTypes = from type in allTypes
			                   where type.IsSubclassOf (typeof(Action)) && !type.IsGenericType && !type.IsAbstract
			                   select type;
			
			foreach (var actionType in actionsTypes)
			{
				var newPool = new ActionsPool (actionType);
				pools.Add (actionType, newPool);
				var poolField = actionType.BaseType.GetField ("pool", BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic);
				poolField.SetValue (null, newPool);
			}

			fullActionsGraph = new ActionGraph (actionsTypes, pools);

			var field = typeof(Condition).GetProperty ("Scribe", BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic);
			field.SetValue (null, Scribes.Register ("Conditions"), null);



			var conditionPairs = from type in allTypes
			                     let attrs = type.GetCustomAttributes (typeof(ConditionAttribute), false)
			                     where attrs.Length > 0 && type.IsSubclassOf (typeof(Condition)) && !type.IsGenericType && !type.IsAbstract
			                     select new KeyValuePair<string, Type> ((attrs [0] as ConditionAttribute).Name, type);
			foreach (var cPair in conditionPairs)
				conditions.Add (cPair.Key, cPair.Value);



			var mm = Find.Root<ModsManager> ();
//			var table = mm.GetTable ("num_condition");
//			mm.SetTableAsGlobal ("num_condition");
//			var numNames = Enum.GetNames (typeof(NumericConditionSpec));
//			for (int i = 0; i < numNames.Length; i++)
//			{
//				var numName = numNames [i];
//				table.Set (numName, i);
//			}
//
//			table = mm.GetTable ("class_condition");
//			mm.SetTableAsGlobal ("class_condition");
//			var clNames = Enum.GetNames (typeof(NumericConditionSpec));
//			for (int i = 0; i < clNames.Length; i++)
//			{
//				var clName = clNames [i];
//				table.Set (clName, i);
//			}
			Fulfill.Dispatch ();
		}

		public Action GetAction (Type type)
		{
			return pools [type].GetFreeAction ();
		}

		public void ReleaseAction (Action action)
		{
			pools [action.GetType ()].ReturnAction (action);
		}

		public Dictionary<Type, List<ActionsPool>> GetActionsForPrefab (GameObject go)
		{
			
			return fullActionsGraph.ProvideGraphForPrefab (go);
		}

		public Condition GetCondition (string conditionName)
		{
			Type cType = null;
			if (!conditions.TryGetValue (conditionName, out cType))
				return null;
			return Activator.CreateInstance (cType) as Condition;

		}
	}

	public class ConditionAttribute : Attribute
	{
		public string Name { get; internal set; }

		public ConditionAttribute (string name)
		{
			Name = name;
		}
	}

	public class ActionsPool
	{
		Stack<Action> actions = new Stack<Action> ();
		Type t;


		public ActionsPool (Type t)
		{
			this.t = t;
		}

		public Action GetFreeAction ()
		{
			if (actions.Count == 0)
			{
//				Debug.Log ("Pop New " + t);
				return Activator.CreateInstance (t) as Action;
			} else
//				Debug.Log ("Pop " + t);
			return actions.Pop ();
		}

		public void ReturnAction (Action action)
		{	
//			Debug.Log ("Returned " + t);
			actions.Push (action);
		}
	}

}


