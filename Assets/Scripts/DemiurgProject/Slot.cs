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
        [SerializeField]
        TagsCollection
            tags;
        public TagsCollection Tags { get; internal set; }
        void Awake ()
        {
            Tags = new TagsCollection ();
            tags = Tags;
        }
    }
    [MoonSharpUserData]
    public class SlotComponent : MonoBehaviour
    {
    }

}

