using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public static class FactionGenerator
	{

		public static Faction GenerateFaction (GameObject host, List<GameObject> structures, List<GameObject> armies)
		{
			if (host == null)
				host = new GameObject ();
			var faction = host.AddComponent<Faction> ();
			faction.ControlledStructures.AddRange (structures);
			faction.ControlledArmies.AddRange (armies);
			return faction;
		}
	}
}


