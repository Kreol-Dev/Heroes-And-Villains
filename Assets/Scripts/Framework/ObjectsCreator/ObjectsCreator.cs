using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Demiurg.Core.Extensions;
using System.Linq.Expressions;

[RootDependencies (typeof(ModsManager))]
public class ObjectsCreator : Root
{
	Dictionary<string, Dictionary<string, GameObject>> prototypes;

	Dictionary<string, Type> registeredCmps = new Dictionary<string, Type> ();

	protected override void CustomSetup ()
	{
		var modsManager = Find.Root<ModsManager> ();
		var asm = modsManager.GetModAssembly ("Core mod");
		List<Type> components = new List<Type> ();
		foreach (var type in asm.GetTypes())
		{
			if (type.IsSubclassOf (typeof(EntityComponent)) && !type.IsAbstract && !type.IsGenericType)
				components.Add (type);
		}
		foreach (var cmp in components)
		{
			RegisterComponent (cmp);
		}
		Fulfill.Dispatch ();

	}

	public GameObject CreateObject (string name, ITable fromTable)
	{
		GameObject go = new GameObject (name);
		foreach (var cmpName in fromTable.GetKeys())
		{
			Type type = null;
			registeredCmps.TryGetValue (cmpName as string, out type);
			if (type == null)
				continue;
			EntityComponent cmp = go.AddComponent (type) as EntityComponent;
			cmp.LoadFromTable (fromTable.GetTable (cmpName) as ITable);
		}
		return go;
	}

	Type eCompNameAttr = typeof(ECompName);

	void RegisterComponent (Type cmp)
	{
		if (!cmp.IsDefined (eCompNameAttr, false))
			return;
        
		ECompName nameAttr = cmp.GetCustomAttributes (eCompNameAttr, false) [0] as ECompName;
		Debug.Log ("Component registered: " + nameAttr.Name);
		registeredCmps.Add (nameAttr.Name, cmp);
	}

	protected override void PreSetup ()
	{
        
	}

}

public class ECompName : Attribute
{
	public string Name { get; internal set; }

	public ECompName (string name)
	{
		Name = name;
	}
}

public abstract class EntityComponent : MonoBehaviour
{
	public abstract void LoadFromTable (ITable table);


	public abstract void CopyTo (GameObject go);

	public abstract void PostCreate ();

}

public abstract class EntityComponent<TSharedData> : EntityComponent
{
	protected TSharedData SharedData { get; set; }


}
