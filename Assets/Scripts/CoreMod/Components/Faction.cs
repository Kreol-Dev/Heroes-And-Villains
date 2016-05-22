using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class Faction : EntityComponent
	{
		Philosophy philosophy;
		Relations relations;

		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<Faction> ();
		}

		public override void PostCreate ()
		{
			if (Name == null)
				Name = NameGenerator.GenerateFactionName ();
			philosophy = GetComponent<Philosophy> ();
			if (philosophy == null)
				philosophy = PhilosophyGenerator.GeneratePhilosophy (this);
			relations = GetComponent<Relations> ();
		}

		protected override void PostDestroy ()
		{
			
		}


		public string Name;
		public List<GameObject> ControlledStructures = new List<GameObject> ();
		public List<GameObject> ControlledArmies = new List<GameObject> ();

	}
}


