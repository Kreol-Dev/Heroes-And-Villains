using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using Demiurg.Core;

namespace CoreMod
{
    public class TagsAssigner : SlotsProcessor
    {

        [AInput ("tags")]
        Dictionary<string, List<Tag>> availableTags;
        [AConfig ("tags_namespace")]
        string tagsNamespace;
        List<Tag> tags;

        public override void Work ()
        {
            Debug.LogWarning ("ASSIGNING TAGS");
            tags = availableTags [tagsNamespace];
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

