using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class Goal_IncreasePopulation : AI.Goal
	{
		C_Population condition;

		public Goal_IncreasePopulation (GameObject target, int delta)
		{
			condition = target.GetComponent<AI.Conditions> ().GetCondition<C_Population> ();
			condition.TargetPopulation = condition.settlement.Population + delta;
		}

	}
}


