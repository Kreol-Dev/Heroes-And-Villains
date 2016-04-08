using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AI
{
	public abstract class Decision
	{
		public abstract bool CheckPrefab (GameObject go);


	}

	public abstract class Decision<C, T> : Decision where C : Condition
	{
		Dictionary<Type, List<T>> objectsPostStates;


		public PlanResult Decide (C condition, Planner planner, out IEnumerable<Condition> resultConditions)
		{
			resultConditions = null;
			List<T> relObjects = null;
			if (!objectsPostStates.TryGetValue (condition.StateType, out relObjects))
			{
				return new PlanResult (0, -1f);
			} else if (relObjects.Count == 0)
				return new PlanResult (0, -1f);
			float minCost = float.MaxValue;
			int minIndex = -1;
			PlanResult optimalResult = new PlanResult (0, -1f);
			IEnumerable<Condition> optimalConditions = null;
			for (int i = 0; i < relObjects.Count; i++)
			{
				T obj = relObjects [i];
				var conditions = GetPreconditions (obj);
				bool satisfied = true;
				PlanResult res = ObjectResult (obj, planner, condition);
				if (res.Effect <= 0)
					continue;
				foreach (var c in conditions)
				{
					BorrowCondition (c);
				}
				foreach (var c in conditions)
				{
					if (!c.Satisfied)
					{
						var result = c.Plan (planner);
						if (result.Effect < 1)
						{
							satisfied = false;
							break;
						} else
							res.Cost += result.Cost;
					}
				}
				if (!satisfied)
				{
					foreach (var c in conditions)
					{
						ReleaseCondition (c);
					}
					continue;
				}
				if (res.Cost < minCost)
				{
					if (minIndex != -1)
						foreach (var c in GetPreconditions(relObjects[minIndex]))
						{
							ReleaseCondition (c);
						}
					optimalResult = res;
					minIndex = i;
					optimalConditions = conditions;
				}
			}

			if (minIndex == -1)
				return new PlanResult (0, -1f);
			else
			{
				resultConditions = optimalConditions;
				return optimalResult;
			}


		}

		public void DePlan (IEnumerable<Condition> conditions)
		{
			foreach (var c in conditions)
			{
				ReleaseCondition (c);
				c.DePlan ();				
			}
			
		}

		public void Approve (Agent agent, IEnumerable<Condition> conditions)
		{
			foreach (var c in conditions)
			{
				ReleaseCondition (c);		
				c.PlannedAction.ApproveAction (agent);
			}
		}

		void BorrowCondition (Condition c)
		{
			BorrowableCondition<int> intC = c as BorrowableCondition<int>;
			if (intC != null)
				intC.DefaultBorrow ();
			BorrowableCondition<float> floatC = c as BorrowableCondition<float>;
			if (floatC != null)
				floatC.DefaultBorrow ();
		}

		void ReleaseCondition (Condition c)
		{
			BorrowableCondition<int> intC = c as BorrowableCondition<int>;
			if (intC != null)
				intC.Release ();
			BorrowableCondition<float> floatC = c as BorrowableCondition<float>;
			if (floatC != null)
				floatC.Release ();
		}

		public override bool CheckPrefab (GameObject go)
		{
			return false;
		}

		protected abstract IEnumerable<Condition> GetPreconditions (T obj);

		protected abstract IEnumerable<Type> GetObjectStates (T obj);

		protected abstract IEnumerable<T> GetPossibleObjects ();

		protected abstract PlanResult ObjectResult (T obj, Planner planner, C condition);
	}
}


