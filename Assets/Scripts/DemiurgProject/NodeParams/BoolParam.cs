
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
            object o = table [Name];
            if (o != null)
                GetItselfFrom (o);
        }
        public override void GetItselfFrom (object o)
        {
            if (o is bool)
                Content = (bool)o;
            else
                Content = false;
        }
    }
}





