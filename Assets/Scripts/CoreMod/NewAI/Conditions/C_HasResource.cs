using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class C_HasResource : NewAI.Condition
	{
		public ResourceType Type;
		public City City;
		public int Resource;

		public override void Setup (GameObject target)
		{
			City = target.GetComponent<City> ();
			Type = null;
			Resource = -1;
		}

		public override NewAI.Task CreateTask (NewAI.Agent agent)
		{
			var task = new T_TradeFor ();
			task.Setup (City.gameObject.GetComponent<NewAI.Agent> (), this);
			return task;
		}

		public override GameObject TargetAgent {
			get
			{
				return City.gameObject;
			}
		}

		public override bool Satisfied {
			get
			{
				for (int i = 0; i < City.resources.Count; i++)
					if (City.resources [i].Type == Type && City.resources [i].Count >= Resource)
						return true;
				return false;
			}
		}



	}


}
