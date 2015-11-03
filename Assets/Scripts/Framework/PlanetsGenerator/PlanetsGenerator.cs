
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using System;
using System.Reflection;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

[RootDependencies(typeof(LuaContext), typeof(ModsManager), typeof(ObjectsCreator))]
public class PlanetsGenerator : Root
{
    public const string WiringTable = "wiring";
    LuaContext luaContext;
    ModsManager modsManager;
    WorldCreator creator;
    protected override void CustomSetup ()
    {
        creator = new WorldCreator ();

		

        luaContext = Find.Root<LuaContext> ();
        //luaContext.DeclareLibrary("Demiurg", new NameFuncPair[]{new NameFuncPair( "module_outputs", Outputs)});
        modsManager = Find.Root<ModsManager> ();
        //luaContext.LoadScripts (modsManager.GetFiles("Demiurg\\" + WiringTable + ".lua"), WiringTable);
        //luaContext.LoadScript ("Mods\\CoreMod\\Demiurg\\Wiring\\Wiring.lua", WiringTable);
        Dictionary<string, Type> nodes = FindNodeTypes ();
        Script script = new Script ();
        
        Table context = new Table (script);
        script.Options.ScriptLoader = new FileSystemScriptLoader ();
        script.DoFile ("Mods\\CoreMod\\Demiurg\\Wiring\\Wiring.lua", context);
        //script.Globals.Values
        Dictionary<string, Table> tables = new Dictionary<string, Table> ();
        foreach (var pair in context.Pairs)
        {
            Debug.Log (pair.Key.CastToString ());
            tables.Add (pair.Key.CastToString (), pair.Value.Table);
        }

        creator.InitWiring (tables, nodes);

        Fulfill.Dispatch ();
    }

    Dictionary<string, Type> FindNodeTypes ()
    {
        Dictionary<string, Type> nodes = new Dictionary<string, Type> ();
        Assembly asm = Assembly.GetExecutingAssembly ();
        foreach (var type in asm.GetTypes())
        {
            if (type.IsSubclassOf (typeof(CreationNode)) && !type.IsAbstract && !type.IsGenericType)
                nodes.Add (type.FullName, type);
        }
        return nodes;
    }

 
}




