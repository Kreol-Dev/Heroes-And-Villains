using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class Condition_PopulationBiggerThan : AI.Condition
	{
		public override bool IsValidFor (GameObject go)
		{
			var settlement = go.GetComponent<Settlement> ();
			return settlement != null;
		}

		public Settlement settlement { get; private set; }

		public int TargetPopulation;

		protected override void Setup ()
		{
			settlement = Host.GetComponent<Settlement> ();
		}

		protected override bool IsFulfilled ()
		{
			return settlement.Population > TargetPopulation;
		}
	}

}

