
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
namespace LuaTableEntries
{
    public class Float : Entry<float>
    {
        public Float (float value)
        {
            Content = value;
        }
        public override void Load (ILuaState luaVM)
        {
            Content = (float)luaVM.ToNumber (-1);
        }
		
        public override string Serialize (int tabLevel = 0)
        {
            return Content.ToString ();
        }
    }
}

