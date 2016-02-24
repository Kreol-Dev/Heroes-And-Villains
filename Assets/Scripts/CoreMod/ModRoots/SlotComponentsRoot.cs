using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UIO;
using System.Collections.Generic;

namespace CoreMod
{
	[RootDependencies (typeof(ModsManager))]
	public class SlotComponentsRoot : ModRoot
	{
		List<Type> types;

		protected override void CustomSetup ()
		{
			Type attrType = typeof(ASlotComponent);
			var MM = Find.Root<ModsManager> ();
			int ID = 0;

			var cmps = from type  in MM.GetAllTypes ()
			           let cmp = Attribute.GetCustomAttribute (type, attrType, true) as ASlotComponent
			           where cmp != null
			           select new SlotPair (){ Name = cmp.Name, Type = type };
			ITable table = MM.GetTable ("component");
			types = new List<Type> ();
			foreach (var cmp in cmps)
			{

				cmp.ID = ID++;
				types.Add (cmp.Type);
				table.Set (cmp.Name, cmp.ID);
			}

			MM.SetTableAsGlobal ("component");

			Fulfill.Dispatch ();
		}

		public Type GetTypeByID (int id)
		{
			return types [id];
		}

		class SlotPair
		{
			public int ID;
			public string Name;
			public Type Type;
		}

	}

	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public class ASlotComponent : System.Attribute
	{

		public string Name { get; internal set; }

		public ASlotComponent (string name)
		{
			Name = name;
		}
	}
}