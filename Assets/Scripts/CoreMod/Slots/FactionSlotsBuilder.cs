using UnityEngine;
using System.Collections;
using Demiurg;
using Demiurg.Core;
using System.Collections.Generic;
using System.Linq;

namespace CoreMod
{
	public class FactionSlotsBuilder : SlotsProcessor
	{
		[AInput ("settlements")]
		List<GameObject> towns;

		public override void Work()
		{
			IEnumerable<GameObject> rand_towns = towns.OrderBy (x => Random.Next ());			
			IEnumerator towns_numerator = rand_towns.GetEnumerator ();

			foreach (var go in InputObjects)
			{				
				FactionSlot faction = go.AddComponent<FactionSlot> ();
				faction.Ownership.Add ((GameObject)towns_numerator.Current);
				towns_numerator.MoveNext ();
			}
			OutputObjects = InputObjects;
			FinishWork ();
		}
	}
}

