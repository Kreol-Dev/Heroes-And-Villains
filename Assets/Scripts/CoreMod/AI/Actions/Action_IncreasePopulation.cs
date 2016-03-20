using UnityEngine;
using System.Collections;
using AI;

namespace CoreMod
{
	public class Action_IncreasePopulation : AI.Action<Action_IncreasePopulation, C_Population>
	{
		C_Food Food = new C_Food ();

		public override void ApproveAction ()
		{
			
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
			Food.TargetFood = (PostCondition.TargetPopulation - PostCondition.CurPopulation) * 2;
			if (Food.TargetFood <= Food.CurFood)
				Food.Satisfied = true;
			else
				Food.Satisfied = false;
		}

		protected override void BorrowStates ()
		{
//			if (Food.Satisfied)
//			{
			Food.CurFood -= Food.TargetFood;
			Food.Borrowed = true;
//			}
		}

		protected override void ReleaseStates ()
		{
			if (Food.Borrowed)
			{
				Food.CurFood += Food.TargetFood;
				Food.Borrowed = false;
			}
		}
	
	}



}
