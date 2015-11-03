
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
using System.Text;
namespace LuaTableEntries
{
	public class Table : Entry
	{
		Dictionary<string, Entry> entries = new Dictionary<string, Entry>();
		List<string> keys = new List<string>();
		List<Entry> array = new List<Entry>();

		#region Load
		public Dictionary<string, Entry> GetNamedEntries()
		{
			return entries;
		}
		public List<Entry> GetAllEntries()
		{
			List<Entry> list = new List<Entry>();
			for (int i = 0; i < keys.Count; i++)
			{
				string key = keys[i];
				list.Add(entries[key]);
			}
			list.AddRange(array);
			return list;
		}
		public List<Entry> GetArray()
		{
			return array;
		}
		public T Get<T>(string name) where T : Entry
		{
			if (entries.ContainsKey(name))
				return entries[name] as T;
			return null;
		}

		public T Get<T>(int id) where T : Entry
		{
			if (array.Count > id && id >= 0)
				return array[id] as T;
			return null;
		}
		

		public override void Load (ILuaState luaVM)
		{
			luaVM.PushNil();
			while(luaVM.Next(-2))
			{
				Entry entry = null;
				if (luaVM.IsString(-1))
					entry = new LuaString(luaVM.ToString(-1));
				else if (luaVM.IsNumber(-1))
					entry = new Float((float)luaVM.ToNumber(-1));
				else if (luaVM.IsTable(-1))
					entry = new Table();
				else if (luaVM.IsFunction(-1))
					entry = new Function(luaVM);
				else if (luaVM.IsBoolean(-1))
					entry = new Boolean(luaVM.ToBoolean(-1));

				if (luaVM.IsNumber(-2))
					Add (entry);
				else
					Set (luaVM.ToString(-2), entry);
				entry.Load(luaVM);				
				luaVM.Pop(1);
			}
		}
		#endregion

		#region Save
		public void Set (string name, Entry entry)
		{
			if (!entries.ContainsKey(name))
			{
				keys.Add(name);
				entries.Add(name, entry);
			}				
			else
				entries[name] = entry;
		}
		
		public void Set (int id, Entry entry)
		{
			if (array.Count > id && id >= 0)
			{
				array[id] = entry;
			}				
			else
				array.Add(entry);
		}

		public void Add(Entry entry)
		{
			array.Add(entry);
		}

		public override string Serialize (int tabLevel = 0)
		{
			StringBuilder builder = new StringBuilder(entries.Count * (30 + tabLevel));

			builder.Append("{\n");
			
			for (int i = 0; i < keys.Count; i++)
			{
				string key = keys[i];
				builder.Append(' ', tabLevel + 1);
				builder.Append(key);
				builder.Append(" = ");
				builder.Append(entries[key].Serialize(tabLevel + 1));
				builder.Append(",\n");
			}
			
			builder.Append(' ', tabLevel);
			builder.Append("}");
			return builder.ToString();
		}
		#endregion
	}
}
