using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UIO;


namespace AI
{
	[ECompName ("planner")]
	public class Planner : EntityComponent
	{
		Dictionary<Type, Decision> decisions = new Dictionary<Type, Decision> ();

		public override void OnInitPrefab ()
		{
			//TODO: Написать код получения графа действий из общего графа
			var aiRoot = Find.Root<AI.AIRoot> ();
			this.actions = aiRoot.GetActionsForPrefab (this.gameObject);
		}

		public override void LoadFromTable (ITable table)
		{
			Find.Root<ModsManager> ().Defs.LoadObject (this, table);
		}

		public override EntityComponent CopyTo (GameObject go)
		{
			var planner = go.AddComponent<Planner> ();
			planner.actions = this.actions;
			return planner;
		}

		public override void PostCreate ()
		{
		}

		protected override void PostDestroy ()
		{
		}

		[Defined ("risky")]
		public float Risky { get; internal set; }

		[Defined ("bold")]
		public float Bold { get; internal set; }

		[Defined ("concentrated_on_result")]
		public float ContentratedOnResult { get; internal set; }

		Dictionary<Type, List<ActionsPool>> actions;
		Stack<List<Action>> cachedRelActions = new Stack<List<Action>> ();
		//List<Action> cachedRelActions = new List<Action> ();

		public List<Action> RelevantActions (Type conditionType)
		{
			List<ActionsPool> relPools = null;
			List<Action> relActions = null;
			if (cachedRelActions.Count == 0)
				relActions = new List<Action> ();
			else
				relActions = cachedRelActions.Pop ();
			if (actions.TryGetValue (conditionType, out relPools))
			{
				for (int i = 0; i < relPools.Count; i++)
					relActions.Add (relPools [i].GetFreeAction ());
			}
			return relActions;

		}

		public void ReturnList (List<Action> actions)
		{
			actions.Clear ();
			cachedRelActions.Push (actions);
		}

		public void Plan (Condition condition)
		{
			var result = condition.Plan (this);
			//if (condition.PlannedAction != null)
			//condition.PlannedAction.ApproveAction ();
		}




	}


	public struct PlanResult
	{
		public float Cost;
		public float Effect;

		public PlanResult (float cost, float effect)
		{
			Cost = cost;
			Effect = effect;
		}

	}

}

