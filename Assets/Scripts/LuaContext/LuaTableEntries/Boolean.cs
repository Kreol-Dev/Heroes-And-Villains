
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
namespace LuaTableEntries
{
	public class Boolean: Entry<bool>
	{
		public Boolean(bool value)
		{
			Content = value;
		}
		public override void Load (ILuaState luaVM)
		{
			Content = luaVM.ToBoolean(-1);
		}
		
		public override string Serialize (int tabLevel = 0)
		{
			return Content.ToString();
		}
	}
}

