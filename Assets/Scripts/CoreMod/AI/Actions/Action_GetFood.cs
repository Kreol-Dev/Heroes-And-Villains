using UnityEngine;
using System.Collections;
using AI;

namespace CoreMod
{
	public class Action_GetFood : AI.Action<Action_GetFood, C_Food>
	{
		C_Production production = new C_Production ();

		public override bool CheckPrefab (GameObject go)
		{
			return production.CanBeApplied (go);
		}

		public override void ApproveAction (Agent agent)
		{
			ReleaseStates ();
			if (production.PlannedAction != null)
				production.PlannedAction.ApproveAction (agent);
			agent.PushAction (this);
		}

		public override void OnTick ()
		{
			if (production.CurProduction > 0)
			{
				var result = Mathf.Min (production.TargetProduction, production.CurProduction);
				production.CurProduction -= result;
				PostCondition.CurFood += result;
				Done ();
			} else
				Fail ();
		}

		public override void OnTimedUpdate (float timeDelta)
		{
			
		}
		//		public override void DiscardAction ()
		//		{
		//		}

		protected override PlanResult OnPlan (Planner planner, out float risk, out float difficulty)
		{
			risk = 0f;
			difficulty = 1f;
			if (!production.Satisfied)
			{
				var baseEffect = (float)production.CurProduction / (float)production.TargetProduction;
				var result = production.Plan (planner);
				result.Effect += baseEffect;
				return result;
			}

			return new PlanResult (0f, 1f);
		}

		protected override void PreparePreConditions ()
		{
			production.Setup (PostCondition.Component);
			production.TargetProduction = PostCondition.TargetFood;
			if (production.TargetProduction < production.CurProduction)
				production.Satisfied = true;
			else
				production.Satisfied = false;
		}

		protected override void BorrowStates ()
		{
			production.CurProduction -= production.TargetProduction;
			production.Borrowed = true;
		}

		protected override void ReleaseStates ()
		{
			if (production.Borrowed)
			{
				production.CurProduction += production.TargetProduction;
				production.Borrowed = false;
			}
		}

		protected override void ReleaseConditions ()
		{
			production.DePlan ();
		}
	}

}
