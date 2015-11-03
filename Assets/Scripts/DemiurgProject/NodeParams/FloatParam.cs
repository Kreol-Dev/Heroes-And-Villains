
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
namespace Demiurg
{
    public class FloatParam : NodeParam<float>
    {
        public FloatParam (string name):base(name)
        {
        }
        public override void GetItself (Table table)
        {
            Content = (float)table [Name];
        }
    }
}





