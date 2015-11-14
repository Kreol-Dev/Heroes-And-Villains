using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;

namespace CoreMod
{
    public class TagsAssigner : SlotsProcessor
    {
        class TagRef
        {
            public StringParam TagName = new StringParam ("tag_name");
        }
        ArrayParam<TagRef> tagsRefs;
        protected override void SetupIOP ()
        {
            base.SetupIOP ();
            tagsRefs = Config<ArrayParam<TagRef>> ("tags");
        }
        protected override void Work ()
        {
            List<Tag> tags = FindTags ();
            Scribe.LogFormat ("[TAGS] GObjects {0}, Tags {1}", InputObjects.Content.Count, tags.Count);
            foreach (var go in InputObjects.Content)
            {
                for (int i = 0; i < tags.Count; i++)
                    if (tags [i].CheckSlot (go))
                    {
                        Slot slot = go.GetComponent<Slot> ();
                        if (slot == null)
                            slot = go.AddComponent<Slot> ();
                        slot.Tags.AddTag (tags [i]);
                    }
            }
        }
        
        
        List<Tag> FindTags ()
        {
            List<Tag> tags = new List<Tag> ();
            Scribe.LogFormat ("[TAGS] tag refs count: {0} ", tagsRefs.Content.Length);
            foreach (var tagRef in tagsRefs.Content)
            {
                Tag tag = Creator.FindTag (tagRef.TagName);
                if (tag != null)
                {
                    tags.Add (tag);
                    Scribe.LogFormat ("Found tag {0} for {1}", tagRef.TagName.Content, Name);
                }
                else
                    Scribe.LogFormatWarning ("Can't find tag {0} for {1}", tagRef.TagName.Content, Name);
            }
            return tags;
                
        }
    }

}

