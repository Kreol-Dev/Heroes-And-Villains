
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
namespace LuaTableEntries
{
	public class Integer : Entry<int>
	{
		public Integer(int value)
		{
			Content = value;
		}
		public override void Load (ILuaState luaVM)
		{
			Content = luaVM.ToInteger(-1);
		}
		
		public override string Serialize (int tabLevel = 0)
		{
			return Content.ToString();
		}
	}
}


