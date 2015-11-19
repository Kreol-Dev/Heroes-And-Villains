using UnityEngine;
using System.Collections;
using System;


namespace Demiurg.Core
{
    public class Input : Attribute
    {
        public string Name { get; internal set; }
        public Input (string name)
        {
            Name = name;
        }
    }
}
