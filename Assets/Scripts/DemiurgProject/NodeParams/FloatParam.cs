
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
namespace Demiurg 
{
	public class FloatParam : NodeParam<float>
	{
		public FloatParam (string name):base(name){}
		public override void GetItself (Table table)
		{
			Content = table.Get<Float>(Name);
		}
	}
}





