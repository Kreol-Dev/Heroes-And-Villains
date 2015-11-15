using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using System;


namespace CoreMod
{
    public class SlotsReplacer : SlotsProcessor
    {
        class Replacer
        {
            public GameObject GO;
            public Dictionary<Tag, int> Weights = new Dictionary<Tag, int> ();
            public TagsCollection Tags = new TagsCollection ();
        }
        struct TagPair
        {
            public Tag Tag;
            public int Weight;
        }
        class TagRef
        {
            public StringParam TagName = new StringParam ("tag_name");
            public IntParam Weight = new IntParam ("weight");
        }
        class ReplacerData
        {
            public StringParam TypeName = new StringParam ("replacer");
            public ArrayParam<TagRef> Tags = new ArrayParam<TagRef> ("tags");
        }
        GlobalArrayParam<ReplacerData> replacersData;
        protected override void SetupIOP ()
        {
            base.SetupIOP ();
            replacersData = Config<GlobalArrayParam<ReplacerData>> ("replacers");
        }
        Replacer[] replacers;
        protected override void Work ()
        {
            Debug.LogWarning ("replacer works: " + Name);
            replacers = FormReplacers ();
            List<GameObject> gobjects = new List<GameObject> ();
            foreach (var slotGO in InputObjects.Content)
            {
                Slot slot = slotGO.GetComponent<Slot> ();
                slot.Replacer = Replacement (slot);
                SlotComponent[] components = slotGO.GetComponents<SlotComponent> ();
                for (int i = 0; i < components.Length; i++)
                    components [i].FillComponent (slot.Replacer);
                slot.Replacer.SetActive (true);
            }
        }

        Replacer[] FormReplacers ()
        {
            Replacer[] replacers = new Replacer[replacersData.Content.Length];
            for (int i = 0; i < replacers.Length; i++)
            {
                replacers [i] = new Replacer ();
                var replacer = replacers [i];
                GameObject gameObject = this.Creator.FindReplacer (replacersData.Content [i].TypeName);
                gameObject.SetActive (false);
                replacer.GO = gameObject;
                foreach (var tagRef in replacersData.Content[i].Tags.Content)
                {
                    Tag tag = this.Creator.FindTag (tagRef.TagName);
                    if (tag != null)
                    {
                        replacer.Tags.AddTag (tag);
                        replacer.Weights.Add (tag, tagRef.Weight);
                    }
                }
            }

            return replacers;
        }

        GameObject Replacement (Slot slot)
        {
            int maxSimilarity = int.MinValue;
            int maxID = -1;
            for (int i = 0; i < replacers.Length; i++)
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


