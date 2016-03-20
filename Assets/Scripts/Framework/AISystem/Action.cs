using UnityEngine;
using System.Collections;

namespace AI
{
	public abstract class Action
	{
		protected Condition PostCondition;

		public PlanResult Plan (Planner planner, Condition postCondition)
		{
			this.PostCondition = postCondition;
			PreparePreConditions ();
			MakeDecisions ();
			BorrowStates ();
			float risk;
			float difficulty;
			var carriedResult = OnPlan (planner, out risk, out difficulty);
			carriedResult.Cost += (1 - planner.Bold) * difficulty + (1 - planner.Risky) * risk;
			return carriedResult;
		}

		public abstract void ApproveAction ();



		public void DePlan ()
		{
			ReleaseStates ();
			OnDePlan ();
		}

		protected abstract PlanResult OnPlan (Planner planner, out float risk, out float difficulty);

		protected abstract void PreparePreConditions ();

		protected virtual void MakeDecisions ()
		{
			
		}

		//Like in SpendMoney
		protected virtual void BorrowStates ()
		{
			
		}

		//If SpendMoney canceled
		protected virtual void ReleaseStates ()
		{
			
		}

		protected abstract void OnDePlan ();
	

	}

	public abstract class Action<T, C> : Action where T : Action<T, C> where C : Condition
	{
		protected C PostCondition { get { return base.PostCondition as C; } }

		static ActionsPool pool;

		public void Free ()
		{
			pool.ReturnAction (this);
		}

		public Action GetAnother ()
		{
			return pool.GetFreeAction ();
		}

		protected sealed override void OnDePlan ()
		{
			pool.ReturnAction (this);
		}
	}

}