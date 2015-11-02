using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
using LuaTableEntries;

public class LuaContext : Root
{
	Scribe scribe;
	ILuaState luaVM;
	Dictionary<string, Table> tables;

	protected override void PreSetup ()
	{
		base.PreSetup ();
		luaVM = LuaAPI.NewState();
		scribe = Scribes.Register("LuaContext");
		tables = new Dictionary<string, Table>();
	}

	protected override void CustomSetup ()
	{
		Fulfill.Dispatch();
	}
	
	public void DeclareGlobalFunction(string name, CSharpFunctionDelegate function)
	{
		luaVM.PushCSharpFunction(function);
		luaVM.SetGlobal(name);
	}
	
	public void DeclareLibrary(string name, params NameFuncPair[] functions)
	{
		luaVM.L_RequireF(name, x => { x.L_NewLib(functions); return 1; }, true);
	}

	
	bool LoadFile(ILuaState targetVM, string path, string environment)
	{
		targetVM.L_LoadFile(path);
		luaVM.GetGlobal(environment);
		luaVM.SetUpvalue(2, 1);
		try
		{
			scribe.LogFormat("Opening: {0}", path);
			targetVM.Call(0, 0);
			return true;
		}
		catch
		{
			scribe.LogFormat("Error opening: {0}", path);
			targetVM.Pop(1);
			return false;
		}
	}
	
	public void LoadScripts(List<string> paths, string tableName)
	{
		for (int i = 0; i < paths.Count; i++)
			LoadScript(paths[i], tableName);
		if (paths.Count == 0)
			if (!tables.ContainsKey(tableName))
					tables.Add(tableName, new Table());
	}
	public void LoadScript(string path, string tableName)
	{
		Table table = null;
		if (tables.ContainsKey(tableName))
		{
			table = tables[tableName];
			
			scribe.LogFormat("Accessing table {0} with path {1}", tableName, path);
		}
		else
		{
			table = new Table();
			tables.Add(tableName, table);
			scribe.LogFormat("Creating table {0} with path {1}", tableName, path);
			luaVM.NewTable();
			luaVM.SetGlobal(tableName);
		}
		
		
		if (!LoadFile(luaVM, path, tableName))
		{
			scribe.LogFormat("Cant' open file at {0}", path);
			return;
		}
		table.Load(luaVM);
		
	}
	
	
	public Table GetTable(string name)
	{
		if (tables.ContainsKey(name))
			return tables[name];
		else
		{
			scribe.LogFormat("Attempted to get a table: {0}, while tables are: ", name);
			foreach ( var tableNode in tables)
				scribe.Log(tableNode.Key);
			return null;
		}
		
	}
	
	
	
}

