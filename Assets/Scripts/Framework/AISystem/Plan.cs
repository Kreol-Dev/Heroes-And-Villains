using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
	public class Plan
	{
		public float Cost {
			get
			{
				float cost = 0f;
				for (int i = 0; i < Length; i++)
					cost += nodes [i].Cost;
				return cost;
			}
		}

		public int Length { get; internal set; }

		List<PlanningNode> nodes = new List<PlanningNode> ();

		public void Reset ()
		{
			Length = 0;
		}

		public PlanningNode this [int nodeID] {
			get
			{
				for (int i = 0; i <= nodeID - nodes.Count; i++)
					nodes.Add (new PlanningNode (nodes.Count));
				if (nodeID > Length)
					Length = nodeID + 1;		
				return nodes [nodeID];
			}
		}

	}

	public class PlanningNode
	{
		public List<Action> Actions { get; internal set; }

		public int ID { get; internal set; }

		public float Cost = 0f;

		public PlanningNode (int id)
		{
			ID = id;
			Actions = new List<Action> ();
		}

		public void Reset ()
		{
			Actions.Clear ();
		}

	}

}

