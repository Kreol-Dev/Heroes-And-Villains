
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
namespace Demiurg
{
    public class IntParam : NodeParam<int>
    {
        public IntParam (string name):base(name)
        {
        }
        public override void GetItself (Table table)
        {
            Debug.LogFormat ("{0} {1}", Name, table [Name]);
            Content = (int)(double)table [Name];
        }
    }
}





