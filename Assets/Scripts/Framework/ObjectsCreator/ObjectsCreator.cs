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

	public GameObject CreateObject (GameObject prototype)
	{
		GameObject obj = new GameObject ();
		foreach (var cmp in prototype.GetComponents<EntityComponent>())
			cmp.CopyTo (obj);
	}

	public GameObject GetPrototype (string packName, string name)
	{
		Dictionary<string, GameObject> pack = null;
		prototypes.TryGetValue (packName, out pack);
		if (pack == null)
			return null;
		GameObject prototype = null;
		pack.TryGetValue (name);
		return prototype;
	}

	public GameObject CreateObject (string packName, string name)
	{
		Dictionary<string, GameObject> pack = null;
		prototypes.TryGetValue (packName, out pack);
		if (pack == null)
			return null;
		GameObject prototype = null;
		pack.TryGetValue (name);
		if (prototype == null)
			return null;
		GameObject obj = new GameObject ();
		foreach (var cmp in prototype.GetComponents<EntityComponent>())
			cmp.CopyTo (obj);
		return obj;
	}


	public GameObject RegisterObject (string packName, string name, ITable fromTable)
	{
		Dictionary<string, GameObject> pack = null;
		prototypes.TryGetValue (packName, out pack);
		if (pack == null)
		{
			pack = new Dictionary<string, GameObject> ();
			prototypes.Add (packName, pack);
		}

		GameObject go = new GameObject (name);
		foreach (var cmpName in fromTable.GetKeys())
		{
			Type type = null;
			registeredCmps.TryGetValue (cmpName as string, out type);
			if (type == null)
				continue;
			EntityComponent cmp = go.AddComponent (type) as EntityComponent;
			cmp.LoadFromTable (fromTable.GetTable (cmpName));
		}
		pack.Add (name, go);
		go.SetActive (false);
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
		prototypes = new Dictionary<string, Dictionary<string, GameObject>> ();
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


public delegate void GenericSenderDelegate<T> (T sender);
public abstract class EntityComponent : MonoBehaviour
{
	public abstract void LoadFromTable (ITable table);

	public abstract void CopyTo (GameObject go);

	public abstract void PostCreate ();

	public event GenericSenderDelegate<EntityComponent> NotifyUpdate;

	bool Updated = false;

	protected void GotUpdated ()
	{
		Updated = true;
	}

	void Update ()
	{
		if (Updated)
		{
			Updated = false;
			NotifyUpdate (this);
		}
	}
}
