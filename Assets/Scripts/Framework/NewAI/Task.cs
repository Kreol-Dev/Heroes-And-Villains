using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace NewAI
{
	public abstract class Task
	{
		public int Iteration;

		public abstract void Setup (Agent agent, Condition condition);

		public abstract bool Do (Agent agent, Condition c, Utilities uts);

		protected abstract void OnActionSucceed ();

		protected abstract void OnActionFailed ();


		protected Action PlannedAction;
	}

	public abstract class Task<C, J> : Task where C : Condition where J : class
	{
		protected abstract void InitAction (J jobAction);

		public sealed override bool Do (Agent agent, Condition c, Utilities uts)
		{
			if (c.Satisfied)
				return false;
			var actions = agent.GetActions (typeof(J), PlannedAction);
			float maxUt = 0;
			Action maxUtAction = null;
			for (int i = 0; i < actions.Count; i++)
			{
				if (actions [i] != PlannedAction)
				{
					actions [i].Setup (agent);
					InitAction (actions [i] as J);
				}
				float utility = actions [i].GetUtility (uts);
				if (utility > maxUt)
				{
					maxUt = utility;
					maxUtAction = actions [i];
				}
			}
			if (maxUtAction != null && PlannedAction != maxUtAction)
			{
				maxUtAction.Prepare (Iteration);
			}
			PlannedAction = maxUtAction;
			if (PlannedAction != null)
			{

				PlannedAction.Update (OnActionSucceed, OnActionFailed, uts);
				return true;
			}
			return false;
		}

		public sealed override void Setup (Agent agent, Condition condition)
		{
			Setup (agent, condition as C);
		}

		protected abstract void Setup (Agent agent, C condition);
	}

}