using UnityEngine;
using System.Collections;
using System;


namespace Demiurg.Core
{
    public class Output : Attribute
    {
        public string Name { get; internal set; }
        public Output (string name)
        {
            Name = name;
        }
    }

}