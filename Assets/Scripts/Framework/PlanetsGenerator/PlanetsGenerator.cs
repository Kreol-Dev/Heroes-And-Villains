
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
[RootDependencies(typeof(LuaContext), typeof(ModsManager))]
public class PlanetsGenerator : Root
{
	public const string LayersTable = "Layers";
	public const string LayersWiringTable = "LayersWiring";
	public const string GenerationModulesTable = "GenerationModules";
	LuaContext luaContext;
	ModsManager modsManager;
	protected override void CustomSetup ()
	{
		luaContext = Find.Root<LuaContext>();
		modsManager = Find.Root<ModsManager>();
		luaContext.LoadScripts (modsManager.GetFiles("Demiurg\\" + LayersTable), LayersTable);
		luaContext.LoadScripts (modsManager.GetFiles("Demiurg\\" + LayersWiringTable), LayersWiringTable);
		luaContext.LoadScripts (modsManager.GetFiles("Demiurg\\" + GenerationModulesTable), GenerationModulesTable);
		WorldCreator creator = new WorldCreator();
		Fulfill.Dispatch();
	}

}




