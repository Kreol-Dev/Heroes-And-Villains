
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using System;
using System.Reflection;
using UniLua;


[RootDependencies(typeof(LuaContext), typeof(ModsManager), typeof(ObjectsCreator))]
public class PlanetsGenerator : Root
{
	public const string WiringTable = "Wiring";
	LuaContext luaContext;
	ModsManager modsManager;
	Dictionary<string, Type> modules;
	protected override void CustomSetup ()
	{
		WorldCreator creator = new WorldCreator();

		modules = LoadModules();
		

		luaContext = Find.Root<LuaContext>();
		luaContext.DeclareLibrary("Demiurg", new NameFuncPair[]{new NameFuncPair( "module_outputs", Outputs)});
		modsManager = Find.Root<ModsManager>();
		luaContext.LoadScripts (modsManager.GetFiles("Demiurg\\" + WiringTable), WiringTable);



		//creator.InitWiring(luaContext.GetTable(WiringTable), modules);

		Fulfill.Dispatch();
	}

	Dictionary<string, Type> LoadModules ()
	{
		Dictionary<string, Type> modules = new Dictionary<string, Type>();
		Assembly asm = Assembly.GetExecutingAssembly();
		foreach (var type in asm.GetTypes ())
		{
			if (type.IsSubclassOf(typeof(CreationModule)))
			{
				modules.Add(type.Name, type);
			}
		}
		return modules;
	}

	int Outputs(ILuaState luaVM)
	{
		string moduleName = luaVM.ToString(-1);
		string moduleType = luaVM.ToString(-1);
		luaVM.NewTable();//main table
		List<string> outputs = CreationModule.GetOutputs(modules[moduleType]);
		foreach (var output in outputs)
		{
			luaVM.PushString(output);
			luaVM.NewTable();
			luaVM.PushString("module");
			luaVM.PushString(moduleName);
			luaVM.SetTable(-3);
			luaVM.PushString("module_type");
			luaVM.PushString(moduleType);
			luaVM.SetTable(-3);
			luaVM.PushString("output");
			luaVM.PushString(output);
			luaVM.SetTable(-3);
			luaVM.SetTable(-3);//Set main table
		}
		return 1;
	}
}




