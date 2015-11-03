
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
namespace Demiurg
{
    public class StringParam : NodeParam<string>
    {
        public StringParam (string name):base(name)
        {
        }
        public override void GetItself (Table table)
        {
            Content = (string)table [Name];
        }
		
    }
}





