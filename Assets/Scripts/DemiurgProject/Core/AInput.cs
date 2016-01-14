using UnityEngine;
using System.Collections;
using System;


namespace Demiurg.Core
{
    public class AInput : Attribute
    {
        public string Name { get; internal set; }

        public AInput (string name)
        {
            Name = name;
        }
    }
}
