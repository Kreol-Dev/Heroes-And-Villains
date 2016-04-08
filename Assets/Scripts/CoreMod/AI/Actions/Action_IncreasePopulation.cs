using UnityEngine;
using System.Collections;
using AI;

namespace CoreMod
{
	public class Action_IncreasePopulation : AI.Action<Action_IncreasePopulation, C_Population>
	{
		C_Food Food = new C_Food ();

		public override void ApproveAction (Agent agent)
		{
			ReleaseStates ();
			if (Food.PlannedAction != null)
				Food.PlannedAction.ApproveAction (agent);
			agent.PushAction (this);
		}

		public override void OnTick ()
		{
			if (Food.CurValue > 0)
			{
				var food = Mathf.Min (Food.TargetValue, Food.CurValue);
				Food.CurValue -= food;
				PostCondition.CurValue += food / 2;
				Done ();
			} else
				Fail ();
		}

		public override void OnTimedUpdate (float timeDelta)
		{

		}

		public override bool CheckPrefab (GameObject go)
		{
			return Food.CanBeApplied (go);
		}
		//		public override void DiscardAction ()
		//		{
		//
		//		}

		protected override PlanResult OnPlan (AI.Planner planner, out float risk, out float difficulty)
		{
			risk = 0f;
			difficulty = 1f;
			if (!Food.Satisfied)
			{
				var carriedResult = Food.Plan (planner);
				return carriedResult;
			}
			return new PlanResult (0f, 1f);
		}

		protected override void PreparePreConditions ()
		{
			Food.Setup (PostCondition.Component);
			Food.TargetValue = (PostCondition.TargetValue - PostCondition.CurValue) * 2;
			Food.Spec = NumericConditionSpec.MoreEqual;
		}

		protected override void BorrowStates ()
		{
			Food.Borrow (Food.TargetValue);
		}

		protected override void ReleaseStates ()
		{
			Food.Release ();
		}

		protected override void ReleaseConditions ()
		{
			Food.DePlan ();
		}
	}



}
