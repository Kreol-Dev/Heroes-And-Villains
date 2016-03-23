using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;
using System;
using System.Linq;

namespace CoreMod
{
	[RootDependencies (typeof(ModsManager))]
	public class Modifiers : ModRoot
	{
		Scribe scribe;

		public delegate void ModifierDelegate (object target, Modifier mod);

		public event ModifierDelegate ModifierAdded;
		public event ModifierDelegate ModifierRemoved;

		Dictionary<string, Type> aspectsByName = new Dictionary<string, Type> ();
		Dictionary<Type, Type> modifierTypeByAspects = new Dictionary<Type, Type> ();
		Dictionary<object, HashSet<Modifier>> modifiersPerObject = new Dictionary<object, HashSet<Modifier>> ();

		Dictionary<string, Modifier> modifiersByName = new Dictionary<string, Modifier> ();

		protected override void CustomSetup ()
		{
			
			scribe = Scribes.Find ("MODIFIERS");
			var mm = Find.Root<ModsManager> ();
			Type genericModifier = typeof(Modifier<>);
			var aspectsTypes = from type in mm.GetAllTypes ()
			                   let attrs = type.GetCustomAttributes (typeof(AspectAttribute), false)
			                   where attrs.Length > 0
			                   select new KeyValuePair<string, Type> ((attrs [0] as AspectAttribute).Name, type);
			foreach (var aType in aspectsTypes)
			{
				aspectsByName.Add (aType.Key, aType.Value);
				var cmpType = aType.Value.BaseType.GetGenericArguments () [0];
				modifierTypeByAspects.Add (aType.Value, genericModifier.MakeGenericType (cmpType));
			}

			var modifiersTable = mm.GetTable ("entities_modifiers");
			foreach (var modNamespaceKey in modifiersTable.GetKeys())
			{
				if (mm.IsTechnical (modifiersTable, modNamespaceKey))
					continue;
				var modsTable = modifiersTable.GetTable (modNamespaceKey);
				foreach (var modNameKey in modsTable.GetKeys())
				{
					
					string modName = modNamespaceKey.ToString () + '.' + modNameKey.ToString ();
					Type modType = null;
					List<EffectAspect> aspects = new List<EffectAspect> ();
					var aspectsTable = modsTable.GetTable (modNameKey);
					foreach (var aspectName in aspectsTable.GetKeys())
					{
					
						Type aspectType = null;
						if (!aspectsByName.TryGetValue (aspectName as string, out aspectType))
						{
							scribe.LogFormatWarning ("Can't find aspect with the name {0}", aspectName);
							continue;
						}
						Type aspectModType = null;
						if (!modifierTypeByAspects.TryGetValue (aspectType, out aspectModType))
						{
							scribe.LogFormatWarning ("Can't find modifier with the aspect {0}", aspectName);
							continue;
						}
						if (modType != null && aspectModType != modType)
						{
							continue;
						} else
							modType = aspectModType;
						EffectAspect aspect = Activator.CreateInstance (aspectType) as EffectAspect;
						aspect.LoadFrom (aspectName, modsTable);
						aspects.Add (aspect);
					}
				
					Modifier mod = Activator.CreateInstance (modType) as Modifier;
					mod.Setup (aspects);
					modifiersByName.Add (modName, mod);
				}

			}
			Fulfill.Dispatch ();
		}

		protected override void PreSetup ()
		{
			base.PreSetup ();
		}

		public bool AttachModifier (IDestroyable to, string modName)
		{
			Modifier mod = GetModifier (modName);
			if (mod == null)
			{
				scribe.LogFormatWarning ("Couldn't been able to add {0} modifier to {1}, there is no such modifier", modName, to);
				return false;
			}
			if (!AttachModifier (to, mod))
			{
				scribe.LogFormatWarning ("Couldn't been able to add {0} modifier to {1}, as its already got one", modName, to);
				return false;
			}
			return true;
			
		}

		void OnObjectDestroyed (object obj)
		{
			IDestroyable desObj = obj as IDestroyable;
			desObj.Destroyed -= OnObjectDestroyed;
			var mods = modifiersPerObject [obj];
			foreach (var mod in mods)
			{
				mod.DetachFrom (obj);
			}

			modifiersPerObject.Remove (obj);

		}

		public bool AttachModifier (IDestroyable to, Modifier mod)
		{
			HashSet<Modifier> modsOfObject = null;
			if (!modifiersPerObject.TryGetValue (to, out modsOfObject))
			{
				modsOfObject = new HashSet<Modifier> ();
				to.Destroyed += OnObjectDestroyed;
				modifiersPerObject.Add (to, modsOfObject);
			}
			if (modsOfObject.Add (mod))
			{
				mod.AttachTo (to);
				return true;
			}
			return false;
		}

		public bool DetachModifier (object from, string name)
		{
			Modifier mod = GetModifier (name);
			if (mod == null)
			{
				scribe.LogFormatWarning ("Couldn't been able to detach modifier {0} from {1}, there is no such modifier", name, from);
				return false;
			}
			if (!DetachModifier (from, mod))
			{
				scribe.LogFormatWarning ("Couldn't been able to detach modifier {0} from {1}, no such modifier attached", name, from);
				return false;
			}
			return true;
		}

		public bool DetachModifier (object from, Modifier mod)
		{
			HashSet<Modifier> modsOfObject = null;
			if (!modifiersPerObject.TryGetValue (from, out modsOfObject))
				return false;
			if (modsOfObject.Remove (mod))
			{
				mod.DetachFrom (from);
				return true;
			}
			return false;
		}

		public Modifier GetModifier (string modName)
		{
			Modifier mod = null;
			modifiersByName.TryGetValue (modName, out mod);
			return mod;
		}

		static List<Modifier> nullList = new List<Modifier> ();

		public IEnumerable<Modifier> GetModifiers (object of)
		{
			HashSet<Modifier> mods = null;
			modifiersPerObject.TryGetValue (of, out mods);
			if (mods == null)
				return nullList;
			return mods;
		}
			
	}

	public class ModifierConverter : IConverter<Modifier>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			if (reference)
			{
				var modifierName = table.GetString (key);
				return Find.Root<Modifiers> ().GetModifier (modifierName);
			}
			return null;
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}

	public interface IDestroyable
	{
		event ObjectDelegate<object> Destroyed;
	}

	public abstract class Modifier
	{
		public abstract void Setup (List<EffectAspect> aspects);

		public abstract void AttachTo (object obj);

		public abstract void DetachFrom (object obj);
	}


	public class Modifier<T> : Modifier where T : class
	{
		List<EffectAspect<T>> effects = new List<EffectAspect<T>> ();

		public override void Setup (List<EffectAspect> aspects)
		{
			effects.Clear ();
			for (int i = 0; i < aspects.Count; i++)
			{
				effects.Add (aspects [i] as EffectAspect<T>);
			}
		}

		public override void AttachTo (object obj)
		{
			T target = obj as T;
			if (target != null)
				ApplyTo (target);
		}

		public override void DetachFrom (object obj)
		{
			T target = obj as T;
			if (target != null)
				Reverse (target);
		}

		public void ApplyTo (T target)
		{
			for (int i = 0; i < effects.Count; i++)
				effects [i].ApplyTo (target);
		}

		public void Reverse (T target)
		{
			for (int i = 0; i < effects.Count; i++)
				effects [i].Reverse (target);
		}
	
	
	
	}
}


