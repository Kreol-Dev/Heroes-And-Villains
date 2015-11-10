
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
namespace LuaTableEntries
{
    public abstract class Entry
    {
        public abstract void Load (ILuaState luaVM);
        public abstract string Serialize (int tabLevel = 0);
    }
    public abstract class Entry<T> : Entry
    {
        public T Content { get; set; }
        public static implicit operator T (Entry<T> value)
        {
            return value.Content;
        }
    }
}