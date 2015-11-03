
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
namespace Demiurg
{
    public class BoolParam : NodeParam<bool>
    {
        public BoolParam (string name):base(name)
        {
        }
        public override void GetItself (Table table)
        {
            Content = (bool)table [Name];
        }
    }
}





