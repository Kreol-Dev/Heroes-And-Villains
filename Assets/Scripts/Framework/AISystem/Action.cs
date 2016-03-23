using UnityEngine;
using System.Collections;

namespace AI
{
	public abstract class Action
	{
		public Condition PostCondition { get; internal set; }

		public delegate void ActionDelegate (Action action);

		public event ActionDelegate ActionDone;

		protected void Done ()
		{
			//Debug.Log ("Done " + this.GetType ());
			if (ActionDone != null)
				ActionDone (this);
		}

		public event ActionDelegate ActionFailed;

		protected void Fail ()
		{
			Debug.Log ("Fail " + this.GetType ());
			if (ActionFailed != null)
				ActionFailed (this);
		}

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

		public abstract bool CheckPrefab (GameObject go);

		public abstract void ApproveAction (Agent agent);

		/// <summary>
		/// For simulation updates
		/// </summary>
		public abstract void OnTick ();

		/// <summary>
		/// For animation and similar stuff updates
		/// </summary>
		/// <param name="timeDelta">Time delta.</param>
		public abstract void OnTimedUpdate (float timeDelta);


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
			ReleaseConditions ();
			pool.ReturnAction (this);
		}

		protected abstract void ReleaseConditions ();
	}

}