using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;


namespace CoreMod
{
	[ECompName ("settlement")]
	public class Settlement : EntityComponent
	{
		public override void CopyTo (GameObject go)
		{
			Settlement settlement = go.AddComponent<Settlement> ();
			settlement.Population = Population;
			settlement.Race = Race;
		}

		public int Population;
		public string Race;

		public override void LoadFromTable (ITable table)
		{
			Population = (int)(double)table.Get ("population");
			Race = (string)table.Get ("race");
		}
	}
}


