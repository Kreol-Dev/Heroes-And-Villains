using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using Demiurg.Core;
using System.Linq;

namespace CoreMod
{
	public class TagsAssigner : SlotsProcessor
	{
		IEnumerable<Tag> tags;

		public override void Work ()
		{
			Debug.LogWarning ("ASSIGNING TAGS");
			tags = Find.Root<TagsRoot> ().GetAllTags ();
			foreach (var go in InputObjects)
			{
                
				Slot slot = go.GetComponent<Slot> ();

				if (slot == null)
					slot = go.AddComponent<Slot> ();
				foreach (var tag in tags)
				{
					if (tag.CheckSlot (go))
						slot.Tags.AddTag (tag);
				}
			}
			OutputObjects = InputObjects;
			FinishWork ();
		}


	}

}

