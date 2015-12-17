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
        Dictionary<string, Tag> tags;
        [AInput ("avaliable_replacers")]
        Dictionary<string, GameObject> avaliableReplacers;

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
            public string TagName;
            [AConfig (2)]
            public int Weight;
        }

        class ReplacerData
        {
            [AConfig ("ref")]
            public string TypeName;
            [AConfig ("tags")]
            public List<TagRef> Tags;
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
            List<Replacer> replacers = new List<Replacer> ();
            foreach (var data in replacersData)
            {
                Replacer rep = new Replacer ();
                avaliableReplacers.TryGetValue (data.TypeName, out rep.GO);
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

        GameObject Replacement (Slot slot)
        {
            int maxSimilarity = int.MinValue;
            int maxID = -1;
            for (int i = 0; i < replacers.Count; i++)
            {
                int sim = slot.Tags.ComputeSimilarity (replacers [i].Tags, replacers [i].Weights);
                if (sim > maxSimilarity)
                {
                    maxSimilarity = sim;
                    maxID = i;
                }
            }
            return UnityEngine.Object.Instantiate<GameObject> (replacers [maxID].GO);


        }
    }
}


