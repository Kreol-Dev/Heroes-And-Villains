using UnityEngine;
using System.Collections;
using NewAI;
using System.Collections.Generic;

namespace CoreMod
{
	public class A_BuildBuilding : Action, J_BuildBuilding
	{

		float time;
		float timeDelta;
		BuildingType type;
		City city;
		List<C_HasResource> resourcesNeeded = new List<C_HasResource> ();
		bool canDo = false;
		Agent agent;

		public void Setup (Agent agent, CoreMod.BuildingType type)
		{ 
			this.agent = agent;
			this.type = type;
			time = type.BuildTime;
			timeDelta = Find.Root<AI.Ticker> ().TickDelta;
			city = agent.GetComponent<City> ();
		}

		public override void Update (System.Action onSuccess, System.Action onFail, Utilities uts)
		{
			if (!canDo)
			{
				canDo = true;
				for (int i = 0; i < resourcesNeeded.Count; i++)
				{
					var resC = resourcesNeeded [i];
					if (resC.Satisfied)
						continue;
					canDo = false;
					if (resC.AssignedTask == null)
						resC.AssignedTask = resC.CreateTask (agent);
					resC.AssignedTask.Do (agent, resC, uts);
				}
			} else
			{
				for (int i = 0; i < resourcesNeeded.Count; i++)
				{
					var resC = resourcesNeeded [i];
					if (resC.Satisfied)
						continue;
					onFail ();
					return;
				}
				time -= timeDelta;
				if (time <= 0)
				{
					city.buildings.Add (new Building (){ Type = type });
					for (int i = 0; i < type.Cost.Length; i++)
						city.FindRes (type.Cost [i].Type).Count -= type.Cost [i].Amount;

					onSuccess ();
				}
			}


		}

		public override float GetUtility (Utilities uts)
		{
			return 1f;
		}

		public override void Setup (Agent agent)
		{
			
		}

		public override void Prepare (int iteration)
		{
			canDo = false;
			resourcesNeeded.Clear ();
			for (int i = 0; i < type.Cost.Length; i++)
			{
				var res = city.resources.Find (r => r.Type == type.Cost [i].Type);
				C_HasResource resC = new C_HasResource ();
				resC.Setup (this.city.gameObject);
				resC.City = this.city;
				resC.Resource = type.Cost [i].Amount;
				resC.Type = type.Cost [i].Type;
				resourcesNeeded.Add (resC);
			}
		}

		public override bool IsPossibleToPerformBy (GameObject go)
		{
			return go.GetComponent<City> () != null;
		}



	}


}
