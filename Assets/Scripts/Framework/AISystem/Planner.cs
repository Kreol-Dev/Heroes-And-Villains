using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
	public class Planner : MonoBehaviour
	{
		[SerializeField]
		float laziness;

		public float Laziness {
			get
			{
				return laziness;
			}
			set
			{
				if (value >= 1)
					laziness = 1;
				else if (value < 0)
					laziness = 0;
				else
					laziness = value;
			}
		}

		[SerializeField]
		float cautiousness;

		public float Cautiousness {
			get
			{
				return cautiousness;
			}
			set
			{
				if (value >= 1)
					cautiousness = 1;
				else if (value < 0)
					cautiousness = 0;
				else
					cautiousness = value;
			}
		}

		public Plan Plan (Goal goal)
		{
			Plan plan = new AI.Plan ();


			return plan;
		}

	}

	public class Plan
	{

		public List<BaseAction> Actions = new List<BaseAction> ();
	}

}

