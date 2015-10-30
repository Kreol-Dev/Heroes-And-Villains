
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
using System;
using System.Reflection;


namespace Demiurg 
{
	public class ArrayParam<T> : GlobalArrayParam<T> where T : class, new()
	{
		protected List<Entry> Entries;
		public ArrayParam (string name):base(name){}
		public override void GetItself (Table table)
		{
			Table listTable = table.Get<Table>(Name);
			base.GetItself(listTable);
		} 
	}

	public class GlobalArrayParam<T> : NodeParam<T[]> where T : class, new()
	{
		static List<FieldInfo> nodeParams = new List<FieldInfo>();
		static GlobalArrayParam()
		{
			Type t = typeof(T);
			Type nodeType = typeof(NodeParam);
			FieldInfo[] infos = t.GetFields(BindingFlags.Public);
			foreach ( var info in infos )
			{
				if (info.FieldType.IsSubclassOf(nodeType))
					nodeParams.Add(info);
			}
		}
		public GlobalArrayParam (string name):base(name){}
		public override void GetItself (Table table)
		{
			var list = table.GetArray();
			Content = new T[list.Count];
			for (int i = 0; i < list.Count; i++) {
				Content[i] = new T();
				var element = list [i];
				Table elemTable = element as Table;
				foreach (var param in nodeParams) {
					((NodeParam)param.GetValue(Content[i])).GetItself(elemTable);
				}
			}

		}
	}
}





