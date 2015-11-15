using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MoonSharp.Interpreter;


namespace Demiurg
{
    [MoonSharpUserData]
    public class Slot : SlotComponent
    {
        public GameObject Replacer;
        [SerializeField]
        TagsCollection
            tags;
        public TagsCollection Tags { get; internal set; }
        void Awake ()
        {
            Tags = new TagsCollection ();
            tags = Tags;
            gameObject.AddComponent<CoreMod.TagsVisual> ();
        }
    }
    [MoonSharpUserData]
    public class SlotComponent : MonoBehaviour
    {
        public virtual void FillComponent (GameObject go)
        {

        }
    }

}

