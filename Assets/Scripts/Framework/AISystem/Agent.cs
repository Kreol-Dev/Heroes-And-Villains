using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
	[ECompName ("agent")]
	public class Agent : EntityComponent
	{
		

		public override EntityComponent CopyTo (GameObject go)
		{
			var cmp = go.AddComponent<Agent> ();
			return cmp;
		}

		static Ticker ticker;

		public override void PostCreate ()
		{
			if (ticker == null)
				ticker = Find.Root<AI.Ticker> ();
		}

		protected override void PostDestroy ()
		{
		}

		[SerializeField]
		List<Action> actions = new List<Action> ();
		Condition condition;
		int curActionIndex = -1;

		public void PushAction (Action action)
		{
			actions.Add (action);

		}

		public void Do (Condition condition)
		{
			this.condition = condition;
			curActionIndex = 0;
			//actions.Add (action);
			//ticker.Tick += action.OnTick;
			condition.PlannedAction.ApproveAction (this);
			Perform ();

		}

		public void Perform ()
		{
			var action = actions [curActionIndex];
			action.ActionDone += OnActionDone;
			action.ActionFailed += OnActionFailed;
			ticker.Tick += action.OnTick;
		}

		void OnActionDone (Action action)
		{
			action.ActionDone -= OnActionDone;
			action.ActionFailed -= OnActionFailed;
			ticker.Tick -= action.OnTick;
			curActionIndex++;
			if (curActionIndex == actions.Count)
			{
				curActionIndex = -1;
				condition.DePlan ();
				actions.Clear ();
				condition = null;
			} else
				Perform ();
		}

		void OnActionFailed (Action action)
		{
			action.ActionDone -= OnActionDone;
			action.ActionFailed -= OnActionFailed;
			ticker.Tick -= action.OnTick;
			curActionIndex = -1;
			condition.DePlan ();
			condition = null;
			actions.Clear ();
		}

		void Update ()
		{
			if (curActionIndex != -1)
				actions [curActionIndex].OnTimedUpdate (Time.deltaTime);
		}
	}
}


