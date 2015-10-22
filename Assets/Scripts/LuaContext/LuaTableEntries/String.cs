
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
namespace LuaTableEntries
{
	public class LuaString : Entry<string>
	{
		public LuaString(string value)
		{
			Content = value;
		}
		public override void Load (ILuaState luaVM)
		{
			Content = luaVM.ToString(-1);
		}
		
		public override string Serialize (int tabLevel = 0)
		{
			return string.Format("\"{0}\"", Content);
		}
	}
}
