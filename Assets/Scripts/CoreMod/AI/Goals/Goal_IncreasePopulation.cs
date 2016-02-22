using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class Goal_IncreasePopulation : AI.Goal
	{
		Condition_PopulationBiggerThan condition;

		public Goal_IncreasePopulation (GameObject target, int delta)
		{
			condition = target.GetComponent<AI.Conditions> ().GetCondition<Condition_PopulationBiggerThan> ();
			condition.TargetPopulation = condition.settlement.Population + delta;
		}

	}
}


