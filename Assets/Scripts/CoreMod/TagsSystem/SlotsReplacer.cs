using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using System;
using Demiurg.Core;


namespace CoreMod
{
	public class SlotsReplacer : SlotsProcessor
	{
		[AInput ("available_tags")]
		Dictionary<string, List<Tag>> tags;
		[AInput ("available_replacers")]
		Dictionary<string, List<GameObject>> avaliableReplacers;
		[AConfig ("tags_namespace")]
		string tagsNamespace;
		[AConfig ("replacers_namespace")]
		string replacersNamespace;

		class Replacer
		{
			public GameObject GO;
			public Dictionary<Tag, int> Weights = new Dictionary<Tag, int> ();
			public TagsCollection Tags = new TagsCollection ();
		}

		class TagPair
		{
			public Tag Tag;
			public int Weight;
		}

		class TagRef
		{
			[AConfig (1)]
			public string TagName { get; set; }

			[AConfig (2)]
			public int Weight { get; set; }
		}

		class ReplacerData
		{
			[AConfig ("ref")]
			public string TypeName { get; set; }

			[AConfig ("tags")]
			public List<TagRef> Tags { get; set; }
		}

		[AConfig ("replacers")]
		List<ReplacerData> replacersData;
		List<Replacer> replacers;

		public override void Work ()
		{
			replacers = FormReplacers ();
			OutputObjects = new List<GameObject> ();
			foreach (var slotGO in InputObjects)
			{
				Slot slot = slotGO.GetComponent<Slot> ();
				slot.Replacer = Replacement (slot);
				SlotComponent[] components = slotGO.GetComponents<SlotComponent> ();

				for (int i = 0; i < components.Length; i++)
					components [i].FillComponent (slot.Replacer);

				slot.Replacer.SetActive (true);
				OutputObjects.Add (slot.Replacer);
			}
			FinishWork ();
		}

		List<Replacer> FormReplacers ()
		{
            
			Dictionary<string, Tag> tags = new Dictionary<string, Tag> ();
			if (this.tagsNamespace != "")
				foreach (var tag in this.tags[this.tagsNamespace])
					tags.Add (tag.Name, tag);
			Dictionary<string, GameObject> replacerGOs = new Dictionary<string, GameObject> ();
			foreach (var rep in this.avaliableReplacers[this.replacersNamespace])
				replacerGOs.Add (rep.name, rep);

			List<Replacer> replacers = new List<Replacer> ();
			foreach (var data in replacersData)
			{
				Replacer rep = new Replacer ();
				replacerGOs.TryGetValue (data.TypeName, out rep.GO);
				if (rep.GO == null)
				{
					continue;
				}
				foreach (var tagRef in data.Tags)
				{
					Tag tag = null;
					tags.TryGetValue (tagRef.TagName, out tag);
					if (tag == null)
					{
						continue;
					}
					rep.Tags.AddTag (tag);
					rep.Weights.Add (tag, tagRef.Weight);
				}
				replacers.Add (rep);
			}
			return replacers;
		}

		List<Replacer> possibleSlotReplacers = new List<Replacer> ();

		GameObject Replacement (Slot slot)
		{
			possibleSlotReplacers.Clear ();
			if (replacers.Count == 0)
				return null;
			int maxSimilarity = int.MinValue;
			int maxID = -1;
			for (int i = 0; i < replacers.Count; i++)
			{
				int sim = slot.Tags.ComputeSimilarity (replacers [i].Tags, replacers [i].Weights);
				if (sim > maxSimilarity)
				{
					possibleSlotReplacers.Clear ();
					maxSimilarity = sim;
					maxID = i;
					possibleSlotReplacers.Add (replacers [maxID]);
				} else if (sim == maxSimilarity)
				{
					possibleSlotReplacers.Add (replacers [i]);
				}
			}
			GameObject replacer = possibleSlotReplacers [Random.Next () % possibleSlotReplacers.Count].GO;
			slot.Similarity = maxSimilarity;
			GameObject go = new GameObject (replacer.name);
			foreach (var comp in replacer.GetComponents<EntityComponent>())
				comp.CopyTo (go);
			return go;


		}
	}
}


