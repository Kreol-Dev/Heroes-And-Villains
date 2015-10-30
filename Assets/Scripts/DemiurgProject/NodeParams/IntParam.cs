
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
namespace Demiurg 
{
	public class IntParam : NodeParam<int>
	{
		public IntParam (string name):base(name){}
		public override void GetItself (Table table)
		{
			Content = table.Get<Integer>(Name);
		}
	}
}





