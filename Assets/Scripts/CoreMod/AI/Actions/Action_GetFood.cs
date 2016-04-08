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
			if (production.CurValue > 0)
			{
				var result = Mathf.Min (production.TargetValue, production.CurValue);
				production.CurValue -= result;
				PostCondition.CurValue += result;
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
				var baseEffect = (float)production.CurValue / (float)production.TargetValue;
				var result = production.Plan (planner);
				result.Effect += baseEffect;
				return result;
			}

			return new PlanResult (0f, 1f);
		}

		protected override void PreparePreConditions ()
		{
			production.Setup (PostCondition.Component);
			production.TargetValue = PostCondition.TargetValue;
			production.Spec = NumericConditionSpec.MoreEqual;
		}

		protected override void BorrowStates ()
		{
			production.Borrow (production.TargetValue);
		}

		protected override void ReleaseStates ()
		{
			production.Release ();
		}

		protected override void ReleaseConditions ()
		{
			production.DePlan ();
		}
	}

}
