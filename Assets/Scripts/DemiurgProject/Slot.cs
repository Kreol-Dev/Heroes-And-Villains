using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Slot : MonoBehaviour
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

