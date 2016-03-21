using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace AI
{
	public class ActionGraph
	{
		Dictionary<Type, List<ActionNode>> postConditionToActions = new Dictionary<Type, List<ActionNode>> ();

		class ActionNode
		{
			public Type ActionType { get; internal set; }

			public ActionsPool ActionsPool { get; internal set; }

			public Dictionary<Type, List<ActionNode>> PreActions { get; internal set; }

			public ActionNode (Type actionType, ActionsPool actionsPool)
			{
				ActionType = actionType;
				ActionsPool = actionsPool;
				PreActions = new Dictionary<Type, List<ActionNode>> ();
			}
		}

		void AddAction (ActionNode node, Type postConditionType)
		{
			List<ActionNode> actions = null;
			if (!postConditionToActions.TryGetValue (postConditionType, out actions))
			{
				actions = new List<ActionNode> ();
				postConditionToActions.Add (postConditionType, actions);
			}
			actions.Add (node);
		}

		public ActionGraph (IEnumerable<Type> actionTypes, Dictionary<Type, ActionsPool> pools)
		{
			//TODO: Change everything to a tree
			foreach (var actionType in actionTypes)
			{
				var postConditionType = actionType.BaseType.GetGenericArguments () [1];
				ActionNode node = new ActionNode (actionType, pools [actionType]);
				AddAction (node, postConditionType);
			}

			foreach (var pair in postConditionToActions)
			{
				Type postCondition = pair.Key;
				foreach (var actionNode in pair.Value)
				{
					var preConditions = from field in actionNode.ActionType.GetFields ()
					                    where field.Name != "PostCondition" && field.FieldType.IsSubclassOf (typeof(Condition))
					                    select field.FieldType;
					foreach (var pretype in preConditions)
						actionNode.PreActions.Add (pretype, postConditionToActions [pretype]);
				}
			}
		}


		public Dictionary<Type, List<ActionsPool>> ProvideGraphForPrefab (GameObject go)
		{
			Dictionary<Type, List<ActionsPool>> graph = new Dictionary<Type, List<ActionsPool>> ();
			foreach (var pair in postConditionToActions)
			{
				foreach (var node in pair.Value)
				{
					var action = node.ActionsPool.GetFreeAction ();
					if (action.CheckPrefab (go))
					{
						List<ActionsPool> pools = null;
						graph.TryGetValue (pair.Key, out pools);
						if (pools == null)
						{
							pools = new List<ActionsPool> ();
							graph.Add (pair.Key, pools);
						}
						pools.Add (node.ActionsPool);
					}
					node.ActionsPool.ReturnAction (action);

				}
			}
			return graph;
		}
	}
}


