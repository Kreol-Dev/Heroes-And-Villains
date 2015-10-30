
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
namespace Demiurg 
{
	public class BoolParam : NodeParam<bool>
	{
		public BoolParam (string name):base(name){}
		public override void GetItself (Table table)
		{
			Content = table.Get<Boolean>(Name);
		}
	}
}





