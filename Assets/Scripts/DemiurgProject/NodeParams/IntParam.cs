
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
            object o = table [Name];
            GetItselfFrom (o);
        }
        public override void GetItselfFrom (object o)
        {
            if (o == null)
                Undefined = true;
            else
                Content = (int)(double)o;
        }
    }
}





