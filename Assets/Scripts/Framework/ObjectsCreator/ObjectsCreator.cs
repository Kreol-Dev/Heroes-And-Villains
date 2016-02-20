using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Demiurg.Core.Extensions;
using System.Linq.Expressions;

[RootDependencies (typeof(ModsManager), typeof(MapRoot.Map), typeof(Sprites))]
public class ObjectsCreator : Root
{
	Dictionary<string, Dictionary<string, GameObject>> prototypes;

	Dictionary<string, Type> registeredCmps = new Dictionary<string, Type> ();

	List<Type> typesByID = new List<Type> ();

	protected override void CustomSetup ()
	{
		var modsManager = Find.Root<ModsManager> ();
		List<Type> components = new List<Type> ();
		foreach (var type in modsManager.GetAllTypes())
		{
			if (type.IsSubclassOf (typeof(EntityComponent)) && !type.IsAbstract && !type.IsGenericType)
				components.Add (type);
		}
		ITable componentsTable = modsManager.GetTable ("eComponent");
		int id = 0;
		foreach (var cmp in components)
		{
			componentsTable.Set (cmp.Name, id++);
			RegisterComponent (cmp);
		}
		modsManager.SetTableAsGlobal ("eComponent");
		Fulfill.Dispatch ();

	}

	public Type GetRegisteredType (string name)
	{
		Type type = null;
		registeredCmps.TryGetValue (name, out type);
		return type;
		
	}

	public Type GetTypeByID (int id)
	{
		if (typesByID.Count > id)
			return typesByID [id];
		return null;
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
		typesByID.Add (cmp);
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


	public abstract EntityComponent CopyTo (GameObject go);

	public abstract void PostCreate ();

}

public abstract class EntityComponent<TSharedData> : EntityComponent
{
	protected TSharedData SharedData { get; set; }


}
