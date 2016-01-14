using UnityEngine;
using System.Collections;
using System;


namespace Demiurg.Core
{
    public class AOutput : Attribute
    {
        public string Name { get; internal set; }

        public AOutput (string name)
        {
            Name = name;
        }
    }

}