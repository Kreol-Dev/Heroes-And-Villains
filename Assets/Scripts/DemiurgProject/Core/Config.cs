using UnityEngine;
using System.Collections;
using System;

namespace Demiurg.Core
{
    public class Config : Attribute
    {
        public string Name { get; internal set; }
        public Config (string name)
        {
            Name = name;
        }
    }
}

