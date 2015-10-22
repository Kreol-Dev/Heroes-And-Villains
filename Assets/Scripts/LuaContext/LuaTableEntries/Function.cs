
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using UniLua;
namespace LuaTableEntries
{
	public class Function : Entry
	{
		ILuaState luaVM;
		int functionRef;
		public Function(ILuaState luaVM) { this.luaVM = luaVM; }
		public override void Load (ILuaState luaVM)
		{
			functionRef = luaVM.L_Ref( LuaDef.LUA_REGISTRYINDEX );
		}
		public void Call(int argumentsCount = 0, int returnsCount  = 0)
		{
			luaVM.RawGetI( LuaDef.LUA_REGISTRYINDEX, functionRef );
			var status = luaVM.PCall( argumentsCount, returnsCount, 0 );
			if( status != ThreadStatus.LUA_OK )
			{
				Debug.LogError( luaVM.ToString(-1) );
			}
			luaVM.Pop(1);
		}
		public override string Serialize (int tabLevel = 0)
		{
			return null;
		}
	}
}


