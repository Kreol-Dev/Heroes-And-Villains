
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

            Content = (int)(double)table [Name];
            Debug.LogFormat ("{0} {1} {2}", Content, (double)table [Name], table [Name]);
        }
        public override void GetItselfFrom (object o)
        {
            Content = (int)(double)o;
        }
    }
}





