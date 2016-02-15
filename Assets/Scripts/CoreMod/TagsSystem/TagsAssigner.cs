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

		[AConfig ("tags_namespaces")]
		List<string> tagsNamespaces;
		List<Tag> tags;

		public override void Work ()
		{
			Debug.LogWarning ("ASSIGNING TAGS");
			tags = new List<Tag> ();
			foreach (var name in tagsNamespaces)
				tags.AddRange (from pair in Find.Root<TagsRoot> ().GetTags (name)
				               select pair.Value);
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

