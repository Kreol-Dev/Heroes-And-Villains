using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AI
{
	//	public class Conditions : MonoBehaviour
	//	{
	//		Dictionary<Type, Condition> conditions = new Dictionary<Type, Condition> ();
	//
	//		public T GetCondition<T> () where T : Condition
	//		{
	//			Type t = typeof(T);
	//			Condition condition = null;
	//			conditions.TryGetValue (t, out condition);
	//			return condition as T;
	//		}
	//
	//		//		public void AssignCondition (Type conditionType)
	//		//		{
	//		//			conditions.Add(conditionType,
	//		//		}
	//	}

	public abstract class Condition
	{
		public Action PlannedAction { get; internal set; }

		public abstract PlanResult Plan (Planner planner);
	}

	public abstract class Condition<T, C> : Condition where T : Condition<T, C> where C : Component
	{

		public C Component { get; internal set; }

		public bool Satisfied;
		public bool Borrowed;

		public sealed override PlanResult Plan (Planner planner)
		{
			var relActions = planner.RelevantActions (typeof(T));
			//var node = plan [lastNodeID];
			float minCost = float.MaxValue;
			PlanResult bestResult = new PlanResult ();
			int minIndex = -1;
			for (int i = 0, maxCount = relActions.Count; i < maxCount; i++)
			{
				var action = relActions [i];
				var result = action.Plan (planner, this);
				if (result.Effect < 0)
					continue;
				float cost = (1 - planner.ContentratedOnResult) * result.Effect + planner.ContentratedOnResult * result.Cost;
				if (cost < minCost)
				{
					minCost = cost;
					bestResult = result;
					minIndex = i;
				}
			}
			if (minIndex > 0)
				PlannedAction = relActions [minIndex];
			else
				PlannedAction = null;
			for (int i = 0, maxCount = relActions.Count; i < maxCount; i++)
			{
				if (i != minIndex)
					relActions [i].DePlan ();
			}
			return bestResult;
		}

		public void Setup (GameObject go)
		{
			Component = go.GetComponent<C> ();
		}

		public void Setup (C cmp)
		{
			Component = cmp;
		}


	}
		
}


