
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core;
using System;
using System.Reflection;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using DemiurgBinding;
using Demiurg.Core.Extensions;

[RootDependencies (typeof(LuaContext), typeof(ModsManager), typeof(Sprites))]
public class PlanetsGenerator : Root
{
    Scribe scribe = Scribes.Find ("PlanetsGenerator");
    LuaContext luaContext;
    ModsManager modsManager;
    //WorldCreator creator;
    ITable wiring;

    protected override void CustomSetup ()
    {
        /*creator = new WorldCreator ();

		

        luaContext = Find.Root<LuaContext> ();
        //luaContext.DeclareLibrary("Demiurg", new NameFuncPair[]{new NameFuncPair( "module_outputs", Outputs)});
        modsManager = Find.Root<ModsManager> ();
        //luaContext.LoadScripts (modsManager.GetFiles("Demiurg\\" + WiringTable + ".lua"), WiringTable);
        //luaContext.LoadScript ("Mods\\CoreMod\\Demiurg\\Wiring\\Wiring.lua", WiringTable);
        Dictionary<string, Type> nodes = FindNodeTypes ();

        Script script = new Script ();
        RegisterSlotComponents (script);
        script.Globals ["wiring"] = new Table (script);
        script.Globals ["replacers"] = new Table (script);
        script.Globals ["tags"] = new Table (script);
        script.Globals ["tag_expressions"] = new Table (script);
        (script.Globals ["tag_expressions"] as Table) ["component"] = script.Globals ["component"];
        script.Options.ScriptLoader = new FileSystemScriptLoader ();
        script.DoFile ("Mods\\CoreMod\\Demiurg\\TagExpressions\\Expressions.lua", script.Globals ["tag_expressions"] as Table);
        script.DoFile ("Mods\\CoreMod\\Demiurg\\Replacers\\Replacers.lua", script.Globals ["replacers"] as Table);
        (script.Globals ["tags"] as Table) ["tag_expressions"] = script.Globals ["tag_expressions"];
        script.DoFile ("Mods\\CoreMod\\Demiurg\\Tags\\Tags.lua", script.Globals ["tags"] as Table);
        script.DoFile ("Mods\\CoreMod\\Demiurg\\Wiring\\Wiring.lua", script.Globals ["wiring"] as Table);


        //script.Globals.Values
        Dictionary<string, Table> tables = new Dictionary<string, Table> ();
        foreach (var pair in ((Table)script.Globals ["wiring"]).Pairs)
        {
            Debug.Log (pair.Key.CastToString ());
            tables.Add (pair.Key.CastToString (), pair.Value.Table);
        }
        creator.SetupReplacers (FormReplacers (script.Globals ["replacers"] as Table));
        creator.SetupTags (FormTags (script.Globals ["tags"] as Table));
        creator.InitWiring (tables, nodes);*/

        wiring = Find.Root<ModsManager> ().GetTable ("wiring");


        DemiurgEntity dem = new DemiurgEntity (FindAvatarTypes (), PrepareAvatarsTables (), LoadConverters (), LoadLoaders ());
        Fulfill.Dispatch ();
    }

    Dictionary<string, Type> FindAvatarTypes ()
    {
        Dictionary<string, Type> nodes = new Dictionary<string, Type> ();
        Assembly asm = Assembly.GetExecutingAssembly ();
        foreach (var type in asm.GetTypes())
        {
            if (type.IsSubclassOf (typeof(Demiurg.Core.Avatar)) && !type.IsAbstract && !type.IsGenericType)
                nodes.Add (type.FullName, type);
        }
        return nodes;
    }

    Dictionary<string, ITable> PrepareAvatarsTables ()
    {
        Dictionary<string, Demiurg.Core.Extensions.ITable> tables = new Dictionary<string, Demiurg.Core.Extensions.ITable> ();
        foreach (var avatarKey in wiring.GetKeys())
        {
            
            ITable avatarTable = wiring.Get (avatarKey) as ITable;
            if (avatarTable != null)
            if (avatarTable.Get ("global") == null || ((bool)avatarTable.Get ("global")) != true)
            {
                Debug.Log (avatarKey.GetType ());
                tables.Add ((string)avatarKey, avatarTable);
                scribe.LogFormat ("{0} has been added ", avatarKey);
            }
                
        }
        return tables;
    }

    List<Demiurg.Core.Extensions.IConverter> LoadConverters ()
    {
        List<IConverter> convs = new List<IConverter> ();
        Assembly asm = Assembly.GetExecutingAssembly ();
        Type convType = typeof(IConfigLoader);
        foreach (var type in asm.GetTypes())
        {
            if (convType.IsAssignableFrom (type) && !type.IsAbstract && !type.IsGenericType)
                convs.Add (Activator.CreateInstance (type) as IConverter);
        }
        return convs;
    }

    List<Demiurg.Core.Extensions.IConfigLoader> LoadLoaders ()
    {
        List<IConfigLoader> loads = new List<IConfigLoader> ();
        Assembly asm = Assembly.GetExecutingAssembly ();
        Type loaderType = typeof(IConfigLoader);
        foreach (var type in asm.GetTypes())
        {
            if (loaderType.IsAssignableFrom (type) && !type.IsAbstract && !type.IsGenericType)
                loads.Add (Activator.CreateInstance (type) as IConfigLoader);
        }
        return loads;
    }
    /*
    Dictionary<string, Type> FindNodeTypes ()
    {
        
    }

    public Dictionary<string, Tag> FormTags (Table tagsTable)
    {
        Dictionary<string, Tag> tags = new Dictionary<string, Tag> ();
        foreach (var entry in tagsTable.Pairs)
        {
            if (entry.Value.Table ["expression"] == null)
                continue;
            Tag tag = new Tag (entry.Key.ToPrintString (), tags.Count, entry.Value.Table ["expression"] as Closure, entry.Value.Table ["criteria"] as Table);
            Debug.LogWarning (entry.Key.ToPrintString ());
            tags.Add (entry.Key.ToPrintString (), tag);
        }
        return tags;
    }

    public Dictionary<string, GameObject> FormReplacers (Table replacersTable)
    {
        Dictionary<string, GameObject> gos = new Dictionary<string, GameObject> ();
        foreach (var entry in replacersTable.Pairs)
        {
            GameObject go = new GameObject ("prototype replacer " + entry.Key.ToPrintString ());
            go.SetActive (false);
            foreach (var compEntry in entry.Value.Table.Pairs)
            {
                string compName = compEntry.Key.ToPrintString ();
                if (compName == "graphics")
                    go.AddComponent<CoreMod.EntityGraphics> ().LoadFromTable (entry.Value.Table);
                else
                if (compName == "settlement")
                    go.AddComponent<CoreMod.Settlement> ().LoadFromTable (entry.Value.Table);
            }
            gos.Add (entry.Key.ToPrintString (), go);
        }
        return gos;
    }*/

    void RegisterSlotComponents (BindingTable table)
    {
        /*script.Globals ["component"] = new Table (script);
        Table table = script.Globals ["component"] as Table;
        Type[] types = Assembly.GetExecutingAssembly ().GetTypes ();
        List<Type> slotComponents = new List<Type> ();
        Type slotComponentType = typeof(SlotComponent);
        UserData.RegisterType (typeof(SlotComponentsProvider));
        for (int i = 0; i < types.Length; i++)
            if (types [i].IsSubclassOf (slotComponentType))
            {
                UserData.RegisterType (types [i]);
                table [types [i].Name] = slotComponents.Count;
                slotComponents.Add (types [i]);
            }
                
        SlotComponentsProvider.Types = slotComponents;*/

    }
}




