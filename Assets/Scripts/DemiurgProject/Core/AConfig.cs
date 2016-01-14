using UnityEngine;
using System.Collections;
using System;

namespace Demiurg.Core
{
    public class AConfig : Attribute
    {
        public object Name { get; internal set; }

        public AConfig (string name)
        {
            Name = name;
        }

        public AConfig (int id)
        {
            Name = id;
        }
    }
}

