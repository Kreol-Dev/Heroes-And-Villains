
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
using UnityEditor;
using System.Reflection;
using System;


namespace Demiurg
{
	public abstract class CreationModule : ILuaTabled
	{
		Dictionary<string, CreationModule> modules;
		public void OtherModules(Dictionary<string, CreationModule> modules)
		{
			this.modules = modules;
		}
		protected virtual void GetParams(Table paramsTable){}
		protected abstract void Work();
		public void InitFrom (Table table)
		{
			GetParams(table.Get<Table>("params"));
			Table inputs = table.Get<Table>("inputs");
			foreach ( var input in inputs.GetNamedEntries())
			{
				Table inputRefTable = input.Value as Table;
				string inputName = input.Key;
				string outModule = inputRefTable.Get<LuaString>("module");
				string outputName = inputRefTable.Get<LuaString>("output");
				Connect(modules[outModule], outputName, inputName);
			}
		}
		public void Connect(CreationModule outputModule, string outputName, string inputName)
		{
			FieldInfo output = null;
			outputs.TryGetValue(outputName, out output);
			if (output == null)
				return;
			FieldInfo input = null;
			inputs.TryGetValue(outputName, out input);
			if (input == null)
				return;
			Input inputField = input.GetValue(this) as Input;
			inputField.ConnectTo(output.GetValue(outputModule) as Output);

		}
		public void LoadFrom (Table table)
		{
			throw new System.NotImplementedException();
		}
		public Table ToTable ()
		{
			throw new System.NotImplementedException();
		}

		static Dictionary<string, FieldInfo> inputs = new Dictionary<string, FieldInfo>();
		static Dictionary<string, FieldInfo> outputs = new Dictionary<string, FieldInfo>();
		public static void InitIOForModule(Type type)
		{
//			foreach( var field in type.GetFields())
//			{
//				if (field.FieldType.IsSubclassOf(typeof(Input)))
//					inputs.Add(field.Name, field);
//				else if (field.FieldType.IsSubclassOf(typeof(Output)))
//					outputs.Add(field.Name, field);
//			}
		}

		public static List<string> GetOutputs(Type type)
		{
			List<string> outputNames = new List<string>();
			foreach(var field in type.GetField("outputs", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<string, FieldInfo>)
				outputNames.Add(field.Key);
			return outputNames;
		}
	}


}



