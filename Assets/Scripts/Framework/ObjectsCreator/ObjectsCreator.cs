
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
using System;
using Demiurg;
using System.Reflection;

public enum CreationWay
{
    Load,
    Init
}
[RootDependencies(typeof(ModsManager))]
public class ObjectsCreator : Root
{
    ModsManager manager;
    Scribe scribe = Scribes.Register ("ObjectsCreator");
    protected override void CustomSetup ()
    {
        manager = Find.Root<ModsManager> ();

        Fulfill.Dispatch ();
    }

    public T CreateFromTypedTable<T> (Table table, CreationWay way) where T : class, ILuaTabled
    {
        Type assumedType = typeof(T);
        Type actualType = manager.ResolveType (table.Get<LuaString> ("ObjectRef"));
        if (!actualType.IsSubclassOf (assumedType))
        {
            scribe.LogFormat ("{0} is not a subclass of {1}", actualType, assumedType);
            return null;
        }
			
        T t = null;
        if (assumedType.IsSubclassOf (typeof(MonoBehaviour)))
        {
            GameObject go = new GameObject ();
            t = go.AddComponent (actualType) as T;
        }
        else
            t = Activator.CreateInstance (actualType) as T;
        switch (way)
        {
        case CreationWay.Init:
            t.InitFrom (table);
            break;
        case CreationWay.Load:
            t.LoadFrom (table);
            break;
        }
        return t;

    }
}




