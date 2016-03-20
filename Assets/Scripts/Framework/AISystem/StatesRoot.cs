using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

[RootDependencies (typeof(ModsManager))]
public class StatesRoot : Root
{

	Dictionary<Type, object> states = new Dictionary<Type, object> ();

	protected override void CustomSetup ()
	{
		var types = Find.Root<ModsManager> ().GetAllTypes ();
		var stateTypes = from type in types
		                 where type.IsSubclassOf (typeof(EntityState<,>))
		                 select type;
		foreach (var stateType in stateTypes)
		{
			states.Add (stateType, Activator.CreateInstance (stateType));
		}
		Fulfill.Dispatch ();
	}

	public T GetState<T> () where T : class
	{
		object state = null;
		states.TryGetValue (typeof(T), out state);
		return state as T;
	}

	public object GetStateObject (Type type)
	{
		object state = null;
		states.TryGetValue (type, out state);
		return state;
	}

	public IEnumerable<Type> GetStateTypesFromObject (object obj)
	{
		var fields = obj.GetType ().GetFields (System.Reflection.BindingFlags.NonPublic);
		var statesFields = from field in fields
		                   where field.FieldType.IsSubclassOf (typeof(EntityState<,>))
		                   select field.FieldType;
		return statesFields;
	}

	public IEnumerable GetStatesFromObject (object obj)
	{
		var types = GetStateTypesFromObject (obj);
		var states = from type in types
		             select this.GetStateObject (type);
		return states;
	}

	protected override void PreSetup ()
	{
		base.PreSetup ();
	}


}

public abstract class EntityState<T, C> where C : Component
{
	public abstract T Get (C cmp);

	public abstract void Set (C cmp, T value);

	public bool Has (GameObject go)
	{
		return go.GetComponent<C> () != null;
	}
}


public abstract class IntState<C> : EntityState<int, C> where C : Component
{
	public abstract void Add (C cmp, int value);

	public abstract void Mul (C cmp, float value);
}

public abstract class FloatState<C> : EntityState<float, C> where C : MonoBehaviour
{
	public abstract void Add (C cmp, float value);

	public abstract void Mul (C cmp, float value);
}
