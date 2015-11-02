
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
	public const string WiringTable = "wiring";
	LuaContext luaContext;
	ModsManager modsManager;
	protected override void CustomSetup ()
	{
		WorldCreator creator = new WorldCreator();

		

		luaContext = Find.Root<LuaContext>();
		luaContext.DeclareLibrary("Demiurg", new NameFuncPair[]{new NameFuncPair( "module_outputs", Outputs)});
		modsManager = Find.Root<ModsManager>();
		//luaContext.LoadScripts (modsManager.GetFiles("Demiurg\\" + WiringTable + ".lua"), WiringTable);
		luaContext.LoadScript("Mods\\CoreMod\\Demiurg\\Wiring\\Wiring.lua", WiringTable);
		Dictionary<string, Type> nodes = FindNodeTypes();


		creator.InitWiring(luaContext.GetTable(WiringTable), nodes);

		Fulfill.Dispatch();
	}

	Dictionary<string, Type> FindNodeTypes ()
	{
		Dictionary<string, Type> nodes = new Dictionary<string, Type>();
		Assembly asm = Assembly.GetExecutingAssembly();
		foreach ( var type in asm.GetTypes())
		{
			if (type.IsSubclassOf(typeof(CreationNode)) && !type.IsAbstract && !type.IsGenericType)
				nodes.Add(type.FullName, type);
		}
		return nodes;
	}


	int Outputs(ILuaState luaVM)
	{
//		string moduleName = luaVM.ToString(-1);
//		string moduleType = luaVM.ToString(-1);
//		luaVM.NewTable();//main table
//		List<string> outputs = CreationModule.GetOutputs(modules[moduleType]);
//		foreach (var output in outputs)
//		{
//			luaVM.PushString(output);
//			luaVM.NewTable();
//			luaVM.PushString("module");
//			luaVM.PushString(moduleName);
//			luaVM.SetTable(-3);
//			luaVM.PushString("module_type");
//			luaVM.PushString(moduleType);
//			luaVM.SetTable(-3);
//			luaVM.PushString("output");
//			luaVM.PushString(output);
//			luaVM.SetTable(-3);
//			luaVM.SetTable(-3);//Set main table
//		}
		return 1;
	}
}




