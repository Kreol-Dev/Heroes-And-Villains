using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public static class PhilosophyGenerator
	{
		public static Philosophy GeneratePhilosophy (Faction faction)
		{
			var phil = faction.gameObject.AddComponent<Philosophy> ();

			for (int i = 0; i < faction.ControlledStructures.Count; i++)
			{
				Philosophy structPhilosophy = faction.ControlledStructures [i].GetComponent<Philosophy> ();
				if (structPhilosophy == null)
					continue;
				//Somehow affect philosophy of a faction based on structure philosophy or whatever
			}


			return phil;
		}

	}


}
