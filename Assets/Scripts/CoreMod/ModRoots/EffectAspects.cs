using UnityEngine;
using System.Collections;
using System.Linq;
using System.Reflection;
using UIO;
using System;

namespace CoreMod
{
	[RootDependencies (typeof(ModsManager), typeof(StatesRoot))]
	public class StateApects : ModRoot
	{
		
		protected override void CustomSetup ()
		{
			var mm = Find.Root<ModsManager> ();
			var states = Find.Root<StatesRoot> ();
			var aspects = from type in mm.GetAllTypes ()
			              where type.IsSubclassOf (typeof(EffectAspect)) && !type.IsGenericType && !type.IsAbstract
			              select type;
			foreach (var aspect in aspects)
			{
				var stateField = aspect.GetProperty ("State", BindingFlags.NonPublic | BindingFlags.Static);
				if (stateField == null)
					continue;
				stateField.SetValue (null, states.GetStateObject (stateField.PropertyType), null);
			}
			Fulfill.Dispatch ();
		}
	}

	public class AspectAttribute : Attribute
	{
		public string Name { get; internal set; }

		public AspectAttribute (string name)
		{
			Name = name;			
		}
	}

	public abstract class EffectAspect
	{
		public abstract void ApplyTo (GameObject go);

		public abstract void Reverse (GameObject go);

		public abstract void LoadFrom (object key, ITable table);
	}

	public abstract class EffectAspect<C> : EffectAspect where C : class
	{
		public sealed override void ApplyTo (GameObject go)
		{
			C cmp = go.GetComponent<C> ();
			if (cmp != null)
				ApplyTo (cmp);
		}

		public sealed override void Reverse (GameObject go)
		{
			C cmp = go.GetComponent<C> ();
			if (cmp != null)
				Reverse (cmp);
		}

		public abstract void ApplyTo (C cmp);

		public abstract void Reverse (C cmp);
	}

	public abstract class StateEffectAspect<C, T, TS, S> : EffectAspect 
		where C : EntityComponent where S : EntityState<TS, C>
	{
		[Defined ("value", true)]
		public T Value;

		static protected EntityState<TS, C> State { get; private set; }
	}

	public abstract class IntAddAspect<C, S> : StateEffectAspect<C, int, int, S>
		where C : EntityComponent where S : IntState<C>
	{
		
		public override void ApplyTo (GameObject go)
		{
			((IntState<C>)State).Add (go.GetComponent<C> (), Value);
		}

		public override void Reverse (GameObject go)
		{
			((IntState<C>)State).Add (go.GetComponent<C> (), -Value);
		}
	}

	public abstract class FloatAddAspect<C, S> : StateEffectAspect<C, float, float, S>
		where C : EntityComponent where S : FloatState<C>
	{

		public override void ApplyTo (GameObject go)
		{
			((FloatState<C>)State).Add (go.GetComponent<C> (), Value);
		}

		public override void Reverse (GameObject go)
		{
			((FloatState<C>)State).Add (go.GetComponent<C> (), -Value);
		}
	}

	public abstract class FloatMulAspect<C, S> : StateEffectAspect<C, float, float, S>
		where C : EntityComponent where S : FloatState<C>
	{

		public override void ApplyTo (GameObject go)
		{
			((FloatState<C>)State).Mul (go.GetComponent<C> (), Value);
		}

		public override void Reverse (GameObject go)
		{
			((FloatState<C>)State).Mul (go.GetComponent<C> (), 1f / Value);
		}
	}

	public abstract class IntMulAspect<C, S> : StateEffectAspect<C, float, int, S>
		where C : EntityComponent where S : IntState<C>
	{

		public override void ApplyTo (GameObject go)
		{
			((IntState<C>)State).Mul (go.GetComponent<C> (), Value);
		}

		public override void Reverse (GameObject go)
		{
			((IntState<C>)State).Mul (go.GetComponent<C> (), 1f / Value);
		}
	}

	public abstract class SetAspect<C, T, S> : StateEffectAspect<C, T, T, S> 
		where T : class where C : EntityComponent where S : EntityState<T, C>
	{
		public override void ApplyTo (GameObject go)
		{
			State.Set (go.GetComponent<C> (), Value);
		}

		public override void Reverse (GameObject go)
		{
			State.Set (go.GetComponent<C> (), null);
		}
	}

	public abstract class BoolSetAspect<C, S> : StateEffectAspect<C, bool, bool, S>
		where C : EntityComponent where S : EntityState<bool, C>
	{
		public override void ApplyTo (GameObject go)
		{
			State.Set (go.GetComponent<C> (), Value);
		}

		public override void Reverse (GameObject go)
		{
			State.Set (go.GetComponent<C> (), !Value);
		}
	}
}

